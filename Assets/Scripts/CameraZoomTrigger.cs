using UnityEditor.UI;
using UnityEngine;

public class CameraZoomTrigger : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private bool zoomOutZone = true;
    [SerializeField] private bool chaseZone = false;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (chaseZone)
            {
                Debug.Log("Chasing");
                cameraManager.SwitchToChaseCam();
            }
            else if (zoomOutZone)
            {
                Debug.Log("Zoomed out!");
                cameraManager.SwitchToZoomOutCam();
            }
            else
            {
                Debug.Log("Zoomed in!");
                cameraManager.SwitchToZoomInCam();
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Back to default zoom");
            cameraManager.SwitchToBirdCam();
        }
    }
}
