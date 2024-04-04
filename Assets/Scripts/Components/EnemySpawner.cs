using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject SpawnEnemy;

    [SerializeField]
    float SpawnDelay = 2.0f;

    public GameObject SpawnPoint;
    public GameObject PatrolPoint;

    public Vector3 SpawnPosition;
    public Vector3 PatrolPosition;

    public GameObject SpawnedPawn;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPosition = SpawnPoint.transform.position;
        PatrolPosition = PatrolPoint.transform.position;

        SpawnedPawn = Instantiate(SpawnEnemy,SpawnPosition, Quaternion.identity);
        SpawnedPawn.GetComponent<Patrolling>().pointA = SpawnPoint;
        SpawnedPawn.GetComponent<Patrolling>().pointB = PatrolPoint;
        SpawnedPawn.GetComponent<Enemy>().isSpawned = true;
        SpawnedPawn.GetComponent<Enemy>().Spawner = this;

    }

    IEnumerator SpawnPawn()
    {
        yield return new WaitForSeconds(SpawnDelay);
        SpawnedPawn = Instantiate(SpawnEnemy,SpawnPosition, Quaternion.identity);
        SpawnedPawn.GetComponent<Patrolling>().pointA = SpawnPoint;
        SpawnedPawn.GetComponent<Patrolling>().pointB = PatrolPoint;
        SpawnedPawn.GetComponent<Enemy>().isSpawned = true;
        SpawnedPawn.GetComponent<Enemy>().Spawner = this;
    }

    public void Spawn(GameObject gameobject)
    {
        Destroy(gameobject);
        StartCoroutine(SpawnPawn());
    }
}
