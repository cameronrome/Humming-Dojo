using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject controlsMenuUI;
    public GameObject settingsMenuUI;
    public GameObject exitLevelUI;
    public bool isPaused = false;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        exitLevelUI.SetActive(false);
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;

        // Optionally re-enable player control script here
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(null);

        // Optionally disable player control script here
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // reset before switching scenes
        SceneManager.LoadScene("MainMenuScene");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
