using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject previousMenuPanel;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;

    public AudioSource musicSource;

    void Start()
    {
        // Load saved volume levels
        float masterVol = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);

        masterVolumeSlider.value = masterVol;
        musicVolumeSlider.value = musicVol;

        AudioListener.volume = masterVol;
        if (musicSource != null)
            musicSource.volume = musicVol;

        // Hook up the listeners
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void OpenOptions()
    {
        previousMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        previousMenuPanel.SetActive(true);
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = volume;

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}
