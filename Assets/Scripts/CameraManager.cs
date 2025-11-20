using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera birdCam; //3
    [SerializeField] private CinemachineCamera shoulderCam; //2
    [SerializeField] private CinemachineCamera breatheCam; //1
    [SerializeField] private CinemachineCamera zoomOutCam; //4
    [SerializeField] private CinemachineCamera zoomInCam; //5
    [SerializeField] private CinemachineCamera chaseCam; //6

    public void SwitchToShoulderCam()
    {
        birdCam.Priority = 3;
        shoulderCam.Priority = 10;
        breatheCam.Priority = 1;
        zoomOutCam.Priority = 4;
        zoomInCam.Priority = 5;
        chaseCam.Priority = 1;
    }

    public void SwitchToBirdCam()
    {
        birdCam.Priority = 10;
        shoulderCam.Priority = 2;
        breatheCam.Priority = 1;
        zoomOutCam.Priority = 4;
        zoomInCam.Priority = 5;
        chaseCam.Priority = 1;
    }

    public void SwitchToZoomInCam()
    {
        birdCam.Priority = 3;
        shoulderCam.Priority = 2;
        breatheCam.Priority = 5;
        zoomOutCam.Priority = 4;
        zoomInCam.Priority = 10;
        chaseCam.Priority = 1;
    }

    public void SwitchToZoomOutCam()
    {
        birdCam.Priority = 3;
        shoulderCam.Priority = 2;
        breatheCam.Priority = 1;
        zoomOutCam.Priority = 10;
        zoomInCam.Priority = 5;
        chaseCam.Priority = 1;
    }

    public void SwitchToBreatheCam()
    {
        birdCam.Priority = 3;
        shoulderCam.Priority = 2;
        breatheCam.Priority = 10;
        zoomOutCam.Priority = 4;
        zoomInCam.Priority = 1;
        chaseCam.Priority = 1;
    }

    public void SwitchToChaseCam()
    {
        birdCam.Priority = 3;
        shoulderCam.Priority = 2;
        breatheCam.Priority = 10;
        zoomOutCam.Priority = 4;
        zoomInCam.Priority = 1;
        chaseCam.Priority = 20;
    }


}