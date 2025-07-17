using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public Transform cameraTarget; 
    public float cameraMoveSpeed = 2f;
    public GameObject mainMenuUI;
    public MonoBehaviour playerController;
    public MonoBehaviour cameraFollowScript;
    public GameObject inventoryUI;


    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        StartCoroutine(MoveCameraToGameplay());
    }

    IEnumerator MoveCameraToGameplay()
    {
        Transform cam = Camera.main.transform;
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;

        float t = 0f;
        float duration = 1.5f;

        while (t < 1f)
        {
            t += Time.deltaTime * cameraMoveSpeed;
            cam.position = Vector3.Lerp(startPos, cameraTarget.position, t);
            cam.rotation = Quaternion.Slerp(startRot, cameraTarget.rotation, t);
            yield return null;
        }

        // Enable camera follow and player control
        cameraFollowScript.enabled = true;
        playerController.enabled = true;

        inventoryUI.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
