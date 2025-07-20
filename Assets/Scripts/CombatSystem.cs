using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.TextCore.Text;
using UnityEngine.SceneManagement;

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

    private BreathMeter playerBreath;

    public GameObject combatCanvas;
    public GameObject healthbarCanvas;
    public GameObject attackCanvas;

    public CameraFollow cameraFollow;
    public PlayerController playerController;

    public void BeginCombat()
        {
            healthbarCanvas.SetActive(true);

            cameraFollow.StartCombatZoom();
            playerController.DisableMovement();

            state = CombatState.START;

            enemyHealth = enemy.GetComponent<Health>();
            playerHealth = player.GetComponent<Health>();

            playerBreath = player.GetComponent<BreathMeter>();

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
        bool usedBreath = UseBreath(50f); //use breath

        if (usedBreath)
        {
            bool enemyAlive = AttackAndCheckEnemyAlive(10f); //attack
            if (enemyAlive)
            {
                battle_text.text = "Your humming did light damage to the enemy.";
                yield return new WaitForSeconds(2f);
            }
        }
    }

    IEnumerator TwoNoteAttack()
    {
        bool usedBreath = UseBreath(30f); //use breath 

        if (usedBreath)
        {
            bool enemyAlive = AttackAndCheckEnemyAlive(30f); //attack
            if (enemyAlive)
            {
                battle_text.text = "Your humming pattern did medium damage to the enemy.";
                yield return new WaitForSeconds(2f);
            }
        }
    }

    IEnumerator ThreeNoteAttack()
    {
        bool usedBreath = UseBreath(60f); //use breath

        if (usedBreath)
        {
            bool enemyAlive = AttackAndCheckEnemyAlive(60f); //attack
            if (enemyAlive)
            {
                battle_text.text = "Your melodic chorus did heavy damage to the enemy!";
                yield return new WaitForSeconds(2f);
            }
        }
    }

    IEnumerator RefillBreath()
    {
        float current_breath = playerBreath.GetCurrentBreath();
        float max_breath = playerBreath.GetMaxBreath();

        if (current_breath == max_breath)
        {
            battle_text.text = "You are unable to gain any more breath from your meditation.";
        }
        else
        {
            playerBreath.RestoreBreath(35);
            battle_text.text = "Your thoughtful meditation has caused your breath to restore.";
        }
        yield return new WaitForSeconds(2f);
    }

    IEnumerator Healing()
    {
        float current_health = playerHealth.GetCurrentHealth();
        float max_health = playerHealth.GetMaxHealth();

        if (current_health == max_health)
        {
            battle_text.text = "You are unable to gain any more health from your humming melody.";
        }
        else
        {
            playerHealth.Heal(35);
            battle_text.text = "Your blissful melody healed you considerably.";
        }  

        yield return new WaitForSeconds(2f);
    }


    private bool UseBreath(float breath_needed) //returns true if it used breath
    {
        bool enough_breath = playerBreath.EnoughBreath(breath_needed);

        if (!enough_breath)
        {
            battle_text.text = "You do not have enough breath for this attack.";
            return false;
        }
        else
        {
            playerBreath.UseBreath(breath_needed);
            return true;
        }
    }

    private bool AttackAndCheckEnemyAlive(float damage)
    {
        bool dead = enemyHealth.TakeDamage(damage); // attack

        if (dead)
        {
            state = CombatState.WIN;
            return false;
        }
        else
        {
            return true;
        }
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

        playerController.EnableMovement();
        cameraFollow.EndCombatZoom();
    }

    IEnumerator PlayerLose()
    {
        battle_text.text = "You suffer a defeat to the evil enemy.";

        yield return new WaitForSeconds(3f);

        combatCanvas.SetActive(false);
        attackCanvas.SetActive(false);
        healthbarCanvas.SetActive(false);

        // playerController.EnableMovement(); //optional code to give player movement back, if scene isn't reloaded

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload current scene by index, like player "dies"

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

    public void OnDefendButton()
    {
        StartCoroutine(PerformPlayerAction(RefillBreath()));
    }
    public void OnHealButton()
    {
        StartCoroutine(PerformPlayerAction(Healing()));
    }
}
