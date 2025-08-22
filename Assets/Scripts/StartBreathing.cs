using UnityEngine;

public class StartBreathing : MonoBehaviour, Interactable
{
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private GameObject breathDial;

    private bool interacting = false;
    public void Interact()
    {
        if (!interacting) //hit interact once
        {
            interacting = true;
            playerController.DisableMovement();
            breathDial.SetActive(true);
            cameraFollow.StartBreathingZoom();


        }
        else //hit interact again, leaving the humming screen
        {
            interacting = false;
            playerController.EnableMovement();
            breathDial.SetActive(false);
            cameraFollow.StopBreathingZoom();

        }
    }
}
