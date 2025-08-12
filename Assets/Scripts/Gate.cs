using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private HumDial humDial;
    [SerializeField] private List<int> keys;

    public bool inRange;

    private bool opened;

    public void ShowHumDial()
    {
        if (opened) return;

        humDial.SetKeys(keys);
        humDial.Open();
    }

    public void HideHumDial()
    {
        humDial.Close();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && !opened)
        {
            inRange = true;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            inRange = false;
        }
    }

    private void OpenGate()
    {
        if (opened || !inRange) return;

        animator.SetBool("Opened", true);
        opened = true;
    }

    private void Start()
    {
        humDial.OnHumPass += OpenGate;
    }
}
