using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

public enum CombatState { START, PLAYER_TURN, ENEMY_TURN, WIN, LOSE}
public class CombatSystem : MonoBehaviour
{
    public CombatState state;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject player;

    [SerializeField] private Image player_health_bar;
    [SerializeField] private Image enemy_health_bar;
    [SerializeField] private TextMeshProUGUI battle_text;

    [SerializeField] private Health enemyHealth;
    [SerializeField] private Health playerHealth;

    [SerializeField] private BreathMeter playerBreath;

    [SerializeField] private GameObject combatCanvas;
    [SerializeField] private GameObject healthbarCanvas;
    [SerializeField] private GameObject attackCanvas;
    [SerializeField] private GameObject gemUI;

    [SerializeField] private HumDial humDial;

    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private PlayerController playerController;

    //HEALTH AND BREATH CONSTANTS
    private float oneNoteAttackDMG = 10f;
    private float twoNoteAttackDMG = 30f;
    private float threeNoteAttackDMG = 60f;

    private float oneNoteAttackBRTH = 15f;
    private float twoNoteAttackBRTH = 40f;
    private float threeNoteAttackBRTH = 75f;

    private bool humPassed = false;

    public void BeginCombat()
        {
            gemUI.SetActive(false);
            healthbarCanvas.SetActive(true);

            cameraFollow.StartCombatZoom();
            playerController.DisableMovement();

            state = CombatState.START;

            enemyHealth = enemy.GetComponent<Health>();
            playerHealth = player.GetComponent<Health>();

            playerBreath = player.GetComponent<BreathMeter>();

            humDial.setKeyDuration(.75f);

            StartCoroutine(SetupBattle());
        }

    IEnumerator SetupBattle()
    {
        battle_text.text = "A wretched enemy appears from the shadows.";

        yield return new WaitForSeconds(3f); //enemy spawn animation?

        state = CombatState.PLAYER_TURN;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PerformPlayerAction(IEnumerator action)
    {
        combatCanvas.SetActive(false);
        attackCanvas.SetActive(false); //gets rid of the canvas while it happens

        yield return StartCoroutine(action);
        //yield return new WaitForSeconds(3f);

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
        //check breath
        bool enoughBreath = CheckBreath(oneNoteAttackBRTH); //check if enough breath

        if (enoughBreath)
        {
            humPassed = false; 

            humDial.SetKeys(new List<int>() { 2 } );
            humDial.Open();

            battle_text.text = "Hum the note to invoke a small attack.";

            UnityAction handler = () => AttackHelper(oneNoteAttackDMG, oneNoteAttackBRTH);
            humDial.OnHumPass += handler; //use move on enemy

            // wait until hum passes
            yield return new WaitUntil(() => humPassed);

            battle_text.text = "Your humming did light damage to the enemy.";
            humDial.OnHumPass -= handler;
        }
        yield return new WaitForSeconds(3f);
    }

    

    IEnumerator TwoNoteAttack()
    {
        //check breath
        bool enoughBreath = CheckBreath(twoNoteAttackBRTH); //check if enough breath

        if (enoughBreath)
        {
            humPassed = false;

            humDial.SetKeys(new List<int>() { 2, 3 });
            humDial.Open();

            battle_text.text = "Hum the notes to invoke a strong attack.";

            UnityAction handler = () => AttackHelper(twoNoteAttackDMG, twoNoteAttackBRTH);
            humDial.OnHumPass += handler; //use move on enemy

            // wait until hum passes
            yield return new WaitUntil(() => humPassed);

            battle_text.text = "Your humming pattern did medium damage to the enemy.";
            humDial.OnHumPass -= handler;

            yield return new WaitForSeconds(3f);
            
        }
    }

    IEnumerator ThreeNoteAttack()
    {
        //check breath
        bool enoughBreath = CheckBreath(threeNoteAttackBRTH); //check if enough breath

        if (enoughBreath)
        {
            humPassed = false;

            humDial.SetKeys(new List<int>() { 3, 2, 4 });
            humDial.Open();

            battle_text.text = "Hum the notes to invoke a very powerful attack.";

            UnityAction handler = () => AttackHelper(threeNoteAttackDMG, threeNoteAttackBRTH);
            humDial.OnHumPass += handler; //use move on enemy

            // wait until hum passes
            yield return new WaitUntil(() => humPassed);

            battle_text.text = "Your melodic chorus did heavy damage to the enemy!";
            humDial.OnHumPass -= handler;

            yield return new WaitForSeconds(3f);

        }
    }

    private void AttackHelper(float damage, float breath)
    {
        AttackAndCheckEnemyAlive(damage); // attack
        playerBreath.UseBreath(breath); // use breath

        humPassed = true;
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
        yield return new WaitForSeconds(3f);
    }

    IEnumerator Healing()
    {
        float current_health = playerHealth.GetCurrentHealth();
        float max_health = playerHealth.GetMaxHealth();

        if (current_health == max_health)
        {
            battle_text.text = "You are unable to gain any more health.";
        }
        else
        {
            humPassed = false;

            humDial.SetKeys(new List<int>() { 6, 5 });
            humDial.Open();

            battle_text.text = "Hum the notes to heal yourself.";

            humDial.OnHumPass += HealHelper;
            yield return new WaitUntil(() => humPassed);

            battle_text.text = "Your blissful melody healed you considerably.";
            humDial.OnHumPass -= HealHelper;
        }  

        yield return new WaitForSeconds(3f);
    }

    private void HealHelper()
    {
        playerHealth.Heal(35);
        humPassed = true;
        humDial.Close();
    }

    private bool CheckBreath(float breath_needed) //returns true if it used breath
    {
        bool enough_breath = playerBreath.EnoughBreath(breath_needed);

        if (!enough_breath)
        {
            battle_text.text = "You do not have enough breath for this attack.";
            return false;
        }
        else
        {
            return true;
        }
    }

    private void AttackAndCheckEnemyAlive(float damage)
    {
        bool dead = enemyHealth.TakeDamage(damage); // attack

        if (dead)
        {
            state = CombatState.WIN;
        }

        humDial.Close();
    }

    IEnumerator EnemyTurn()
    {
        battle_text.text = "Enemy is preparing to attack...";
        yield return new WaitForSeconds(2f);

        bool dead = playerHealth.TakeDamage(Random.Range(10f, 30f)); // random damage for now
        battle_text.text = "Enemy hit you!";

        yield return new WaitForSeconds(3f);

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
        battle_text.text = "Attack, Breathe or Heal.";
        yield return new WaitForSeconds(2f);

    }

    IEnumerator PlayerWin()
    {
        battle_text.text = "You defeated that evil enemy!";

        yield return new WaitForSeconds(3f); //timer during enemy death animation

        combatCanvas.SetActive(false);
        attackCanvas.SetActive(false);
        healthbarCanvas.SetActive(false);

        enemy.SetActive(false);

        playerController.EnableMovement();
        cameraFollow.EndCombatZoom();
        gemUI.SetActive(true);
    }

    IEnumerator PlayerLose()
    {
        battle_text.text = "You suffer a defeat to the evil enemy.";

        yield return new WaitForSeconds(3f); //timer for player death animation

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

        battle_text.text = "Choose an attack.";
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
