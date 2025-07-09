using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public enum CombatState { START, PLAYER_TURN, ENEMY_TURN, END}
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
    public GameObject attackCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackCanvas.SetActive(false); //hide Attack UI

        state = CombatState.START;

        enemyHealth = enemy.GetComponent<Health>();
        playerHealth = player.GetComponent<Health>();

        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        player_health_bar.gameObject.SetActive(true);
        enemy_health_bar.gameObject.SetActive(true);

        battle_text.text = "Face off against this wretched enemy.";

        yield return new WaitForSeconds(5f);

        state = CombatState.PLAYER_TURN;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PerformPlayerAction(IEnumerator action)
    {
        attackCanvas.SetActive(false); //gets rid of the canvas while it happens

        yield return StartCoroutine(action);

        //End player turn
        state = CombatState.ENEMY_TURN;
        StartCoroutine(EnemyTurn());
    }
    IEnumerator OneNoteAttack()
    {
        enemyHealth.TakeDamage(10f);
        battle_text.text = "Your humming did some light damage to the enemy.";
        yield return new WaitForSeconds(2f);

    }

    IEnumerator TwoNoteAttack()
    {
        enemyHealth.TakeDamage(30f);
        battle_text.text = "Your humming pattern did some medium damage to the enemy.";
        yield return new WaitForSeconds(2f);
    }

    IEnumerator ThreeNoteAttack()
    {
        enemyHealth.TakeDamage(60f);
        battle_text.text = "Your melodic chorus did some heavy damage to the enemy!";
        yield return new WaitForSeconds(2f);
    }

    IEnumerator Healing()
    {
        playerHealth.Heal(35);
        battle_text.text = "Your blissful melody healed you considerably.";
        yield return new WaitForSeconds(2f);

    }

    IEnumerator EnemyTurn()
    {
        battle_text.text = "Enemy is preparing to attack...";
        yield return new WaitForSeconds(2f);

        playerHealth.TakeDamage(20); // Example enemy attack
        battle_text.text = "Enemy hit you!";

        yield return new WaitForSeconds(2f);

        // Check for player defeat here

        state = CombatState.PLAYER_TURN;
        PlayerTurn();
    }
    IEnumerator PlayerTurn()
    {
        battle_text.text = "Attack the enemy with one note.";
        yield return new WaitForSeconds(2f);

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
