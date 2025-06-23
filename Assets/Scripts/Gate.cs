using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private HumDial humDial;
    [SerializeField] private string correctNote;
    [SerializeField] private float holdDur = 1;
    [SerializeField] private float flashDur = 0.4f;

    private Renderer[] childRenderers => GetComponentsInChildren<Renderer>();
    private Color[] childRendererColors;
    private bool opened;
    private bool flashing;
    private float flashTimer;
    private float noteTimer;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && !opened)
        {
            humDial.gameObject.SetActive(true);
            noteTimer = holdDur;
            flashTimer = flashDur;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            humDial.gameObject.SetActive(false);

            for (int i = 0; i < childRenderers.Length; i++)
                childRenderers[i].material.color = childRendererColors[i];
        }
    }

    private void OpenGate()
    {
        if (opened) return;

        animator.SetBool("Opened", true);
        opened = true;

        humDial.gameObject.SetActive(false);

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
        if (humDial.gameObject.activeSelf)
        {
            if (humDial.currNote == correctNote)
                noteTimer -= Time.deltaTime;
            else
                noteTimer = holdDur;

            if (noteTimer <= 0)
                OpenGate();

            flashTimer -= Time.deltaTime;

            if (flashTimer <= 0)
            {
                if (flashing)
                {
                    for (int i = 0; i < childRenderers.Length; i++)
                        childRenderers[i].material.color = childRendererColors[i];

                    flashing = false;
                } 
                else
                {
                    for (int i = 0; i < childRenderers.Length; i++)
                        childRenderers[i].material.color = humDial.colors[correctNote];

                    flashing = true;
                }

                flashTimer = flashDur;
            }
        }
    }
}
