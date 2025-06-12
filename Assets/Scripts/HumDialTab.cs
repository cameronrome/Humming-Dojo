using UnityEngine;
using UnityEngine.UI;

public class HumDialTab : MonoBehaviour
{
    [SerializeField] private Sprite baseSprite;
    [SerializeField] private Sprite glowSprite;

    public void Glow()
    {
        Image image = GetComponent<Image>();
        image.sprite = glowSprite;
    }

    public void ResetGlow()
    {
        Image image = GetComponent<Image>();
        image.sprite = baseSprite;
    }
}
