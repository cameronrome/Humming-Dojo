using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using System.Collections;
using TMPro;

public class Calibration : MonoBehaviour {
    public HumDial dial;
    private AudioSource audioSource;
    private string micName;
    private AudioClip micClip;
    public AudioPitchEstimator estimator;
    public AudioMixerGroup micSilentGroup;

	public GameObject calibrationBtn;
    private Image calibrationBgImg; 
    public Sprite startSprite; 
    public Sprite stopSprite; 

    public GameObject resetBtn;
    private Button rBtn;
    private Image resetBgImg; 
    public Sprite resetInactiveSprite;
    public Sprite resetActiveSprite;
    
    public GameObject micIcon;
    public GameObject micPulse;
    public GameObject descriptionText;
    private Image descBgImg; 
    public Sprite infoSprite; 
    public Sprite recordSprite; 

    private bool record_flag;
    private bool reset_flag;
    private int pulsePeriod = 180;
    private double pulseMin = 80;
    private double pulseMax = 90;
    private double pulseMid;
    private double pulseHR;

    public GameObject countdownText;
    private DateTime previousFrameTime;
    private DateTime currentFrameTime;
    private TimeSpan timeElapsed = TimeSpan.Zero;
    private int calibrationPeriod = 5;

    const int spectrumSize = 1024;
    private int record_ct;

    private float[] cur_noise_spec = new float[spectrumSize];
    // public Vector3[] noise_lines = new Vector3[1024];

    private bool wasBgMusicPlaying = false;

	void Start () {
        if (dial == null || dial.micAudioSource == null) {
            micName = Microphone.devices[0];
            Debug.Log(micName);
            micClip = Microphone.Start(micName, true, 1, 44100);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = micClip;
            audioSource.loop = true;
            audioSource.outputAudioMixerGroup = micSilentGroup;

            while (!(Microphone.GetPosition(micName) > 0)) { }
            audioSource.Play();
        } else {
            audioSource = dial.micAudioSource;
        }
        
		Button cBtn = calibrationBtn.GetComponent<Button>();
		cBtn.onClick.AddListener(Calibrate);

        calibrationBgImg = calibrationBtn.GetComponent<Image>();
        if (calibrationBgImg != null && startSprite != null && stopSprite != null) {
            calibrationBgImg.sprite = startSprite;        
        }

        rBtn = resetBtn.GetComponent<Button>();
		rBtn.onClick.AddListener(Reset);
        rBtn.enabled = false;

        resetBgImg = resetBtn.GetComponent<Image>();
        if (resetBgImg != null && resetInactiveSprite != null && resetActiveSprite != null) {
            resetBgImg.sprite = resetInactiveSprite;        
        }
        
        //micIcon.SetActive(false);
        //micPulse.SetActive(false);
        descBgImg = descriptionText.GetComponent<Image>();
        if (descBgImg != null && infoSprite != null && recordSprite != null) {
            descBgImg.sprite = infoSprite;        
        }

        countdownText.SetActive(false);

        record_flag = false;
        reset_flag = false;
        pulseMid = (pulseMax + pulseMin) / 2;
        pulseHR = (pulseMax - pulseMin) / 2;

        for (int i = 0; i < spectrumSize; i++) {
            estimator.noise_spec[i] = 0f;
            cur_noise_spec[i] = 0f;
        }
	}

	void Calibrate(){
        record_flag = !record_flag;

        // Start Recording
        if (record_flag) {
            wasBgMusicPlaying = BackgroundMusic.Instance.GetComponent<AudioSource>().isPlaying;

            if (wasBgMusicPlaying)
                BackgroundMusic.Instance.Pause();

            calibrationBgImg.sprite = stopSprite;

            float width = calibrationBtn.GetComponent<RectTransform>().localPosition.x;
            Debug.Log(width);
            calibrationBtn.GetComponent<RectTransform>().transform.position += new Vector3(45f, 0f, 0f);
            calibrationBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(280f, 25f);

            resetBtn.SetActive(false);
            reset_flag = true;

            timeElapsed = TimeSpan.Zero;
            previousFrameTime = DateTime.Now;

            //micIcon.SetActive(true);
            //micPulse.SetActive(true);
            descBgImg = descriptionText.GetComponent<Image>();
            if (descBgImg != null && infoSprite != null && recordSprite != null) {
                descBgImg.sprite = recordSprite;        
            }
            countdownText.SetActive(true);

            for (int i = 0; i < spectrumSize; i++) {
                //estimator.noise_spec[i] = 0f;
                cur_noise_spec[i] = 0f;
            }
        } 
        // Stop Recording
        else {
            if (wasBgMusicPlaying)
                BackgroundMusic.Instance.Play();

            calibrationBtn.GetComponent<RectTransform>().position += new Vector3(-45f, 0f, 0f);
            calibrationBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(220f, 25f);
            calibrationBgImg.sprite = startSprite;

            resetBtn.SetActive(true);
            rBtn.enabled = reset_flag;
            resetBgImg.sprite = (reset_flag) ? resetActiveSprite : resetInactiveSprite;

            //micIcon.SetActive(false);
            //micPulse.SetActive(false);
            descBgImg = descriptionText.GetComponent<Image>();
            if (descBgImg != null && infoSprite != null && recordSprite != null) {
                descBgImg.sprite = infoSprite;        
            }
            countdownText.SetActive(false);
            micPulse.GetComponent<RectTransform>().sizeDelta = new Vector2((float) pulseMin, (float) pulseMin);   

            for (int i = 0; i < spectrumSize; i++) {
                estimator.noise_spec[i] = cur_noise_spec[i];
            }
        }
	}

    void Reset() {
        reset_flag = !reset_flag;
        rBtn.enabled = reset_flag;
        resetBgImg.sprite = (reset_flag) ? resetActiveSprite : resetInactiveSprite;

        for (int i = 0; i < spectrumSize; i++) {
            estimator.noise_spec[i] = 0f;
            cur_noise_spec[i] = 0f;
        }
    }

    void Update()
    {
        // Recording
        if (record_flag) { 
            record_ct = record_ct + 1;
        
            float resize = (float) (pulseMid + (2 * pulseHR / Math.PI) * Math.Acos(Math.Cos(Math.PI * record_ct / pulsePeriod)) - pulseHR);
            micPulse.GetComponent<RectTransform>().sizeDelta = new Vector2(resize, resize);

            if (timeElapsed.Seconds < calibrationPeriod) {
                currentFrameTime = DateTime.Now;
                TimeSpan elapsed = currentFrameTime - previousFrameTime;
                timeElapsed = timeElapsed + elapsed;
                previousFrameTime = currentFrameTime;
            } else {
                Calibrate();
            }
            // estimate the fundamental frequency
            var frequency = estimator.Estimate(audioSource);
            var spectrum = estimator.Spec;
            for (int i = 0; i < spectrumSize; i++)
            {
                float prev_noise = cur_noise_spec[i];
                cur_noise_spec[i] = (prev_noise * (record_ct - 1) + spectrum[i]) / record_ct;
            }

            countdownText.GetComponent<TMP_Text>().text = (calibrationPeriod - timeElapsed.Seconds).ToString();
        }
        // Not Recording 
        else { 
            record_ct = 0;   
        }
    }

    // // For checking noise spectrum
    // void OnDrawGizmos()
    // {
    //     for (int i = 0; i < spectrumSize; i++)
    //     {
    //         noise_lines[i] = new Vector3(0.15f * Mathf.Log((float)i), 
    //                 0.02f * (Mathf.Log(estimator.noise_spec[i]) + 19.5f), -0.1f);
    //     }

    //     if (noise_lines.Length > 0)
    //     {
    //         for (int i = 1; i < noise_lines.Length; i++)
    //         {
    //             Gizmos.color = Color.white;
    //             Gizmos.DrawLine(noise_lines[i - 1], noise_lines[i]);
    //         }
    //     }      
    // }
}