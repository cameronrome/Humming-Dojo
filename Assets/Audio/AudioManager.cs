using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    [SerializeField] EventReference footstepEvent;
    [SerializeField] EventReference ForestAmbience;
    [SerializeField] float rate = 0.45f;
    [SerializeField] GameObject player;
    [SerializeField] PlayerController controller;

    float time;

    private void Start()
    {
        PlayForestAmbience();
    }


    public void PlayFootstep()
    {
        RuntimeManager.PlayOneShotAttached(footstepEvent, player);
    }

    public void PlayForestAmbience()
    {
        RuntimeManager.PlayOneShot(ForestAmbience);
    }

    void Update()
    {
        time += Time.deltaTime;

        if (controller != null && controller.IsWalking)
        {
            if (time >= rate)
            {
                PlayFootstep();
                time = 0f;
            }
        }
        else
        {
          
            time = rate;
        }
    }
}
