using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleFacing : MonoBehaviour
{
    public bool FaceLeft = true;
    public bool DefaultFaceLeft = true;
    private Vector2 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        if (DefaultFaceLeft ^ FaceLeft)
        {
            transform.localScale = new Vector2(-originalScale.x, originalScale.y);
        }
        else 
        {
            transform.localScale = originalScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
