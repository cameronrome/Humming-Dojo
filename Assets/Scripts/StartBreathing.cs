using UnityEngine;

public class StartBreathing : MonoBehaviour, Interactable
{
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private BreathDial breathDial;

    private bool interacting = false;

    public void Interact()
    {
        if (!interacting) //hit interact once
        {
            interacting = true;
            playerController.DisableMovement();
            breathDial.gameObject.SetActive(true);
            breathDial.onBreathPass += CloseBreathDial;
            cameraFollow.StartBreathingZoom();
        }
        else //hit interact again, leaving the humming screen
        {
            CloseBreathDial();
        }
    }

    private void CloseBreathDial()
    {
        interacting = false;
        playerController.EnableMovement();
        breathDial.gameObject.SetActive(false);
        breathDial.onBreathPass -= CloseBreathDial;
        breathDial.Reset();
        cameraFollow.StopBreathingZoom();
    }
}
