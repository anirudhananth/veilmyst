using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int DeathCount;

    Camera MainCam;
    public Vector3 CPPos;
    public Vector3 CamPos;
    public float speed = 5f;
    private float distance;
    private float distanceCovered;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        MainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //MainCam.transform.position = CamPos;
        
    }

    public IEnumerator move()
    {
        speed = 0;
        distance = Vector3.Distance(MainCam.transform.position, CamPos);
        Time.timeScale = 0;
        while (speed < 1)
        {
            MainCam.transform.position = Vector3.Lerp(MainCam.transform.position, CamPos, speed);
            speed += 0.02f;

            //Time.timeScale = speed;
            yield return null;
        }
        Time.timeScale = 1;
    }
}
