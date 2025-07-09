using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public enum CombatState { START, PLAYER_TURN, ENEMY_TURN, WIN, LOSE}
public class CombatSystem : MonoBehaviour
{
    public CombatState state;

    public GameObject enemy;
    public GameObject player;

    public Image player_health_bar;
    public Image enemy_health_bar;
    public TextMeshProUGUI battle_text;

    private Health enemyHealth;
    private Health playerHealth;

    public GameObject combatCanvas;
    public GameObject healthbarCanvas;
    public GameObject attackCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void BeginCombat()
        {
            healthbarCanvas.SetActive(true);

            state = CombatState.START;

            enemyHealth = enemy.GetComponent<Health>();
            playerHealth = player.GetComponent<Health>();

            StartCoroutine(SetupBattle());
        }

    IEnumerator SetupBattle()
    {
        battle_text.text = "A wretched enemy appears from the shadows.";

        yield return new WaitForSeconds(3f);

        state = CombatState.PLAYER_TURN;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PerformPlayerAction(IEnumerator action)
    {
        combatCanvas.SetActive(false);
        attackCanvas.SetActive(false); //gets rid of the canvas while it happens

        yield return StartCoroutine(action);
        yield return new WaitForSeconds(3f);

        if(state == CombatState.WIN)
        {
            yield return StartCoroutine(PlayerWin());
        }
        else if(state == CombatState.LOSE){
            yield return StartCoroutine(PlayerLose());
        }
        else
        {
            state = CombatState.ENEMY_TURN;
            StartCoroutine(EnemyTurn());
        }

    }
    IEnumerator OneNoteAttack()
    {
        bool dead = enemyHealth.TakeDamage(10f);
        yield return new WaitForSeconds(1f);
        if (dead)
        {
            state = CombatState.WIN;
        }
        else
        {
            battle_text.text = "Your humming did light damage to the enemy.";
        }
    }

    IEnumerator TwoNoteAttack()
    {
        bool dead = enemyHealth.TakeDamage(30f);

        yield return new WaitForSeconds(1f);
        if (dead)
        {
            state = CombatState.WIN;
        }
        else
        {
            battle_text.text = "Your humming pattern did medium damage to the enemy.";
        }
    }

    IEnumerator ThreeNoteAttack()
    {
        bool dead = enemyHealth.TakeDamage(60f);

        yield return new WaitForSeconds(1f);
        if (dead)
        {
            state = CombatState.WIN;
        }
        else
        {
            battle_text.text = "Your melodic chorus did heavy damage to the enemy!";
        }
    }

    IEnumerator Healing()
    {
        playerHealth.Heal(35);
        yield return new WaitForSeconds(1f);

        battle_text.text = "Your blissful melody healed you considerably.";


    }

    IEnumerator EnemyTurn()
    {
        battle_text.text = "Enemy is preparing to attack...";
        yield return new WaitForSeconds(3f);

        bool dead = playerHealth.TakeDamage(Random.Range(10f, 30f)); // random damage for now
        battle_text.text = "Enemy hit you!";

        yield return new WaitForSeconds(2f);

        if (dead)
        {
            state = CombatState.LOSE;
            yield return StartCoroutine(PlayerLose());
            yield break;
        }

        state = CombatState.PLAYER_TURN;
        StartCoroutine(PlayerTurn());
    }
    IEnumerator PlayerTurn()
    {
        combatCanvas.SetActive(true);
        battle_text.text = "Attack, Defend or Heal.";
        yield return new WaitForSeconds(2f);

    }

    IEnumerator PlayerWin()
    {
        battle_text.text = "You defeated that evil enemy!";

        yield return new WaitForSeconds(3f);

        combatCanvas.SetActive(false);
        attackCanvas.SetActive(false);
        healthbarCanvas.SetActive(false);

        enemy.SetActive(false);
    }

    IEnumerator PlayerLose()
    {
        battle_text.text = "You suffer a defeat to the evil enemy.";

        yield return new WaitForSeconds(3f);

        combatCanvas.SetActive(false);
        attackCanvas.SetActive(false);
        healthbarCanvas.SetActive(false);
    }

    public void OnAttackButton()
    {
        if (state != CombatState.PLAYER_TURN)
        {
            return;
        }

        //swtiches UIs from the attack, defend, heal screen to the different attacks
        combatCanvas.SetActive(false);
        attackCanvas.SetActive(true);
    }

    public void OnOneNoteAttackButton()
    {
        StartCoroutine(PerformPlayerAction(OneNoteAttack()));
    }

    public void OnTwoNoteAttackButton()
    {
        StartCoroutine(PerformPlayerAction(TwoNoteAttack()));
    }

    public void OnThreeNoteAttackButton()
    {
        StartCoroutine(PerformPlayerAction(ThreeNoteAttack()));
    }

    public void OnHealButton()
    {
        StartCoroutine(PerformPlayerAction(Healing()));
    }
}
