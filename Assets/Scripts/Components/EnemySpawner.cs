using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject SpawnEnemy;
    public Animator Animator;

    [SerializeField]
    float SpawnDelay = 2.0f;
    [Tooltip("The time before the enemy is spawned")]
    [Min(0.33f)]
    public float SpawnAnticipation = 0.5f;

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
        SpawnedPawn.transform.parent = gameObject.transform.parent;
        SpawnedPawn.GetComponent<Patrolling>().pointA = SpawnPoint;
        SpawnedPawn.GetComponent<Patrolling>().pointB = PatrolPoint;
        SpawnedPawn.GetComponent<Enemy>().isSpawned = true;
        SpawnedPawn.GetComponent<Enemy>().Spawner = this;

        Debug.Assert(SpawnDelay >= SpawnAnticipation);
    }

    IEnumerator SpawnPawn()
    {
        yield return new WaitForSeconds(SpawnDelay - SpawnAnticipation);
        Animator.SetTrigger("prespawn");
        yield return new WaitForSeconds(SpawnAnticipation);
        Animator.SetTrigger("spawn");
        SpawnedPawn = Instantiate(SpawnEnemy,SpawnPosition, Quaternion.identity);
        SpawnedPawn.GetComponent<Patrolling>().pointA = SpawnPoint;
        SpawnedPawn.GetComponent<Patrolling>().pointB = PatrolPoint;
        SpawnedPawn.GetComponent<Enemy>().isSpawned = true;
        SpawnedPawn.GetComponent<Enemy>().Spawner = this;
    }

    public void Spawn()
    {
        StartCoroutine(SpawnPawn());
    }
}
