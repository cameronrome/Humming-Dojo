using UnityEngine;

public class StartHumming : MonoBehaviour, Interactable
{
    //[SerializeField] CameraFollow cameraFollow;
    //[SerializeField] PlayerController playerController;
    [SerializeField] private Player playerMovement;
    [SerializeField] private GateOld gate;
    [SerializeField] private CameraManager cameraManager;

    private bool interacting = false;

    public void Interact()
    {
        if (!interacting && gate.inRange) //hit interact once
        {
            interacting = true;

            //playerController.DisableMovement(); //for original movement controller
            playerMovement.DisableMovement(); //for Jerry's new movement controller

            //cameraFollow.StartCombatZoom(gate.GetDirection()); //for old camera holder
            cameraManager.SwitchToShoulderCam(); //for cinemachine

            gate.ShowHumDial();
        }
        else if (gate.inRange) //hit interact again, leaving the humming screen
        {
            interacting = false;

            //playerController.EnableMovement(); //for original movement controller
            playerMovement.EnableMovement(); //for Jerry's new movement controller

            //cameraFollow.EndCombatZoom();
            cameraManager.SwitchToBirdCam();

            gate.HideHumDial();
        }
        
    }

    public void Update()
    {
        if (gate.isOpen())
        {
            interacting = false;
            playerMovement.EnableMovement();
            cameraManager.SwitchToBirdCam();
        }
    }
}
