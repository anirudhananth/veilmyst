using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public GameObject Bullet;
    public float firerate = 2.0f;
    public GameObject B1;
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
        yield return new WaitForSeconds(firerate);
        Debug.Log("Fire");
        B1 = Instantiate(Bullet,gameObject.transform.position, Quaternion.identity);
        if(gameObject.transform.position.x-gameObject.transform.parent.transform.position.x>=0)
        {
            B1.GetComponent<Bullet>().facingside=1;
        }
        else
        {
            B1.GetComponent<Bullet>().facingside=-1;
        }
        //yield return new WaitForSeconds(firerate);
        StartCoroutine(Fire());
    }
}
