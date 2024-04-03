using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Destructible))]
public class Player : Actor
{
    private Destructible destructible;

    public static void PlayerDeathHandler(Destructible self, GameObject killer)
    {
        IEnumerator SlowMoDeath()
        {
            Time.timeScale = 0.1f;
            yield return new WaitForSeconds(0.2f);
            Time.timeScale = 1f;
            Destroy(self.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        self.StartCoroutine(SlowMoDeath());
    }

    private void Start()
    {
        destructible = GetComponent<Destructible>();
        destructible.OnDeath = PlayerDeathHandler;
    }
}