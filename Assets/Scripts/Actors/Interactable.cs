using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private static readonly float FloatMaxOffset = 0.1f;

    private Vector3 initPos;
    private float floatRad = 0;
    [Tooltip("The time it takes for the interactable to float up and down in seconds")]
    private readonly float floatCycleInterval = 2f;
    private float floatSpeedRad;

    private void Start()
    {
        initPos = transform.position;
        floatSpeedRad = 2 * Mathf.PI / floatCycleInterval;
    }

    private void Update()
    {
        floatRad += floatSpeedRad * Time.deltaTime;
        Vector3 newPos = initPos;
        newPos.y += FloatMaxOffset * Mathf.Sin(floatRad);
        transform.position = newPos;
    }
}
