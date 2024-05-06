using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraFallLoop : MonoBehaviour
{
    public float FallSpeed;
    public float ResetDistance;
    public float OscillateSpeedRad;
    public float OscillateDistance;
    public GameObject StillObject;

    private Vector3 initialPos;
    private Vector3 stillObjectOffset;
    private float yOffsetRad = 0;
    private CinemachineBrain brain;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        stillObjectOffset = StillObject.transform.position - transform.position;
    }

    public void FixedUpdate()
    {
        if (brain == null)
        {
            brain = FindObjectOfType<CinemachineBrain>();
            if (brain == null) return;
        }
        if (brain.IsBlending) return;
        var pos = transform.position;
        pos.y += FallSpeed * Time.fixedDeltaTime;
        if (Mathf.Abs(pos.y - initialPos.y) > ResetDistance)
        {
            pos = initialPos;
        }
        transform.position = pos;
        yOffsetRad += OscillateSpeedRad * Time.fixedDeltaTime;
        Vector3 yAdjustment = Vector3.down * Mathf.Sin(yOffsetRad) * OscillateDistance;
        StillObject.transform.position = transform.position + stillObjectOffset + yAdjustment;
    }
}
