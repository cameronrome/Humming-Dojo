using UnityEngine;

public class StartBreathing : MonoBehaviour, Interactable
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Player playerMovement;
    [SerializeField] private BreathDial breathDial;
    [SerializeField] private Vector3 cameraOffset = new(-2.5f, -2.6400001f, -0.75f);
    [SerializeField] private Hud hud;

    private bool interacting;
    private bool playerInRange;
    private bool breathPassSubscribed;

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
        if (interacting) StopBreathing();   // optional: auto-exit if they walk away
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
            StopBreathing();
        }
    }

    // Removed Start()

    private void SubscribeBreathPass()
    {
        if (breathPassSubscribed) return;
        // idempotent pattern to avoid duplicates if something calls this twice
        breathDial.onBreathPass -= StopBreathing;
        breathDial.onBreathPass += StopBreathing;
        breathPassSubscribed = true;
    }

    private void UnsubscribeBreathPass()
    {
        if (!breathPassSubscribed) return;
        breathDial.onBreathPass -= StopBreathing;
        breathPassSubscribed = false;
    }

    private void StopBreathing()
    {
        UnsubscribeBreathPass();

        interacting = false;
        playerMovement.EnableMovement();
        cameraManager.SwitchToBirdCam();
        breathDial.gameObject.SetActive(false);
        hud.Display("Press [e] to interact");
        BackgroundMusic.Instance.Play();
    }

    private void OnDisable() { UnsubscribeBreathPass(); }
    private void OnDestroy() { UnsubscribeBreathPass(); }
}
