using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public GameObject Bullet;
    public float firerate = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fire());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator Fire()
    {
        GameObject B1 = Instantiate(Bullet,gameObject.transform.position, Quaternion.identity);
        B1.GetComponent<Bullet>().facingside = gameObject.transform.position.x;
        yield return new WaitForSeconds(firerate);
        StartCoroutine(Fire());
    }
}
