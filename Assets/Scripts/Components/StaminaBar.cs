using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Stamina stamina;

    public void UpdateStaminaBar()
    {
        slider.value =  stamina.currentstamina/stamina.maxstamina;
    }
    void Update() 
    {
        UpdateStaminaBar();
    }
}
