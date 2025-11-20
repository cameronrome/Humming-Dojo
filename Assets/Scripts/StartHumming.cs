using UnityEngine;

public class StartHumming : MonoBehaviour, Interactable
{
    [SerializeField] private Gate gate;
    [SerializeField] private Player player;
    [SerializeField] private CameraManager cameraManager;

    private bool interacting = false;

    public void Interact()
    {
        if (!interacting && gate.playerInRange) //hit interact once
        {
            interacting = true;
            player.DisableMovement(); //for Jerry's new movement controller
            cameraManager.SwitchToShoulderCam(); //for cinemachine
            gate.ShowHumDial();
        }
        else if (gate.playerInRange) //hit interact again, leaving the humming screen
        {
            interacting = false;
            player.EnableMovement(); //for Jerry's new movement controller
            cameraManager.SwitchToBirdCam();
            gate.HideHumDial();
        }
        
    }

    private void Start()
    {
        gate.OnOpen += StopHumming;
    }

    private void StopHumming()
    {
        interacting = false;
        player.EnableMovement();
        cameraManager.SwitchToBirdCam();
        gate.HideHumDial();
    }
}
