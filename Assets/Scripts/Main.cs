using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LevelSelect(string levelStr)
    {
        SceneManager.LoadScene("Level " + levelStr);
    }
}
