using System.Runtime.CompilerServices;
using UnityEngine;

public class GemCollection : MonoBehaviour
{
    private int Gems = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            Gems++;
            Debug.Log(Gems);
            Destroy(other.gameObject);
        }
    }
}
