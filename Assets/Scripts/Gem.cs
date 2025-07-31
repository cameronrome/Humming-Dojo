using UnityEngine;

public class Gem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnBecameVisible()
    {
        GemManager.Instance.Register(this);
    }

    private void OnBecameInvisible()
    {
        GemManager.Instance.Unregister(this);
    }

    private void OnDestroy()
    {
        if (GemManager.Instance != null)
        {
            GemManager.Instance.Unregister(this);
        }
    }

}
