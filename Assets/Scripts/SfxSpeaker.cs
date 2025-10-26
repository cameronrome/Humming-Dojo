using UnityEngine;

public class SfxSpeaker : MonoBehaviour
{
    public static SfxSpeaker Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Play()
    {
        GetComponent<AudioSource>().Play();
    }
}
