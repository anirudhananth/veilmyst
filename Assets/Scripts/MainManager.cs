using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public float MaxStamina=100;
    private void Awake()
    {
        if(Instance ==null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void IncreaseStamina(float amount)
    {
        MaxStamina+=amount;
    }
}
