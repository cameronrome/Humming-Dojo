using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelWin : MonoBehaviour
{
    [SerializeField] private Player player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayWinAnimation());
        }
    }

    IEnumerator PlayWinAnimation()
    {
        player.DisableMovement();

        yield return new WaitForSeconds(2f); //time to play animation
        //code to start player winning animation

        SceneManager.LoadScene("MainMenuScene");
    }
}
