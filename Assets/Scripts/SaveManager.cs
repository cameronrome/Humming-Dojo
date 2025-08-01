using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveManager : MonoBehaviour
{
    [Header("References")]
    public Health playerHealth;
    public GemCollection gemCollector;

    [Header("Optional Flags")]
    public bool decisionMade = false;

    void Start()
    {
        LoadGame();
    }


    public void SaveGame()
    {

        Vector3 pos = playerHealth.transform.position;
        PlayerPrefs.SetFloat("PlayerPosX", pos.x);
        PlayerPrefs.SetFloat("PlayerPosY", pos.y);
        PlayerPrefs.SetFloat("PlayerPosZ", pos.z);

        PlayerPrefs.SetFloat("CurrentHealth", playerHealth.GetCurrentHealth());
        PlayerPrefs.SetFloat("MaxHealth", playerHealth.GetMaxHealth());
        PlayerPrefs.SetInt("Gems", gemCollector.Gems);
        PlayerPrefs.SetInt("DecisionMade", decisionMade ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("Game Saved!");

        SceneManager.LoadScene("MainMenuScene"); 
    }


    public void LoadGame()
    {
        gemCollector.UpdateGemText();

        if (PlayerPrefs.HasKey("CurrentHealth"))
        {
            
            float loadedCurrentHealth = PlayerPrefs.GetFloat("CurrentHealth");
            float loadedMaxHealth = PlayerPrefs.GetFloat("MaxHealth");
            int loadedGems = PlayerPrefs.GetInt("Gems");
            bool loadedDecision = PlayerPrefs.GetInt("DecisionMade") == 1;

            
            SetPrivateField(playerHealth, "current_health", loadedCurrentHealth);
            SetPrivateField(playerHealth, "max_health", loadedMaxHealth);

            gemCollector.Gems = loadedGems;
            gemCollector.SendMessage("UpdateGemText", SendMessageOptions.DontRequireReceiver);

            decisionMade = loadedDecision;

            Debug.Log("Game Loaded!");
        }

        if (PlayerPrefs.HasKey("PlayerPosX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");
            playerHealth.transform.position = new Vector3(x, y, z);
        }

        else
        {
            Debug.LogWarning("No saved data found!");
        }
    }

    
    private void SetPrivateField<T>(T obj, string fieldName, object value)
    {
        var field = typeof(T).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            field.SetValue(obj, value);
    }
}
