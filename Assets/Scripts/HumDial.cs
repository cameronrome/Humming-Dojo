using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class HumDial : MonoBehaviour
{
    [SerializeField] private Image marker;
    [SerializeField] private Image wave;
    [SerializeField] private Image timer;
    [SerializeField] private Image icon;
    [SerializeField] private Transform markerAnchor;
    [SerializeField] private List<Image> tabs;
    [SerializeField] private List<Sprite> iconSprites;
    [SerializeField] private List<int> keys;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private int numDots = 72;

    public UnityAction OnHumPass;

    private List<GameObject> dotTrail;
    private AudioClip micClip;
    private AudioSource micAudioSource;
    private AudioPitchEstimator pitchEstimator;
    public AudioMixerGroup micSilentGroup;
    private string micName;
    private float prevAngle;
    private float silentTimer;
    private float silentDur = 1.5f;

    private int keyIdx;
    private float keyTimer;
    private float keyDur = 1.5f;

    private List<Color> colors = new List<Color>()
    {
        new Color(252f / 255f, 110f / 255f, 102f / 255f, 0.8f),
        new Color(231f / 255f, 118f / 255f, 25f / 255f, 0.8f),
        new Color(209f / 255f, 198f / 255f, 26f / 255f, 0.8f),
        new Color(103f / 255f, 225f / 255f, 95f / 255f, 0.8f),
        new Color(11f / 255f, 168f / 255f, 207f / 255f, 0.8f),
        new Color(81f / 255f, 158f / 255f, 199f / 255f, 0.8f),
        new Color(180f / 255f, 32f / 255f, 239f / 255f, 0.8f)
    };

    public void SetKeys(List<int> keys)
    {
        new List<int>() { 2 };
        this.keys = keys;
    }

    public void Open()
    {
        gameObject.SetActive(true);
        keyIdx = 0;
        keyTimer = keyDur;
        promptTab(keys[keyIdx]);
        wave.transform.localPosition = new Vector3(0, -150, 0);
        wave.color = colors[keys[keyIdx]];
        timer.color = colors[keys[keyIdx]];
        timer.fillAmount = 0;
        icon.sprite = iconSprites[keys[keyIdx]];
    }

    public void Close()
    {
        gameObject.SetActive(false);
        keyIdx = 0;
    }

    private void promptTab(int idx)
    {
        foreach (Image tab in tabs)
        {
            tab.enabled = true;
        }

        if (idx >= 0 && idx < tabs.Count)
        {
            tabs[idx].enabled = false;
        }
    }

    private float? estimatePitch()
    {
        float pitch = pitchEstimator.Estimate(micAudioSource);

        if (float.IsNaN(pitch))
        {
            silentTimer -= Time.deltaTime;

            if (silentTimer < 0)
            {
                return pitch;
            }

            return null;
        } 
        
        silentTimer = silentDur;

        return pitch;
    }

    private void Start()
    {
        micName = Microphone.devices[0];
        micClip = Microphone.Start(micName, true, 1, 44100);

        micAudioSource = gameObject.AddComponent<AudioSource>();
        micAudioSource.clip = micClip;
        micAudioSource.loop = true;
        micAudioSource.outputAudioMixerGroup = micSilentGroup;

        while (!(Microphone.GetPosition(micName) > 0)) { }
        micAudioSource.Play();

        pitchEstimator = GetComponent<AudioPitchEstimator>();
        dotTrail = new List<GameObject>();

        for (int i = 0; i < numDots; i++)
        {
            dotTrail.Add(null);
        }
    }

    private void Update()
    {
        float? pitch = estimatePitch();

        if (pitch == null) return;

        float nextAngle = 0;

        if (!float.IsNaN((float)pitch))
        {
            float angleRange = 180;
            float angleOffset = 90;
            float freqRange = pitchEstimator.frequencyMax - pitchEstimator.frequencyMin;
            float pitchVal = (float)pitch - pitchEstimator.frequencyMin;

            nextAngle = pitchVal * angleRange / freqRange + angleOffset;
        }

        float targetAngle = Mathf.Lerp(prevAngle, nextAngle, 2 * Time.deltaTime);
        marker.transform.RotateAround(markerAnchor.position, -Vector3.forward, targetAngle - prevAngle);

        if (keyIdx < keys.Count)
        {
            float tabAngle = 180 / tabs.Count;
            float keyAngleStart = tabAngle * keys[keyIdx] + 90;
            float keyAngleEnd = keyAngleStart + tabAngle;

            if (targetAngle >= keyAngleStart && targetAngle <= keyAngleEnd)
            {
                keyTimer -= Time.deltaTime;
                wave.transform.localPosition = new Vector3(0, wave.transform.localPosition.y + (150 / keyDur) * Time.deltaTime, 0);
                timer.fillAmount += (1 / keyDur) * Time.deltaTime; 

                if (keyTimer <= 0 && keyIdx < keys.Count)
                {
                    keyIdx++;

                    if (keyIdx < keys.Count)
                    {
                        wave.color = colors[keys[keyIdx]];
                        timer.color = colors[keys[keyIdx]];
                        timer.fillAmount = 0;
                        icon.sprite = iconSprites[keys[keyIdx]];
                        promptTab(keys[keyIdx]);
                    }
                    else
                    {
                        wave.transform.localPosition = new Vector3(0, -150, 0);
                        timer.fillAmount = 0;
                        icon.color = new Color(0, 0, 0, 0);
                        promptTab(-1);
                        OnHumPass?.Invoke();
                    }
                }
            }
            else
            {
                keyTimer = keyDur;
                timer.fillAmount = 0;
                wave.transform.localPosition = new Vector3(0, -150, 0);
            }
        }

        for (int i = 0; i < numDots; i++)
        {
            float radius = 63f;
            float angleDeg = i * (360 / numDots);
            float angleOffset = -90f;
            float angleRad = Mathf.Deg2Rad * (angleDeg + angleOffset);
            float offset = 14f;

            if (targetAngle > angleDeg && dotTrail[i] == null)
            {
                dotTrail[i] = Instantiate(dotPrefab, transform);
                dotTrail[i].transform.localPosition = new Vector3(-radius * Mathf.Cos(angleRad), radius * Mathf.Sin(angleRad) - offset);
            }

            if (targetAngle < angleDeg && dotTrail[i] != null)
            {
                Destroy(dotTrail[i]);
                dotTrail[i] = null;
            }

            Color dotColor = Color.white;

            if (keyIdx < keys.Count)
            {
                dotColor = colors[keys[keyIdx]];
            }

            if (dotTrail[i] != null)
            {
                dotColor.a = (float)i / (float)numDots;
                dotTrail[i].GetComponent<Image>().color = dotColor;
            }
        }

        prevAngle = targetAngle;
    }
}
