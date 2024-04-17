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
    private float distance;
    private float duration;
    private float distanceCovered;

    // Player-related vars
    public GameObject player;
    public Movement playMovScript;

    // Misc
    public int DeathCount;

    // Start is called before the first frame update
    void Start()
    {
        MainCam = Camera.main;
        
        player = GameObject.FindGameObjectWithTag("Player");
        playMovScript = player.GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        // playMovScript.spawnLocation = CPPos;

        if (MainCam.transform.position != CamPos)
        {
            MainCam.transform.position = Vector3.Lerp(MainCam.transform.position, CamPos, speed);
            speed += 0.08f * Time.deltaTime;
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
            speed += 0.02f * Time.fixedDeltaTime;

            //Time.timeScale = speed;
            yield return new WaitForFixedUpdate();
        }
        Time.timeScale = 1;
    }
}
