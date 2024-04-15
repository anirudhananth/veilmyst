using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    // Start is called before the first frame update
    public float currentstamina;
    public float defaultmaxstamina = 100;
    public float maxstamina;

    public Movement movement;

    [SerializeField]
    public float normaljump_stamina = 30;
    [SerializeField]
    public float fulljump_stamina = 60;
    [SerializeField]
    public float dash_stamina = 80;
    [SerializeField]
    public float sliding_10ms_stamina = 0.5f;
    [SerializeField]
    public float ground_charge_10ms_stamina = 1.25f;

    public bool canrecharge;

    private void Start() 
    {
        maxstamina = defaultmaxstamina;
        currentstamina = maxstamina;
    }
    private void Update()
    {
        if(movement.wallSlide)
        {
            SlideReduceStamina();
        }
        else if(canrecharge)
        {
            RechargeStamina();
        }
    }

    private void RechargeStamina()
    {
        currentstamina+=(Time.deltaTime/0.01f)*ground_charge_10ms_stamina;
        if(currentstamina>maxstamina)
        {
            currentstamina = maxstamina;
        }
    }
    private void SlideReduceStamina()
    {
        currentstamina-=(Time.deltaTime/0.01f)*sliding_10ms_stamina;
        if(currentstamina<=0)
        {
            currentstamina = 0;
            movement.wallSlide = false;
        }
    }

    public void StartRecharge()
    {
        Debug.Log("r");
        canrecharge = true;
    }

    public void StopRecharge()
    {
        canrecharge = false;
    }
    public bool ReduceStamina(float amount)
    {
        if((currentstamina-amount)<0)
        {
            return false;
        }
        else
        {
            currentstamina-=amount;
            return true;
        }
    }
}
