using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    [SerializeField] EventReference footstepEvent;
    [SerializeField] float rate = 0.45f;
    [SerializeField] GameObject player;
    [SerializeField] PlayerController controller;

    float time;

    public void PlayFootstep()
    {
        RuntimeManager.PlayOneShotAttached(footstepEvent, player);
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
