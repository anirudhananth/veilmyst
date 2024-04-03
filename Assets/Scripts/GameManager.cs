using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector3 CPPos;
    public Vector3 CamPos;
    public int DeathCount;

    Camera MainCam;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        MainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MainCam.transform.position = CamPos;
    }
}
