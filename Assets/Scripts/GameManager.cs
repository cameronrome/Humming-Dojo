using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void mainScene()
    {
        SceneManager.LoadScene("TutorialBlockOut");
    }
}
