using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Enemy : Actor
{
    public bool isSpawned = false;
    public bool isDead = false;
    public EnemySpawner Spawner;


    public void onDeath()
    {
        if(isDead)
        {
            return;  
        }
        else
        {
            isDead=true;
        }
            
        if(isSpawned)
        {
            Spawner.Spawn(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
