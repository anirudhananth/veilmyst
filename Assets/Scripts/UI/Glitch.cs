using UnityEngine;

[RequireComponent(typeof(Kino.AnalogGlitch))]
public class Glitch : Showable
{
    private Kino.AnalogGlitch glitchEffect;

    public override void SetShow(bool show) { 
        glitchEffect.enabled = show;
    }

    public override void Toggle() {
        glitchEffect.enabled = !glitchEffect.enabled;
    }

    private void Start() {
        glitchEffect = GetComponent<Kino.AnalogGlitch>();
        SetShow(false);
    }
}