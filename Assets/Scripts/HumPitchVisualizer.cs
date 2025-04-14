using UnityEngine;

public class HumPitchVisualizer : MonoBehaviour
{
    private const int sampleSize = 1024;
    private float[] samples = new float[sampleSize];
    private float[] spectrum = new float[sampleSize];

    public float pitchThresholdLow = 70f;
    public float pitchThresholdHigh = 100f;
    public int numberOfBars = 128;
    public GameObject barPrefab;
    public float scaleMultiplier = 50f;
    public float radius = 10f;

    private GameObject[] bars;
    private string micName;
    private AudioClip micClip;

    public Color lowPitchColor = Color.blue;
    public Color midPitchColor = Color.green;
    public Color highPitchColor = Color.red;

    private float[] smoothedSpectrum = new float[sampleSize];
    public float smoothingFactor = 0.5f; // between 0 (no smoothing) and 1 (no movement)

    private int micFrequency = 44100;

    void Start()
    {
        // Start microphone
        micName = Microphone.devices[0];
        micClip = Microphone.Start(micName, true, 1, micFrequency); // 1-second loop is enough for real-time

        bars = new GameObject[numberOfBars];

        for (int i = 0; i < numberOfBars; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfBars;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            bars[i] = Instantiate(barPrefab, pos, Quaternion.identity);
            bars[i].transform.LookAt(transform.position);
            bars[i].transform.parent = transform;

            Renderer rend = bars[i].GetComponent<Renderer>();
            rend.material = new Material(rend.sharedMaterial); // Make unique instance
        }
    }

    void Update()
    {
        int micPosition = Microphone.GetPosition(micName) - sampleSize;
        if (micPosition < 0) return;

        micClip.GetData(samples, micPosition); // Get microphone data directly

        AudioFFT(samples, spectrum); // Process FFT on microphone data

        // apply Smoothing and update bars on spectrum data
        for (int i = 0; i < sampleSize; i++)
        {
            smoothedSpectrum[i] = Mathf.Lerp(smoothedSpectrum[i], spectrum[i], 1 - smoothingFactor);
        }

        for (int i = 0; i < numberOfBars; i++)
        {
            int spectrumIndex = Mathf.FloorToInt((float)i / numberOfBars * sampleSize);
            float intensity = smoothedSpectrum[spectrumIndex] * scaleMultiplier;
            float clampedHeight = Mathf.Clamp(intensity, 0.1f, 50f);

            Vector3 scale = bars[i].transform.localScale;
            scale.y = Mathf.Lerp(scale.y, clampedHeight, Time.deltaTime * 30);
            bars[i].transform.localScale = scale;
        }

        float freq = GetDominantFrequency(); // Get the dominant frequency
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

    void AudioFFT(float[] data, float[] output)
    {
        // Apply Unity's built-in FFT (spectrum analysis) on mic samples
        // Here we use Mathf.Abs for a basic spectrum estimation
        for (int i = 0; i < data.Length; i++)
        {
            output[i] = Mathf.Abs(data[i]); // Basic approach, better FFT libraries can be used for better results
        }
    }

    float GetDominantFrequency()
    {
        float threshold = 0.01f;

        // Only analyze from ~80 Hz to ~1000 Hz
        int minIndex = Mathf.FloorToInt(80f * sampleSize * 2 / micFrequency);
        int maxIndex = Mathf.FloorToInt(1000f * sampleSize * 2 / micFrequency);

        for (int i = minIndex; i < maxIndex; i++)
        {
            if (smoothedSpectrum[i] > threshold)
            {
                float freq = i * micFrequency / 2f / sampleSize;
                return freq; // Return first strong peak = likely the fundamental
            }
        }

        return 0f;
    }

    void TriggerLowPitchVisual()
    {
        Debug.Log("Low Pitch Visual");
        foreach (GameObject bar in bars)
        {
            bar.GetComponent<Renderer>().material.color = lowPitchColor;
        }
    }

    void TriggerMidPitchVisual()
    {
        Debug.Log("Mid Pitch Visual");
        foreach (GameObject bar in bars)
        {
            bar.GetComponent<Renderer>().material.color = midPitchColor;
        }
    }

    void TriggerHighPitchVisual()
    {
        Debug.Log("High Pitch Visual");
        foreach (GameObject bar in bars)
        {
            bar.GetComponent<Renderer>().material.color = highPitchColor;
        }
    }
}
