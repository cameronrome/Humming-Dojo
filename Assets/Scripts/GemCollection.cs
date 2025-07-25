using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GemCollection : MonoBehaviour
{
    private int Gems = 0;
    [SerializeField] private TextMeshProUGUI gem_text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            Gems++;
            Destroy(other.gameObject);

            gem_text.text = "" + Gems;
        }
    }
}
