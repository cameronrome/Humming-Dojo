using UnityEngine;

public class StartHumming : MonoBehaviour, Interactable
{
    [SerializeField] CameraFollow cameraFollow;
    //[SerializeField] PlayerController playerController;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Gate gate;

    private bool interacting = false;

    public void Interact()
    {
        if (!interacting && gate.inRange) //hit interact once
        {
            interacting = true;

            //playerController.DisableMovement(); //for original movement controller
            playerMovement.DisableMovement(); //for Jerry's new movement controller

            cameraFollow.StartCombatZoom(gate.GetDirection());
            gate.ShowHumDial();
        }
        else if (gate.inRange) //hit interact again, leaving the humming screen
        {
            interacting = false;

            //playerController.EnableMovement(); //for original movement controller
            playerMovement.EnableMovement(); //for Jerry's new movement controller

            cameraFollow.EndCombatZoom();
            gate.HideHumDial();
        }
        
    }
}
