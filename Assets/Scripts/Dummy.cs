using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private AudioClip micClip;
    private AudioSource micAudioSource;
    private MeshRenderer meshRenderer;
    private AudioPitchEstimator pitchEstimator;
    private string micName;
    private List<float> pitches;

    private void Start()
    {
        micName = Microphone.devices[0];
        micClip = Microphone.Start(micName, true, 10, 44100);

        micAudioSource = gameObject.AddComponent<AudioSource>();
        micAudioSource.clip = micClip;
        micAudioSource.loop = true;

        while (!(Microphone.GetPosition(micName) > 0)) { }
        micAudioSource.Play();

        meshRenderer = GetComponent<MeshRenderer>();
        pitchEstimator = GetComponent<AudioPitchEstimator>();
        pitches = new List<float>();
    }

    private void Update()
    {
        float nextPitch = pitchEstimator.Estimate(micAudioSource);

        if (float.IsNaN(nextPitch)) return;

        if (pitches.Count >= 50)
        {
            pitches.RemoveAt(0);
        }

        pitches.Add(nextPitch);

        float avgPitch = 0;

        for (int i = 0; i < pitches.Count; i++)
        {
            avgPitch += pitches[i];
        }

        avgPitch /= pitches.Count;

        if (avgPitch > 150)
        {
            meshRenderer.material.color = Color.green;
        } 
        else if (avgPitch > 120)
        {
            meshRenderer.material.color = Color.red;
        }
        else if (avgPitch > 90)
        {
            meshRenderer.material.color = Color.blue;
        }

        Debug.Log(avgPitch);
    }
}
