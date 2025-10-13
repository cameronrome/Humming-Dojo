using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera birdCam; //3
    [SerializeField] private CinemachineCamera shoulderCam; //2
    [SerializeField] private CinemachineCamera breatheCam; //1
    [SerializeField] private CinemachineCamera zoomOutCam; //4
    [SerializeField] private CinemachineCamera zoomInCam; //5

    public void SwitchToShoulderCam()
    {
        birdCam.Priority = 3;
        shoulderCam.Priority = 10;
        breatheCam.Priority = 1;
        zoomOutCam.Priority = 4;
        zoomInCam.Priority = 5;
    }

    public void SwitchToBirdCam()
    {
        birdCam.Priority = 10;
        shoulderCam.Priority = 2;
        breatheCam.Priority = 1;
        zoomOutCam.Priority = 4;
        zoomInCam.Priority = 5;
    }

    public void SwitchToZoomInCam()
    {
        birdCam.Priority = 3;
        shoulderCam.Priority = 2;
        breatheCam.Priority = 10;
        zoomOutCam.Priority = 4;
        zoomInCam.Priority = 5;
    }

    public void SwitchtoZoomOutCam()
    {
        birdCam.Priority = 3;
        shoulderCam.Priority = 2;
        breatheCam.Priority = 1;
        zoomOutCam.Priority = 10;
        zoomInCam.Priority = 5;
    }

    public void SwitchtoSmallCam()
    {
        birdCam.Priority = 3;
        shoulderCam.Priority = 2;
        breatheCam.Priority = 1;
        zoomOutCam.Priority = 4;
        zoomInCam.Priority = 10;
    }


}