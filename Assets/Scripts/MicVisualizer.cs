using UnityEngine;

public class MicVisualizer : MonoBehaviour
{
    public GameObject barPrefab;
    public int numberOfBars = 128;
    public float scaleMultiplier = 50f;
    public float radius = 10f;
    public Gradient colorGradient;

    private GameObject[] bars;
    private AudioClip micClip;
    private string micName;
    private float[] spectrumData;
    private AudioSource micAudioSource;

    float GetMicVolume()
    {
        float[] data = new float[128];
        int micPosition = Microphone.GetPosition(micName) - data.Length + 1;
        if (micPosition < 0) return 0;
        micClip.GetData(data, micPosition);

        float sum = 0;
        foreach (float sample in data)
        {
            sum += Mathf.Abs(sample);
        }

        return sum / data.Length;
    }


    void Start()
    {
        // Start microphone
        micName = Microphone.devices[0];
        micClip = Microphone.Start(micName, true, 10, 44100);

        // Create an AudioSource and assign the mic input
        micAudioSource = gameObject.AddComponent<AudioSource>();
        micAudioSource.clip = micClip;
        micAudioSource.loop = true;

        // Wait until mic starts before playing
        while (!(Microphone.GetPosition(micName) > 0)) { }
        micAudioSource.Play();


        spectrumData = new float[1024];
        bars = new GameObject[numberOfBars];

        // Create visual bars
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
        // Get audio data from the microphone
        micAudioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);

        for (int i = 0; i < numberOfBars; i++)
        {
            float intensity = spectrumData[i] * scaleMultiplier;
            float clampedHeight = Mathf.Clamp(intensity, 0.1f, 50f);

            // Scale the bar
            Vector3 scale = bars[i].transform.localScale;
            scale.y = Mathf.Lerp(scale.y, clampedHeight, Time.deltaTime * 30);
            bars[i].transform.localScale = scale;

            // Color
            float frequencyRatio = (float)i / (numberOfBars - 1);
            Color barColor = colorGradient.Evaluate(frequencyRatio);

            float brightness = Mathf.Clamp01(intensity * 10);
            barColor *= brightness;

            Renderer rend = bars[i].GetComponent<Renderer>();
            rend.material.color = barColor;
        }
    }
}
