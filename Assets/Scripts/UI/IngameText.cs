using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    private TextMeshProUGUI textBox;

    private void Start()
    {
        textBox = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        textBox.enabled = Menu.FocusedMenu == null && MainManager.InGame;
    }
}
