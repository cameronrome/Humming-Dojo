using UnityEngine;
using UnityEngine.UI;

public class BreathMeter : MonoBehaviour
{
    [SerializeField] private float current_breath;
    [SerializeField] private float max_breath = 100;
    [SerializeField] private Image breath_bar;

    //Right now, logic is the same as health. Future breathing mechanics might differ, hence the different script.

    void Start()
    {
        current_breath = max_breath;
    }

    // Update is called once per frame
    void Update()
    {
        breath_bar.fillAmount = Mathf.Clamp(current_breath / max_breath, 0, 1);
    }

    public bool EnoughBreath(float amount) //check if enough breath to do move
    {
        if(current_breath - amount < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void UseBreath(float amount)
    {
        bool enough_breath = EnoughBreath(amount);

        if (enough_breath)
        {
            current_breath -= amount;
        }
        
    }

    public void RestoreBreath(float amount)
    {
        current_breath += amount;
        current_breath = Mathf.Min(current_breath, max_breath);
    }

    public float GetCurrentBreath()
    {
        return current_breath;
    }

    public float GetMaxBreath()
    {
        return max_breath;
    }
}
