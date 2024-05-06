using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneName = "1-2";
    public bool canteleport = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canteleport)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                OpenScene();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        Player player;
        if (other.gameObject.TryGetComponent<Player>(out player))
        {
            canteleport = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        Player player;
        if (other.gameObject.TryGetComponent<Player>(out player))
        {
            canteleport = false;
        }
    }

    private void OpenScene()
    {
        MainManager.LoadScene(sceneName, false);
    }
}
