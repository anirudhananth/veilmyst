using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Start is called before the first frame update
    public float staminaincreaseamount = 5.0f;
    void Start()
    {
        
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
        MainManager.Instance.IncreaseStamina(staminaincreaseamount);
        Destroy(gameObject);
    }
}