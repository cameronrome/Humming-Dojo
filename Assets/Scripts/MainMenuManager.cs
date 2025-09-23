using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Rotating Image 1")]
    public RectTransform rotatingImage1;
    public float rotationSpeed1 = 20f; 

    [Header("Rotating Image 2")]
    public RectTransform rotatingImage2;
    public float rotationSpeed2 = -20f; 

    void Update()
    {
        if (rotatingImage1 != null)
        {
            rotatingImage1.Rotate(0f, 0f, rotationSpeed1 * Time.deltaTime);
        }

        if (rotatingImage2 != null)
        {
            rotatingImage2.Rotate(0f, 0f, rotationSpeed2 * Time.deltaTime);
        }
    }
}
