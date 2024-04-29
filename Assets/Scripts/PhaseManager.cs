using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhaseManager : MonoBehaviour
{
    [Header("Phase Objects")]
    public bool TargetPhase;
    public GameObject phaseA;
    public GameObject phaseB;

    [Header("Timing Variables")]
    private float timer = 0;
    public float cooldown; // The amount by which the timer increases
    public TMP_Text timerUI;
    public float timeLeft;

    [Header("Phase Type")]
    public bool dashType;
    public bool timeType;

    [Header("Dash Type")]
    public GameObject Player;
    Movement playerMovement;
    PlayerInput playerInput;
    InputAction toggleAction;

    [Header("Time Type")]
    private float shifTimer = 0; // If it is in timer mode then this is when it will change phase
    public float shiftCD; // The amount by which shiftimer changes

    public float GlitchDuration = 0.1f;
    private float transitionTimeout;
    private bool curPhase;

    private Glitch glitch
    {
        get => GameManager.Instance.MainCam.GetComponent<Glitch>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Player
        playerMovement = Player.GetComponent<Movement>();
        playerInput = Player.GetComponent<PlayerInput>();
        toggleAction = playerInput.actions["Phase"];

        // UI
        timerUI.GetComponent<TextMeshProUGUI>().text = "0";
        curPhase = !TargetPhase;
    }

    // Update is called once per frame
    void Update()
    {
        transitionTimeout -= Time.deltaTime;

        if (TargetPhase != curPhase && transitionTimeout <= 0)
        {
            curPhase = TargetPhase;
            glitch.SetShow(false);
            // Phase Change basics
            if (TargetPhase)
            {
                phaseA.SetActive(true);
                phaseB.SetActive(false);
            }
            else
            {
                phaseA.SetActive(false);
                phaseB.SetActive(true);
            }
        }

        // If dashType
        if (timer > Time.time)
        {
            timeLeft = timer - Time.time;
            timerUI.text = timeLeft.ToString();
            if (timeType)
            {
                TimedChanger();
            }
            if (dashType)
            {
                DashChanger();
            }
        }

        // Button for toggle
        if (toggleAction.triggered)
        {
            TogglePhase();
        }
    }

    private void TogglePhase()
    {
        // Toggle the phase and set off the transition effect
        TargetPhase = !TargetPhase;
        glitch.SetShow(true);
        transitionTimeout = GlitchDuration;
    }

    public void PhaseChanger() // Called by button
    {
        timer = Time.time + cooldown;
        // phase = !phase;
        if (!timeType && !dashType)
        {
            TogglePhase();
        }
    }

    void DashChanger() // Change when dashing
    {
        if (playerMovement.isDashing && shifTimer < Time.time)
        {
            TogglePhase();
            shifTimer = Time.time + 1;
        }
    }

    void TimedChanger() // Phase changes after some seconds
    {
        if (shifTimer < Time.time)
        {
            TogglePhase();
            shifTimer = Time.time + shiftCD;
        }
    }
}
