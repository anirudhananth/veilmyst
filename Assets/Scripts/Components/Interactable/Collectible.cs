using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

[RequireComponent(typeof(AudioSource))]
public class Collectible : MonoBehaviour
{
    // Start is called before the first frame update
    public float staminaincreaseamount = 5.0f;
    private bool isDead = false;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        Player player;
        if (other.gameObject.TryGetComponent<Player>(out player))
        {
            Triggerbehavior();
        }
    }

    private void Triggerbehavior()
    {
        if (isDead) return;
        isDead = true;
        MainManager.Instance.IncreaseStamina(staminaincreaseamount);
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        source.Play();
        Destroy(gameObject, 1f);
    }
}
