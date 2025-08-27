using UnityEngine;
using TMPro;

public class GemCollection : MonoBehaviour
{
    public int Gems = 0;
    [SerializeField] private TextMeshProUGUI gem_text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            Gems++;
            UpdateGemText();

            // Save gem's position as collected
            Vector3 pos = other.transform.position;
            string key = $"Gem_{pos.x}_{pos.y}_{pos.z}";
            PlayerPrefs.SetInt(key, 1);

            
            Destroy(other.gameObject);
        }
    }

    public void UpdateGemText()
    {
        gem_text.text = "" + Gems;
    }

    // remove already collected gems on scene load
    public void RemoveCollectedGems()
    {
        foreach (GameObject gem in GameObject.FindGameObjectsWithTag("Gem"))
        {
            Vector3 pos = gem.transform.position;
            string key = $"Gem_{pos.x}_{pos.y}_{pos.z}";
            if (PlayerPrefs.HasKey(key))
            {
                Destroy(gem);
            }
        }
    }
}
