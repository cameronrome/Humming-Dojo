using UnityEngine;

public class CameraZoomTrigger : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Zoomed out!");
            cameraFollow.ZoomOut();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Zoomed back in!");
            cameraFollow.ResetZoom();
        }
    }
}
