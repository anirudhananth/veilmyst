using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Level vars
    public static LevelManager Instance;
    public int levelsCompleted = 0;
    public int currentLevel;

    // Collectible Vars
    private Collectible collectibleScript;
    public GameObject[] collectibles;

    [System.Serializable]
    public struct Collectibles
    {
        public string Name;
        public bool State;

        public Collectibles(string name, bool state)
        {
            this.Name = name;
            this.State = state;
        }

        public void Collect()
        {
            this.State = true;
        }
    }

    // List of collectibles and single collectible
    // [System.Serializable]
    public List<Collectibles> collectiblesList = new List<Collectibles> ();
    public Collectibles singleCollectible;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void CompletedLevel()
    {
        if(currentLevel < levelsCompleted)
        {
            levelsCompleted = currentLevel;
        }
        currentLevel++;
    }

    public void FindCollectibles()
    {
        Debug.Log("loaded");

        collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        bool collectibleFound = false;

        foreach (GameObject collectible in collectibles)
        {
            // add collectible to list of gameObjects in this level
            Debug.Log(collectible);

            collectibleScript = collectible.GetComponent<Collectible>(); // find script to get name of collectible
            // Set the valuse for current collectible
            singleCollectible.Name = collectibleScript.name;
            singleCollectible.State = false;
            collectibleFound = false;

            foreach (Collectibles col in collectiblesList)
            {
                if (singleCollectible.Name == col.Name) // if collectible is already in the list
                {
                    if (col.State) // if already collected
                    {
                        Destroy(collectible);
                    }
                    collectibleFound = true;
                    break;
                }
            }

            if (!collectibleFound) // add to list if not already there 
            {
                collectiblesList.Add(singleCollectible);
            }
        }
    }

    public void CollectedCollectible(string name)
    {
        for(int i = 0; i <  collectiblesList.Count; i++)
        {
            if (collectiblesList[i].Name == name)
            {
                collectiblesList[i].Collect();
                break;
            }
        }
    }
}
