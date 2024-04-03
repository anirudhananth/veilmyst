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

    public static void PlayerDeathHandler(Destructible self, GameObject killer)
    {
        IEnumerator SlowMoDeath()
        {
            yield return new WaitForSeconds(0.8f);
            Destroy(self.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        Player player = self.GetComponent<Player>();
        player.animator.SetBool("isDead", true);
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
        destructible.OnDeath = PlayerDeathHandler;
        Debug.Assert(animator != null);
    }
}