using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera birdCam; //3
    [SerializeField] private CinemachineCamera shoulderCam; //2
    [SerializeField] private CinemachineCamera zoomInCam; //1

    public void SwitchToShoulderCam()
    {
        birdCam.Priority = 3;
        shoulderCam.Priority = 5;
        zoomInCam.Priority = 1;
    }

    public void SwitchToBirdCam()
    {
        birdCam.Priority = 5;
        shoulderCam.Priority = 2;
        zoomInCam.Priority = 1;
    }

    public void SwitchToZoomInCam()
    {
        birdCam.Priority = 3;
        shoulderCam.Priority = 2;
        zoomInCam.Priority = 5;
    }


}