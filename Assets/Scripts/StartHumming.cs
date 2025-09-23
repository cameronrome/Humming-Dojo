using UnityEngine;

public class StartHumming : MonoBehaviour, Interactable
{
    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] PlayerController playerController;
    [SerializeField] Gate gate;

    private bool interacting = false;

    public void Interact()
    {
        if (!interacting && gate.inRange) //hit interact once
        {
            interacting = true;
            playerController.DisableMovement();
            cameraFollow.StartCombatZoom(gate.GetDirection());
            gate.ShowHumDial();
        }
        else if (gate.inRange) //hit interact again, leaving the humming screen
        {
            interacting = false;
            playerController.EnableMovement();
            cameraFollow.EndCombatZoom();
            gate.HideHumDial();
        }
        
    }
}
