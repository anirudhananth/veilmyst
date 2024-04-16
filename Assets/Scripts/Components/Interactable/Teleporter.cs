using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Player player;
        if (other.gameObject.TryGetComponent<Player>(out player))
        {
            Triggerbehavior(other.gameObject);
        }
    }
    
    private void Triggerbehavior(GameObject gameObject)
    {
        Teleport( gameObject);
    }

    private void Teleport(GameObject gameObject)
    {
        gameObject.transform.SetPositionAndRotation(Target.transform.position,Quaternion.identity);
    }
}
