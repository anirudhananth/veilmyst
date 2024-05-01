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
    public CinemachineBrain Brain;

    private float initialPosY;
    private Vector3 stillObjectOffset;
    private float yOffsetRad = 0;

    // Start is called before the first frame update
    void Start()
    {
       initialPosY = transform.position.y; 
       stillObjectOffset = StillObject.transform.position - transform.position;
    }

    public void Update()
    {
        if (Brain.IsBlending) return;
        var pos = transform.position;
        pos.y += FallSpeed * Time.deltaTime; 
        if (Mathf.Abs(pos.y - initialPosY) > ResetDistance)
        {
            pos.y = initialPosY;
        }
        transform.position = pos;
        yOffsetRad += OscillateSpeedRad * Time.deltaTime;
        Vector3 yAdjustment = Vector3.down * Mathf.Sin(yOffsetRad) * OscillateDistance;
        StillObject.transform.position = transform.position + stillObjectOffset + yAdjustment;
    }
}
