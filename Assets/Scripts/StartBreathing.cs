using NUnit.Framework.Constraints;
using UnityEngine;

public class StartBreathing : MonoBehaviour, Interactable
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Player playerMovement;
    [SerializeField] private BreathDial breathDial;
    [SerializeField] private Vector3 cameraOffset = new(-2.5f, -2.6400001f, -0.75f);
    [SerializeField] private Hud hud;

    private bool interacting = false;
    private bool playerInRange = false;

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
    }

    public void Interact()
    {
        if (!interacting && playerInRange) //hit interact once
        {
            interacting = true;
            playerMovement.DisableMovement(); //for Jerry's new movement controller
            breathDial.gameObject.SetActive(true);
            cameraManager.SwitchToZoomInCam();
            BackgroundMusic.Instance.Pause();
        }
        else if (playerInRange) //hit interact again, leaving the humming screen
        {
            StopBreathing();
        }
    }

    private void Start()
    {
        breathDial.onBreathPass += StopBreathing;
    }

    private void StopBreathing()
    {
        interacting = false;
        playerMovement.EnableMovement();
        cameraManager.SwitchToBirdCam();
        breathDial.gameObject.SetActive(false);
        hud.Display("Press [e] to interact");
        BackgroundMusic.Instance.Play();
    }
}
