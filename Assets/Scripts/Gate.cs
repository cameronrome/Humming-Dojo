using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private HumDial humDial;
    [SerializeField] private string[] keyNotes;
    [SerializeField] private float holdDur = 0.5f;

    private Renderer[] childRenderers => GetComponentsInChildren<Renderer>();
    private Color[] childRendererColors;
    private float flashTimer;
    private float noteTimer;
    private bool opened;
    private bool inRange;
    private int keyIdx;
    private int flashIdx;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && !opened)
        {
            humDial.gameObject.SetActive(true);
            noteTimer = holdDur;
            flashTimer = holdDur;
            inRange = true;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            humDial.gameObject.SetActive(false);
            inRange = false;
            ResetColors();
        }
    }

    private void OpenGate()
    {
        if (opened) return;

        animator.SetBool("Opened", true);
        opened = true;
        inRange = false;
        humDial.gameObject.SetActive(false);
        ResetColors();
    }

    private void ResetColors()
    {
        for (int i = 0; i < childRenderers.Length; i++)
            childRenderers[i].material.color = childRendererColors[i];
    }

    private void Start()
    {
        childRendererColors = new Color[childRenderers.Length];

        for (int i = 0; i < childRenderers.Length; i++)
            childRendererColors[i] = childRenderers[i].material.color;
    }

    private void Update()
    {
        if (inRange)
        {
            if (keyIdx < keyNotes.Length && humDial.currNote == keyNotes[keyIdx])
                noteTimer -= Time.deltaTime;
            else
                noteTimer = holdDur;

            if (noteTimer <= 0)
            {
                keyIdx++;

                if (keyIdx >= keyNotes.Length)
                    OpenGate();
            }

            flashTimer -= Time.deltaTime;

            if (flashTimer <= 0)
            {
                if (flashIdx >= keyNotes.Length)
                {
                    ResetColors();
                    flashIdx = 0;
                } 
                else
                {
                    for (int i = 0; i < childRenderers.Length; i++)
                        childRenderers[i].material.color = humDial.colors[keyNotes[flashIdx]];

                    flashIdx++;
                }

                flashTimer = holdDur;
            }
        }
    }
}
