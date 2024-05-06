using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Showable))]
public class IngameUI : MonoBehaviour
{
    public bool Inverted = false;

    private Showable group;

    private void Start()
    {
        group = GetComponent<Showable>();
    }

    private void Update()
    {
        bool inGame = Menu.FocusedMenu == null && MainManager.InGame;
        if (Inverted) inGame = !inGame;
        group.SetShow(inGame);
    }
}
