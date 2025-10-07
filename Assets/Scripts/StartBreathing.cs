using UnityEngine;

public class StartBreathing : MonoBehaviour, Interactable
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private GameObject breathDial;

    [SerializeField] private Vector3 cameraOffset = new(-2.5f, -2.6400001f, -0.75f);

    private bool interacting = false;
    public void Interact()
    {
        if (!interacting) //hit interact once
        {
            interacting = true;
            playerMovement.DisableMovement(); //for Jerry's new movement controller
            breathDial.SetActive(true);
            cameraManager.SwitchToZoomInCam();


        }
        else //hit interact again, leaving the humming screen
        {
            interacting = false;
            //playerController.EnableMovement(); //for original movement controller
            playerMovement.EnableMovement(); //for Jerry's new movement controller
            breathDial.SetActive(false);
            cameraManager.SwitchToBirdCam();

        }
    }
}
