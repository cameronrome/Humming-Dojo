using UnityEngine;

public class StartCombat : MonoBehaviour
{
    public CombatSystem combatSystem;

    private bool combatStarted = false;

    void OnTriggerEnter(Collider other)
    {
        if (!combatStarted && other.CompareTag("Player"))
        {
            combatStarted = true;
            combatSystem.BeginCombat();

            // Optionally disable the trigger if it's single-use
            //gameObject.SetActive(false);
        }
    }
}
