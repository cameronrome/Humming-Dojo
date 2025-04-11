using UnityEngine;

public class HumPitchVisualizer : MonoBehaviour
{

    private const int sampleSize = 1024;
    private float[] spectrum = new float[sampleSize];

    public float pitchThresholdLow = 200f;
    public float pitchThresholdHigh = 600f;
    public int numberOfBars = 128;
    public GameObject barPrefab;
    public float scaleMultiplier = 50f;
    public float radius = 10f;

    private GameObject[] bars;
    private string micName;
    private AudioClip micClip;
    private AudioSource micAudioSource;
    private float[] spectrumData;

    public Color lowPitchColor = Color.blue;
    public Color midPitchColor = Color.green;
    public Color highPitchColor = Color.red;


    void Start()
    {
        // Start microphone
        micName = Microphone.devices[0];
        micClip = Microphone.Start(micName, true, 10, 44100);

        spectrumData = new float[2048];
        bars = new GameObject[numberOfBars];

        // Create an AudioSource and assign the mic input
        micAudioSource = gameObject.AddComponent<AudioSource>();
        micAudioSource.clip = micClip;
        micAudioSource.loop = true;

        // Wait until mic starts before playing
        while (!(Microphone.GetPosition(micName) > 0)) { }
        micAudioSource.Play();

        for (int i = 0; i < numberOfBars; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfBars;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            bars[i] = Instantiate(barPrefab, pos, Quaternion.identity);
            bars[i].transform.LookAt(transform.position);
            bars[i].transform.parent = transform;

            // Create unique material instance
            Renderer rend = bars[i].GetComponent<Renderer>();
            rend.material = new Material(rend.sharedMaterial);
        }
    }

    void Update()
    {
        micAudioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        for (int i = 0; i < numberOfBars; i++)
        {
            float intensity = spectrumData[i] * scaleMultiplier;
            float clampedHeight = Mathf.Clamp(intensity, 0.1f, 50f);

            // Scale the bar
            Vector3 scale = bars[i].transform.localScale;
            scale.y = Mathf.Lerp(scale.y, clampedHeight, Time.deltaTime * 30);
            bars[i].transform.localScale = scale;
        }
            
        float freq = GetDominantFrequency();

        // Debug
        Debug.Log("Frequency: " + freq);

        // Visual triggers based on pitch
        if (freq > 0f && freq < pitchThresholdLow)
        {
            TriggerLowPitchVisual();
        }
        else if (freq >= pitchThresholdLow && freq < pitchThresholdHigh)
        {
            TriggerMidPitchVisual();
        }
        else if (freq >= pitchThresholdHigh)
        {
            TriggerHighPitchVisual();
        }
    }

    float GetDominantFrequency()
    {
        int maxIndex = 0;
        float maxVal = 0f;

        float threshold = 0.01f; // Filter out noise
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > maxVal && spectrum[i] > threshold)
            {
                maxVal = spectrum[i];
                maxIndex = i;
            }
        }
        float freq = maxIndex * AudioSettings.outputSampleRate / 2 / spectrum.Length;
        return freq;
    }

    void TriggerLowPitchVisual()
    {
        Debug.Log("Low Pitch Visual");

        foreach (GameObject bar in bars)
        {
            Renderer rend = bar.GetComponent<Renderer>();
            rend.material.color = lowPitchColor;
        }
    }

    void TriggerMidPitchVisual()
    {
        Debug.Log("Mid Pitch Visual");
        foreach (GameObject bar in bars)
        {
            Renderer rend = bar.GetComponent<Renderer>();
            rend.material.color = midPitchColor;
        }
    }


    void TriggerHighPitchVisual()
    {
        Debug.Log("High Pitch Visual");
        foreach (GameObject bar in bars)
        {
            Renderer rend = bar.GetComponent<Renderer>();
            rend.material.color = highPitchColor;
        }
    }

}
