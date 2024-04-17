using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Stamina stamina;

    [SerializeField] private Slider slider;

    public void UpdateStaminaBar()
    {
        slider.value =  stamina.currentstamina/stamina.maxstamina;
    }
    void Update() 
    {
        UpdateStaminaBar();
    }
}
