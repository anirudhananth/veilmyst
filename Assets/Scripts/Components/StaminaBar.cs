using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Stamina stamina;
    public bool isvisible=false;//TODO move this to game manager

    [SerializeField] private Slider slider;

    public void UpdateStaminaBar()
    {
        slider.value =  stamina.currentstamina/stamina.maxstamina;
    }
    private void Start() {

            gameObject.SetActive(isvisible);
    }
    void Update() 
    {
        UpdateStaminaBar();
    }
}
