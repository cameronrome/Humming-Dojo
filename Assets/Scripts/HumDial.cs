using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class HumDial : MonoBehaviour
{
    public AudioMixerGroup micSilentGroup;
    public List<HumDialTab> tabs;
    public string currNote = "";

    public Dictionary<string, Color> colors = new Dictionary<string, Color>()
    {
        { "C4", new Color(0.9725f, 0.6706f, 0.6784f) },
        { "D4", new Color(0.9961f, 0.8392f, 0.6471f) },
        { "E4", new Color(0.9804f, 0.9647f, 0.7216f) },
        { "F4", new Color(0.8118f, 0.898f, 0.7216f) },
        { "G4", new Color(0.6667f, 0.8745f, 0.9294f) },
        { "A4", new Color(0.651f, 0.7569f, 0.898f) },
        { "B4", new Color(0.7294f, 0.6902f, 0.8431f) },
        { "C5", new Color(0.9294f, 0.7843f, 0.8745f) },
    };

    private AudioClip micClip;
    private AudioSource micAudioSource;
    private AudioPitchEstimator pitchEstimator;
    private List<int> prevNotes;
    private string micName;
    private string[] notes = { "C4", "D4", "E4", "F4", "G4", "A4", "B4", "C5" };
    private float[] notePitches = { 246.94f, 261.63f, 293.66f, 329.63f, 349.23f, 392f, 440f, 493.88f, 523.25f, 554.37f };

    private int CalcNoteIdx(float pitch)
    {
        for (int i = 1; i < notePitches.Length - 1; i++)
            if (pitch >= (notePitches[i - 1] + notePitches[i]) / 2f && pitch < (notePitches[i] + notePitches[i + 1]) / 2f)
                return i;

        return -1;
    }

    private void SetNote(int noteIdx)
    {
        foreach (HumDialTab tab in tabs)
            tab.ResetGlow();

        if (noteIdx >= 0)
        {
            tabs[noteIdx - 1].Glow();
            GetComponentInChildren<TMP_Text>().text = notes[noteIdx - 1];
            currNote = notes[noteIdx - 1];
        }
        else
        {
            GetComponentInChildren<TMP_Text>().text = "";
            currNote = "";
        }
    }

    private void RecordNote(int noteIdx)
    {
        if (prevNotes.Count >= 100)
            prevNotes.RemoveAt(0);

        prevNotes.Add(noteIdx);
    }

    private int? GetNoteIdx()
    {
        if (prevNotes.Count == 0)
            return null;

        int noteIdx = prevNotes[0];

        foreach (int prevNote in prevNotes)
            if (prevNote != noteIdx)
                return null;

        return noteIdx;
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
        prevNotes = new List<int>();

        gameObject.SetActive(false);
    }

    private void Update()
    {

        float pitch = pitchEstimator.Estimate(micAudioSource);
        int noteIdx = CalcNoteIdx(pitch);

        RecordNote(noteIdx);

        int? nextNoteIdx = GetNoteIdx();

        if (nextNoteIdx != null)
            SetNote((int)nextNoteIdx);
    }
}
