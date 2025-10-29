using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class BreathDial : MonoBehaviour
{
    [SerializeField] private Image timer;
    [SerializeField] private Image wave;
    [SerializeField] private Image icon;
    [SerializeField] private Image marker;
    [SerializeField] private Transform markerAnchor;
    [SerializeField] private Sprite inhaleSprite;
    [SerializeField] private Sprite exhaleSprite;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Hud hud;
    [SerializeField] private float inhaleDur = 5f;
    [SerializeField] private float exhaleDur = 4f;
    [SerializeField] private int numDots = 72;

    public UnityAction onBreathPass;

    private List<GameObject> dotTrail;
    private AudioClip micClip;
    private AudioSource micAudioSource;
    private AudioPitchEstimator pitchEstimator;
    public AudioMixerGroup micSilentGroup;
    private string micName;
    private bool inhaling = false;

    public void Reset()
    {
        timer.fillAmount = 0;
        icon.sprite = inhaleSprite;
        inhaling = true;
        wave.transform.localPosition = new Vector3(0, -150, 0);
        marker.transform.localPosition = new Vector3(0, -70, 0);
        marker.transform.rotation = Quaternion.identity;
        hud.Display("Inhale");
    }

    private void UpdateTimer(bool forwards)
    {
        int dir = forwards ? 1 : -1;
        timer.fillAmount += dir * Time.deltaTime / inhaleDur;
        marker.transform.RotateAround(markerAnchor.position, -dir * Vector3.forward, Time.deltaTime * 360f / inhaleDur);
        wave.transform.localPosition = new Vector3(0, wave.transform.localPosition.y + dir * (150 / inhaleDur) * Time.deltaTime, 0);
    }

    private void Start()
    {
        dotTrail = new List<GameObject>();

        for (int i = 0; i < numDots; i++)
        {
            dotTrail.Add(null);
        }

        micName = Microphone.devices[0];
        micClip = Microphone.Start(micName, true, 1, 44100);

        micAudioSource = gameObject.AddComponent<AudioSource>();
        micAudioSource.clip = micClip;
        micAudioSource.loop = true;
        micAudioSource.outputAudioMixerGroup = micSilentGroup;

        while (!(Microphone.GetPosition(micName) > 0)) { }
        micAudioSource.Play();

        pitchEstimator = GetComponent<AudioPitchEstimator>();
    }

    private void OnEnable()
    {
        Reset();
    }

    private void Update()
    {
        if (timer.fillAmount >= 1f)
        {
            if (inhaling)
            {
                inhaling = false;
                timer.fillAmount = 0;
                icon.sprite = exhaleSprite;
                wave.transform.localPosition = new Vector3(0, -150, 0);
                hud.Display("Exhale and hum");
            }
            else
            {
                onBreathPass?.Invoke();
            }
        }

        if (inhaling)
        {
            UpdateTimer(true);
        }
        else
        {
            float pitch = pitchEstimator.Estimate(micAudioSource);

            if (!float.IsNaN(pitch))
            {
                UpdateTimer(true);
            }
        }

        for (int i = 0; i < numDots; i++)
        {
            float fillIdx = (float)i / (float)numDots;
            float radius = 58f;
            float angleDeg = i * (360 / numDots);
            float angleOffset = -90f;
            float angleRad = Mathf.Deg2Rad * (angleDeg + angleOffset);
            float offset = 14f;

            if (timer.fillAmount > fillIdx && dotTrail[i] == null)
            {
                dotTrail[i] = Instantiate(dotPrefab, transform);
                dotTrail[i].transform.localPosition = new Vector3(-radius * Mathf.Cos(angleRad), radius * Mathf.Sin(angleRad) - offset);
            }

            if (timer.fillAmount < fillIdx && dotTrail[i] != null)
            {
                Destroy(dotTrail[i]);
                dotTrail[i] = null;
            }

            if (dotTrail[i] != null)
            {
                Color dotColor = Color.white;
                dotColor.a = (float)i / (float)numDots;
                dotTrail[i].GetComponent<Image>().color = dotColor;
            }
        }
        
    }

    public void SetCombatDuration()
    {
        inhaleDur = 3f;
        exhaleDur = 2f;
    }

    public void ResetDuration()
    {
        inhaleDur = 5f;
        exhaleDur = 4f;
    }
}
