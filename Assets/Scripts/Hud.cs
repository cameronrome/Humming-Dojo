using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;
    
    public void Display(string text)
    {
        displayText.text = text;
    }
}
