using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApplication : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Debug.Log("We pushed Escape & quitted the game.");
            Application.Quit();
        }
    }
}
