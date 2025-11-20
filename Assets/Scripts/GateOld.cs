using System.Collections.Generic;
using UnityEngine;

public class GateOld : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private HumDial humDial;
    [SerializeField] private List<int> keys;

    [SerializeField] private string direction;

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
        HideHumDial();
    }

    private void Start()
    {
        humDial.OnHumPass += OpenGate;
    }

    public string GetDirection()
    {
        return direction;
    }

    public bool isOpen()
    {
        return opened;
    }
}
