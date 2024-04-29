using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseButton : MonoBehaviour
{
    public PhaseManager PM;
    public float cooldown = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cooldown < Time.time)
        {
            PM.PhaseChanger();
            cooldown = Time.time + .5f;
        }
    }
}
