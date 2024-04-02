using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LethalCollision : MonoBehaviour
{
    [SerializeField]
    String lethaltag = "Player";

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag.Equals(lethaltag))
        {
            //This is just place holder, currently we destroy the player
            StartCoroutine(onCollision(other.gameObject));
        }
    }
    
    IEnumerator onCollision(GameObject gameObject)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1f;
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
