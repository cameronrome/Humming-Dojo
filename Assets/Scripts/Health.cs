using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float current_health;
    [SerializeField] private float max_health;
    [SerializeField] private Image health_bar;
    void Start()
    {
        current_health = max_health;
    }

    // Update is called once per frame
    void Update()
    {
        health_bar.fillAmount = Mathf.Clamp(current_health / max_health, 0, 1);
    }

    public void TakeDamage(float amount)
    {
        current_health -= amount;
        current_health = Mathf.Max(current_health, 0);

        Debug.Log(current_health);

        if (current_health <= 0)
        {
            // actions when die
        }
    }

    public void Heal(float amount)
    {
        current_health += amount;
        current_health = Mathf.Min(current_health, max_health);
    }
}
