using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Showable))]
public class IngameUI : MonoBehaviour
{
    private Showable group;

    private void Start()
    {
        group = GetComponent<Showable>();
    }

    private void Update()
    {
        group.SetShow(Menu.FocusedMenu == null && MainManager.InGame);
    }
}
