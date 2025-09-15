using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    public GameObject barPrefab;
    public int numberOfBars = 64;
    public float scaleMultiplier = 50f;
    public float radius = 10f;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spectrumData = new float[1024];
        bars = new GameObject[numberOfBars];

        for (int i = 0; i < numberOfBars; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfBars;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            bars[i] = Instantiate(barPrefab, pos, Quaternion.identity);
            bars[i].transform.LookAt(transform.position);
            bars[i].transform.parent = transform;

            // Assign unique material instance
            Renderer rend = bars[i].GetComponent<Renderer>();
            rend.material = new Material(rend.sharedMaterial);
        }

        audioSource.Play();
    }

    void Update()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);

        for (int i = 0; i < numberOfBars; i++)
        {
            float intensity = spectrumData[i] * scaleMultiplier;
            float clampedHeight = Mathf.Clamp(intensity, 0.1f, 50f);

            // Scale
            Vector3 scale = bars[i].transform.localScale;
            scale.y = Mathf.Lerp(scale.y, clampedHeight, Time.deltaTime * 30);
            bars[i].transform.localScale = scale;

            // Color based on position in frequency range
            float frequencyRatio = (float)i / (numberOfBars - 1);
            Color barColor = colorGradient.Evaluate(frequencyRatio);

            // Modify color intensity
            float brightness = Mathf.Clamp01(intensity * 10);
            barColor *= brightness;

            // set color
            Renderer rend = bars[i].GetComponent<Renderer>();
            rend.material.color = barColor;
        }
    }
}
