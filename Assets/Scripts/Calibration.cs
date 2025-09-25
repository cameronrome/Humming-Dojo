using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Calibration : MonoBehaviour {
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
    private RectTransform micPulseRT;

    private bool record_flag;
    private bool reset_flag;
    private int pulsePeriod = 250;
    private double pulseMin = 22;
    private double pulseMax = 25;
    private double pulseMid;
    private double pulseHR;

    public AudioSource audioSource;
    public AudioPitchEstimator estimator;

    private float estimateRate = 30.0f;
    const int spectrumSize = 1024;
    private int record_ct;

    // public Vector3[] noise_lines = new Vector3[1024];

	void Start () {
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
        
        micIcon.SetActive(false);
        micPulse.SetActive(false);
        //micPulseRT = micPulse.

        record_flag = false;
        reset_flag = false;
        pulseMid = (pulseMax + pulseMin) / 2;
        pulseHR = (pulseMax - pulseMin) / 2;

        // for (int i = 0; i < spectrumSize; i++) {
        //     estimator.noise_spec[i] = 0f;
        // }
	}

	void Calibrate(){
        record_flag = !record_flag;

        // Start Recording
        if (record_flag) { 
            calibrationBgImg.sprite = stopSprite;

            resetBtn.SetActive(false);
            reset_flag = true;

            micIcon.SetActive(true);
            micPulse.SetActive(true);

            // for (int i = 0; i < spectrumSize; i++) {
            //     estimator.noise_spec[i] = 0f;
            // }
        } 
        // Stop Recording
        else { 
            calibrationBgImg.sprite = startSprite;

            resetBtn.SetActive(true);
            rBtn.enabled = reset_flag;
            resetBgImg.sprite = (reset_flag) ? resetActiveSprite : resetInactiveSprite;

            micIcon.SetActive(false);
            micPulse.SetActive(false);
        }

        //Debug.Log("btn click " + record_flag);
	}

    void Reset() {
        reset_flag = !reset_flag;
        rBtn.enabled = reset_flag;
        resetBgImg.sprite = (reset_flag) ? resetActiveSprite : resetInactiveSprite;
    }

    void Update()
    {
        // Recording
        if (record_flag) { 
            record_ct = record_ct + 1;
        
            float resize = (float) (pulseMid + (2 * pulseHR / Math.PI) * Math.Acos(Math.Cos(Math.PI * record_ct / pulsePeriod)) - pulseHR);
            
            micPulse.GetComponent<RectTransform>().sizeDelta = new Vector2(resize, resize);

            // estimate the fundamental frequency
            var frequency = estimator.Estimate(audioSource);
            // var spectrum = estimator.Spec;
            // for (int i = 0; i < spectrumSize; i++)
            // {
            //     float prev_noise = estimator.noise_spec[i];
            //     estimator.noise_spec[i] = (prev_noise * (record_ct - 1) + spectrum[i]) / record_ct;
            // }

            // for (int i = 0; i < spectrumSize; i++)
            // {
            //     noise_lines[i] = new Vector3(0.15f * Mathf.Log((float)i), 
            //             0.02f * (Mathf.Log(estimator.noise_spec[i]) + 19.5f), -0.1f);
            // }
        
        }
        // Not Recording 
        else { 
            record_ct = 0;
            // Debug.Log(reset_flag);            
        }
    }

    // void OnDrawGizmos()
    // {
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