using UnityEngine;
using System.Collections.Generic;

public class GemManager : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 85f;

    private Quaternion rotation;
    private List<Gem> gems;

    #region Singleton
    public static GemManager Instance;

    private void Awake()
    {
        Instance = this;
        rotation = Quaternion.identity;
        gems = new List<Gem>();
    }
    #endregion

    private void Update()
    {
        rotation *= Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f);
        foreach(Gem g in gems)
        {
            g.transform.rotation = rotation;
        }
    }

    public void Register(Gem g)
    {
        if (!gems.Contains(g))
        {
            gems.Add(g);
        }
    }

    public void Unregister(Gem g)
    {
        if (gems.Contains(g))
        {
            gems.Remove(g);
        }
    }
}
