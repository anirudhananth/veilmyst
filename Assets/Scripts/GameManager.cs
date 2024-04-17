using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Camera related vars
    Camera MainCam;
    public Vector3 CPPos;
    public Vector3 CamPos;
    public float speed;
    public float speedScalar;
    private float distance;
    private float duration;
    private float distanceCovered;

    // Player-related vars
    public GameObject player;
    public Movement playMovScript;

    // Enemy-related vars
    public List <GameObject> enemies;
    public List <Vector3> enemPos;

    // Misc
    public int DeathCount;
    int i;

    // Start is called before the first frame update
    void Start()
    {
        MainCam = Camera.main;
        
        // Find Player and Player-related stuff
        player = GameObject.FindGameObjectWithTag("Player");
        playMovScript = player.GetComponent<Movement>();

        FindEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        // playMovScript.spawnLocation = CPPos;

        if (MainCam.transform.position != CamPos)
        {
            MainCam.transform.position = Vector3.Lerp(MainCam.transform.position, CamPos, speed);
            speed += speedScalar * Time.deltaTime;
        }
    }

    public IEnumerator move()
    {
        speed = 0;
        distance = Vector3.Distance(MainCam.transform.position, CamPos);

        //Time.timeScale = 0;
        playMovScript.canDash = false;
        duration = 1f;
        while (speed < 1)
        {
            MainCam.transform.position = Vector3.Lerp(MainCam.transform.position, CamPos, speed / duration);
            speed += 2f * Time.deltaTime;

            //Time.timeScale = speed;
            yield return new WaitForFixedUpdate();
        }
        Time.timeScale = 1;
    }

    public void FindEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.position = enemPos[i];
        }

        enemies.Clear();
        enemPos.Clear();

        // Find enemies and enemy-related stuff
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("DestructibleEnemy"))
        {
            enemies.Add(enemy);
        }
        foreach (GameObject enemy in enemies)
        {
            enemPos.Add(enemy.transform.position);
        }
    }

    public void ResetLevel()
    {
        player.transform.position = CPPos;

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.position = enemPos[i];
        }
    }

    public void NewCheckpoint(Vector3 CheckPointPos, Vector3 CameraPos)
    {
        CPPos = CheckPointPos;
        CamPos = CameraPos;
        if (MainCam.transform.position != CamPos)
        {
            speed = 0;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.position = enemPos[i];
        }
    }
}
