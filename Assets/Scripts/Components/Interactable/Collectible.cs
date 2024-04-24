using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Stamina Var
    public float staminaincreaseamount = 5.0f;

    // For Level Manager
    public string Name;
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
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
        levelManager.CollectedCollectible(gameObject.name);
        Destroy(gameObject);
    }
}
