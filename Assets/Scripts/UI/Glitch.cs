using UnityEngine;
using UnityEngine.Rendering;

public class Glitch : Showable
{
    public Volume glitchEffect;

    public override void SetShow(bool show) { 
        glitchEffect.enabled = show;
    }

    public override void Toggle() {
        glitchEffect.enabled = !glitchEffect.enabled;
    }

    private void Start() {
        SetShow(false);
    }
}