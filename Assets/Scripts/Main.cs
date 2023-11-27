using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static Main instance;
    [SerializeField] private Button[] levelBtns;
    public Text shopCoinText;

    void Start()
    {
        instance = this;
        shopCoinText.text = PlayerPrefs.GetInt("COIN_OWN").ToString();

        if (PlayerPrefs.GetInt("LEVEL_PASSED") == 0)
        {
            PlayerPrefs.SetInt("LEVEL_PASSED", 1);
        }
        
        for(int i = 0; i < PlayerPrefs.GetInt("LEVEL_PASSED"); i++)
        {
            levelBtns[i].interactable = true;
        }
    }

    void Update()
    {
        
    }

    public void LevelSelect(string levelStr)
    {
        SceneManager.LoadScene("Level " + levelStr);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
