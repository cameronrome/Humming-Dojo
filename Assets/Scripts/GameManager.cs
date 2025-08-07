using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject pauseMenuUI;
    public bool isPaused = false;

    [Header("Rotating Image 1")]
    public RectTransform rotatingImage1;
    public float rotationSpeed1 = 20f;

    [Header("Rotating Image 2")]
    public RectTransform rotatingImage2;
    public float rotationSpeed2 = -20f;

    [Header("Rotating Image 3")]
    public RectTransform rotatingImage3;
    public float rotationSpeed3 = 20f;

    public GameObject loadGameButton;

    public void mainScene()
    {
        SceneManager.LoadScene("TutorialBlockOut");
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll(); 
        SceneManager.LoadScene("TutorialBlockOut");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("TutorialBlockOut");
    }


    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    void Start()
    {
        // Disable the Load Game button if no save exists
        if (!PlayerPrefs.HasKey("CurrentHealth"))
        {
            loadGameButton.GetComponent<Button>().interactable = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }

        if (rotatingImage1 != null)
        {
            rotatingImage1.Rotate(0f, 0f, rotationSpeed1 * Time.unscaledDeltaTime);

        }

        if (rotatingImage2 != null)
        {
            rotatingImage2.Rotate(0f, 0f, rotationSpeed2 * Time.unscaledDeltaTime);
        }

        if (rotatingImage3 != null)
        {
            rotatingImage3.Rotate(0f, 0f, rotationSpeed3 * Time.unscaledDeltaTime);
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        // Optionally re-enable player control script here
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        // Optionally disable player control script here
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // reset before switching scenes
        SceneManager.LoadScene("MainMenuScene");
    }

}
