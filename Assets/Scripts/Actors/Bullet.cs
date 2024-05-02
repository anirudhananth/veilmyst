using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float destroytime = 4f;
    public float speed = 1f;

    public float facingside;

    public Vector3 finalposition;
    public bool canbedestroyed=false;
    // Start is called before the first frame update
    void Start()
    {
        finalposition = gameObject.transform.position+new Vector3(facingside,0,0)*999;
        StartCoroutine(SetBulletDestroyable());
        Destroy(gameObject,destroytime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, finalposition, speed*Time.deltaTime);
    }
    IEnumerator SetBulletDestroyable()
    {
        yield return new WaitForSeconds(0.2f);
        canbedestroyed=true;
    }
}
