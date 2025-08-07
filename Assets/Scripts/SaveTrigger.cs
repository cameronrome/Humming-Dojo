using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    public GameObject savePromptUI;
    private SaveManager saveManager;

    void Start()
    {
        if (savePromptUI != null)
            savePromptUI.SetActive(false);

        // Grab reference to SaveManager in the scene
        saveManager = FindObjectOfType<SaveManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Show "progress autosaved" text
            FindObjectOfType<AutoSaveUI>().ShowAutoSaveMessage();

            // Actually save the game
            if (saveManager != null)
            {
                saveManager.SaveGame();
            }
            else
            {
                Debug.LogWarning("SaveManager not found!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && savePromptUI != null)
        {
            savePromptUI.SetActive(false);
        }
    }
}
