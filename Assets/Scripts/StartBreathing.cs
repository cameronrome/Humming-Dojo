using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class StartBreathing : MonoBehaviour, Interactable
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Player playerMovement;
    [SerializeField] private BreathDial breathDial;
    [SerializeField] private Vector3 cameraOffset = new(-2.5f, -2.6400001f, -0.75f);
    [SerializeField] private Hud hud;

    [SerializeField] private GameObject gemPrefab;
    [SerializeField] private Transform gemSpawnPoint;

    private bool interacting;
    private bool playerInRange;
    private bool breathPassSubscribed;

    private int gemCount = 12; //number of gems to spawn
    private float launchSpeed = 6; //angle above horizontal
    private float verticalMultiplier = 0.6f; //target distance landing from center
    private float upOffset = 1f; //spawn slightly above plant

    public void OnTriggerEnter(Collider collider)
    {
        if (!collider.GetComponent<Player>()) return;
        playerInRange = true;
        hud.Display("Press [e] to interact");
    }

    public void OnTriggerExit(Collider collider)
    {
        if (!collider.GetComponent<Player>()) return;
        playerInRange = false;
        hud.Display("");
        if (interacting)
        {
            EndBreathingSession(success: false);   // optional: auto-exit if they walk away
        }
    }

    public void Interact()
    {
        if (!playerInRange) return;

        if (!interacting) // first press → enter breathing
        {
            interacting = true;
            SubscribeBreathPass();
            playerMovement.DisableMovement();
            breathDial.gameObject.SetActive(true);
            cameraManager.SwitchToZoomInCam();
            BackgroundMusic.Instance.Pause();
        }
        else              // second press → exit breathing
        {
            EndBreathingSession(success: false);
        }
    }

    // Removed Start()

    private void SubscribeBreathPass()
    {
        if (breathPassSubscribed) return;
        // idempotent pattern to avoid duplicates if something calls this twice
        breathDial.onBreathPass -= OnBreathPass;
        breathDial.onBreathPass += OnBreathPass;
        breathPassSubscribed = true;
    }

    private void UnsubscribeBreathPass()
    {
        if (!breathPassSubscribed) return;

        breathDial.onBreathPass -= OnBreathPass;
        breathPassSubscribed = false;
    }

    private void OnBreathPass()
    {
        if (!interacting) return;

        //success -> spawn gems and exit
        SpawnGems();
        EndBreathingSession(success: true);
    }

    private void SpawnGems()
    {
        Debug.Log("spawning gems");

        if (gemPrefab == null)
        {
            Debug.LogWarning("Gem prefab not assigned!");
            return;
        }

        Vector3 origin = gemSpawnPoint != null ? gemSpawnPoint.position : transform.position;
        origin += Vector3.up * upOffset;

        for (int i = 0; i < gemCount; i++)
        {
            float degrees = (360f / gemCount) * i;

            Quaternion rotation = Quaternion.Euler(0, degrees, 0);
            Vector3 dir = rotation * Vector3.forward;

            // Simple, controllable velocity
            Vector3 velocity =
                dir * launchSpeed +
                Vector3.up * (launchSpeed * verticalMultiplier);

            GameObject gem = Instantiate(gemPrefab, origin, Quaternion.identity);
            Rigidbody rb = gem.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = velocity;
                rb.linearDamping = 1.5f;               // helps them slow down
                rb.angularDamping = 1.5f;
            }
            else
            {
                Debug.LogWarning("Gem prefab lacks Rigidbody!");
            }
        }
    }

    private void EndBreathingSession(bool success)
    {
        UnsubscribeBreathPass();

        interacting = false;
        playerMovement.EnableMovement();
        cameraManager.SwitchToBirdCam();
        breathDial.gameObject.SetActive(false);

        if (playerInRange)
        {
            hud.Display("Press [e] to interact");
        }
        else
        {
            hud.Display("");
        }    
        BackgroundMusic.Instance.Play();
    }

    private void OnDisable() { UnsubscribeBreathPass(); }
    private void OnDestroy() { UnsubscribeBreathPass(); }
}
