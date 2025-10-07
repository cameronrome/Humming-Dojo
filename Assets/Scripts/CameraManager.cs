using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera birdCam;
    [SerializeField] private CinemachineCamera shoulderCam;

    public void SwitchToShoulderCam()
    {
        birdCam.Priority = 10;
        shoulderCam.Priority = 20;
    }

    public void SwitchToBirdCam()
    {
        birdCam.Priority = 20;
        shoulderCam.Priority = 10;
    }
}