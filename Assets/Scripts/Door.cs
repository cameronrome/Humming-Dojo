using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, Interactable
{
    [SerializeField] GameObject door_to_open;
    public void Interact()
    {
        StartCoroutine(OpenDoorSmoothly());
    }
    IEnumerator OpenDoorSmoothly()
    {
        Vector3 startPos = door_to_open.transform.position;
        Vector3 endPos = startPos + new Vector3(0f, 4f, 0f); // Move up by 4 units
        float duration = 1.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            door_to_open.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        door_to_open.transform.position = endPos; // Ensure exact final position
        Debug.Log("The door has been opened!");
    }
}
