using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Camera related vars
    Camera MainCam;
    PixelPerfectCamera PPCam;
    public Vector3 CPPos;
    public Vector3 CamPos;
    public int PixelPerUnit = 16;

    private float currentPixelPerUnit = 16;

    public float speed;

    // Player-related vars
    public GameObject player;
    public Movement playMovScript;

    // Misc
    public int DeathCount;

    // Start is called before the first frame update
    void Start()
    {
        MainCam = Camera.main;
        PPCam = MainCam.GetComponent<PixelPerfectCamera>();
        currentPixelPerUnit = PixelPerUnit = PPCam.assetsPPU;
        
        player = GameObject.FindGameObjectWithTag("Player");
        playMovScript = player.GetComponent<Movement>();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // playMovScript.spawnLocation = CPPos;
        if (MainCam.transform.position != CamPos)
        {
            MainCam.transform.position = Vector3.Lerp(MainCam.transform.position, CamPos, speed * Time.deltaTime);
            // if (speed < 1) speed = 4f;
            // speed += 0.08f * Time.deltaTime;
        }
        if (PixelPerUnit != (int)currentPixelPerUnit)
        {
            currentPixelPerUnit = Mathf.Lerp(currentPixelPerUnit, PixelPerUnit, speed * Time.deltaTime);
            PPCam.assetsPPU = (int)currentPixelPerUnit;
        }
    }
}
