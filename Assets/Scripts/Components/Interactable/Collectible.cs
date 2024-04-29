using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

[RequireComponent(typeof(AudioSource))]
public class Collectible : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public AudioClip ActivateAudio;
    public AudioClip CollectAudio;

    private bool isTouched = false;
    private bool isDead = false;
    private float groundedTimeout = 0;
    private const float maxGroundedTimeout = 0.05f;

    private AudioSource source;
    private Player player;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            player = GameManager.Instance.player.GetComponent<Player>();
            player.destructible.OnDeath += HandlePlayerDeath;
        }

        if (isDead) return;

        if (isTouched)
        {
            if (player.col.onGround)
            {
                groundedTimeout -= Time.deltaTime;
                if (groundedTimeout > 0) return;
                Collect();
            }
            else
            {
                groundedTimeout = maxGroundedTimeout;
            }
        }
    }

    private void Collect()
    {
        if (isDead) return;
        isDead = true;
        animator.GetComponent<SpriteRenderer>().enabled = false;
        GameObject effect = Instantiate(GameManager.Instance.EffectsPrefab);
        effect.GetComponent<SpriteEffects>().Play(transform.position, "pickup");
        source.clip = CollectAudio;
        source.Play();
        Destroy(gameObject, 1f);
    }

    private void HandlePlayerDeath(Destructible destructible, GameObject killer)
    {
        isTouched = false;
        if (animator) animator.SetBool("active", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out player))
        {
            if (isTouched) return;
            groundedTimeout = maxGroundedTimeout;
            isTouched = true;
            animator.SetBool("active", true);
            source.clip = ActivateAudio;
            source.Play();
        }
    }
}
