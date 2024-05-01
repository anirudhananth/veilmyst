using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEditor;

[RequireComponent(typeof(Destructible))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAudio))]
public class Player : Actor
{
    public Animator animator;
    public Collision col;
    public Destructible destructible;
    public static bool isDead;

    private PlayerInput input;
    private Rigidbody2D rb;
    private PlayerAudio playerAudio;

    public static void PlayerDeathHandler(Destructible self, GameObject killer)
    {

        Player player = self.GetComponent<Player>();
        if(isDead)
        {
            return;
        }
        isDead = true;

        Enemy enemy;
        if (killer.gameObject.TryGetComponent(out enemy))
        {
            if(enemy.tag.Equals("DestructibleEnemy") && self.gameObject.GetComponent<Movement>().isDashing)
            {
                return;
            }
        }
        IEnumerator SlowMoDeath()
        { 
            GameManager gameManager = FindObjectOfType<GameManager>();
            PhaseManager phaseManager = FindObjectOfType<PhaseManager>();
            if(phaseManager && phaseManager.CurrentPhase == phaseManager.StartingPhase)
            {
                phaseManager.PhaseChanger();
            }
            yield return new WaitForSeconds(0.8f);
            self.transform.position = self.GetComponent<Movement>().spawnLocation;
            player.animator.SetBool("isDead", false);
            player.rb.isKinematic = false;
            gameManager.CamPos = gameManager.CamResetPos;
            gameManager.DeathCount++;
            yield return new WaitForSeconds(0.2f);
            player.input.ActivateInput();
            isDead = false;
        }
        player.animator.SetBool("isDead", true);
        player.playerAudio.PlayDeath();
        // Disable movement and inputs
        player.rb.velocity = Vector3.zero;
        player.rb.isKinematic = true;
        player.input.DeactivateInput();

        self.StartCoroutine(SlowMoDeath());
    }

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        destructible = GetComponent<Destructible>();
        playerAudio = GetComponent<PlayerAudio>();
        col = GetComponent<Collision>();
        isDead = false;
        destructible.OnDeath = PlayerDeathHandler;
        Debug.Assert(animator != null);
    }
}