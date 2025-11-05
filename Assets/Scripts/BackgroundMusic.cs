using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance { get; private set; }

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
        if (GetComponent<AudioSource>().isPlaying) return;
        GetComponent<AudioSource>().Play();
    }

    public void Pause()
    {
        GetComponent<AudioSource>().Pause();
    }
}
