using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    public void Interact()
    {
        Debug.Log("The door has been opened!");
    }
}
