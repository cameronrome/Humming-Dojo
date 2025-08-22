using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelWin : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayWinAnimation());
        }
    }

    IEnumerator PlayWinAnimation()
    {
        playerController.DisableMovement();

        yield return new WaitForSeconds(4f); //time to play animation
        //code to start player winning animation

        SceneManager.LoadScene("MainMenuScene");
    }
}
