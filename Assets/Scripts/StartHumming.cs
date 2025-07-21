using UnityEngine;

public class StartHumming : MonoBehaviour, Interactable
{
    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] PlayerController playerController;

    private bool interacting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Interact()
    {
        if (!interacting) //hit interact once
        {
            interacting = true;
            playerController.DisableMovement();
            cameraFollow.StartCombatZoom();
        }
        else //hit interact again, leaving the humming screen
        {
            interacting = false;
            playerController.EnableMovement();
            cameraFollow.EndCombatZoom();
        }
        
    }
}
