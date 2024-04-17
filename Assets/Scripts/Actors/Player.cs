using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Destructible))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : Actor
{
    public Animator animator;

    private PlayerInput input;
    private Rigidbody2D rb;
    private Destructible destructible;
    private Vector2 spawnLocation;

    public static void PlayerDeathHandler(Destructible self, GameObject killer)
    {

        Player player = self.GetComponent<Player>();

        LethalCollision lc;
        if(killer.gameObject.TryGetComponent(out lc))
        {
            if(lc.Parent && lc.Parent.tag.Equals("DestructibleEnemy") && self.gameObject.GetComponent<Movement>().isDashing)
            {
                return;
            }
        }
        IEnumerator SlowMoDeath()
        {
            yield return new WaitForSeconds(0.8f);
            self.transform.position = self.GetComponent<Movement>().spawnLocation;
            player.animator.SetBool("isDead", false);
            player.rb.isKinematic = false;
            yield return new WaitForSeconds(0.2f);
            player.input.ActivateInput();
        }
        player.animator.SetBool("isDead", true);
        // Disable movement and inputs
        player.rb.velocity = Vector3.zero;
        player.rb.isKinematic = true;
        player.input.DeactivateInput();

        self.StartCoroutine(SlowMoDeath());
    }

    private void Start()
    {
        spawnLocation = GameObject.FindWithTag("Player").transform.position;
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        destructible = GetComponent<Destructible>();
        destructible.OnDeath = PlayerDeathHandler;
        Debug.Assert(animator != null);
    }
}