using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerInitializer : MonoBehaviour
{
    // Reference to the GameManager prefab
    public GameObject gameManagerPrefab; 

    private void Awake()
    {
        // Create a new GameManager object from the prefab
        Instantiate(gameManagerPrefab);
    }
}