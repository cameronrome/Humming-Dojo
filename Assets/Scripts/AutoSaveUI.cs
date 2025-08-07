using UnityEngine;
using TMPro;
using System.Collections;

public class AutoSaveUI : MonoBehaviour
{
    public TextMeshProUGUI autoSaveText;
    public float fadeDuration = 1f;
    public float visibleDuration = 1.5f;

    void Awake()
    {
        if (autoSaveText == null)
            autoSaveText = GetComponent<TextMeshProUGUI>();
    }

    public void ShowAutoSaveMessage()
    {
        StopAllCoroutines();
        StartCoroutine(FadeAutoSaveText());
    }

    IEnumerator FadeAutoSaveText()
    {
        // Show instantly
        autoSaveText.alpha = 1f;

        // Wait before fading
        yield return new WaitForSeconds(visibleDuration);

        // Fade out
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            autoSaveText.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        autoSaveText.alpha = 0f;
    }
}
