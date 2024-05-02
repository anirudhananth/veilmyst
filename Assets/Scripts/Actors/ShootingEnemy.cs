using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    public Launcher launcher;   
    private GameObject B1; 

    protected void OnEnable() 
    {
        base.Start();
        launcher = GetComponentInChildren<Launcher>();
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(launcher.firerate);
        animator.SetBool("isShooting", true);
        yield return new WaitForSeconds(0.05f);
        if(!isDead)
        {
            B1 = Instantiate(launcher.Bullet,launcher.gameObject.transform.position, Quaternion.identity);
            B1.transform.parent = gameObject.transform.parent;
            if(launcher.gameObject.transform.position.x-launcher.gameObject.transform.parent.transform.position.x>=0)
            {
                B1.GetComponent<Bullet>().facingside=1;
            }
            else
            {
                B1.GetComponent<Bullet>().facingside=-1;
            }
            yield return new WaitForSeconds(0.15f);
            animator.SetBool("isShooting", false);
            StartCoroutine(Fire());  
        }

    }
}
