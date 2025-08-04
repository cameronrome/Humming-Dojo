using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    public GameObject savePromptUI;



    void Start()
    {
        if (savePromptUI != null)
            savePromptUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            savePromptUI.SetActive(true);

            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            savePromptUI.SetActive(false);
            
        }
    }

    



}
