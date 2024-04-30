using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("Main Variables")]
    public GameManager GM; // game manager
    public Vector3 CamPos;
    public Movement playerMovement;

    [Space]

    [Header("Set If Spawnpoint")]
    public bool isSpawnPoint = false;

    Camera MainCam;

    // Start is called before the first frame update
    void Start()
    {
        MainCam = Camera.main;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(isSpawnPoint) {
                playerMovement.spawnLocation = transform.position;
                GM.CamResetPos = CamPos;
            } 
            GM.CPPos = transform.position;
            GM.MoveCam(CamPos);
        }
    }
}
