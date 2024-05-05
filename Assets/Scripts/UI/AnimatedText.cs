using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AnimatedText : MonoBehaviour
{
    public Animator TextAnimator;
    public float MaxAnimationTimeout = 1f;

    public string text
    {
        get => finalText;
        set => UpdateText(value);
    }
    private TextMeshProUGUI textbox;

    private string finalText;
    private float animationTimeout;

    private void UpdateText(string s)
    {
        if (finalText == s) return;
        animationTimeout = MaxAnimationTimeout;
        TextAnimator.SetBool("animating", true);
        finalText = s;
    }

    private void Start()
    {
        textbox = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (animationTimeout <= 0)
        {
            textbox.text = finalText;
            TextAnimator.SetBool("animating", false);
        }
        else
        {
            animationTimeout -= Time.deltaTime;
        }
    }
}