using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEngine : MonoBehaviour
{
    [SerializeField] private GameObject beforeStartDialog;
    [SerializeField] private GameObject endDialog;
    [SerializeField] private GameObject failDialog;
    [SerializeField] private Sprite[] colors;
    [SerializeField] private Sprite defaultColor;
    [SerializeField] private Image[] colorImages;
    [SerializeField] private int[] selectedColors;
    [SerializeField] private float startTime;
    [SerializeField] private float timeRemaining;
    [SerializeField] private int gameStatus;
    [SerializeField] private int levelIndex;
    [SerializeField] private int currentLevelIndex;
    [SerializeField] private Text lifeCountText;
    [SerializeField] private Text currentCoinText;
    [SerializeField] private Text endResultText;
    [SerializeField] private Text endCoinText;
    [SerializeField] private Text endTimeText;
    [SerializeField] private Text countDownTimeText;
    [SerializeField] private Text alertText;

    private int colorIndex;
    private int panelIndex;
    
    private int coinNum;
    private int totalNum;
    private int correctNum;
    private int lifeNum;
    bool startCounter;
    private bool isStarted;
    float levelTagTimer = 0.5f;

    void Start()
    {        
        RandomColors();
        timeRemaining = startTime;
        isStarted = false;

        levelIndex = PlayerPrefs.GetInt("LEVEL_PASSED");
        coinNum = PlayerPrefs.GetInt("COIN_OWN");
        currentCoinText.text = coinNum.ToString();
    }

    void Update()
    {
        if (startCounter)
        {
            CountDown();
        }
        else
        {
            if (Time.timeSinceLevelLoad >= levelTagTimer)
            {
                startCounter = true;
            }
        }

        if(gameStatus == 1)
        {
            // Select color palette
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                colorIndex = 0;    

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "color")
                    {                        
                        colorIndex = int.Parse(hit.transform.gameObject.name.Split(" ")[1]);                   
                    }
                }                
            }

            // Select colors
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                panelIndex = 0;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "panel")
                    {
                        panelIndex = int.Parse(hit.transform.gameObject.name.Split(" ")[1]);

                        if (colorIndex == selectedColors[panelIndex])
                        {                            
                            colorImages[panelIndex].sprite = colors[colorIndex];
                            correctNum++;

                            // Pass level
                            if (correctNum >= totalNum)
                            {
                                gameStatus = 0;
                                Time.timeScale = 0;
                                endDialog.SetActive(true);
                                endTimeText.text = countDownTimeText.text;
                                
                                // Compare with current level index
                                if(levelIndex == currentLevelIndex)
                                {
                                    levelIndex++;
                                    PlayerPrefs.SetInt("LEVEL_PASSED", levelIndex);
                                }                                                                

                                // Players can get coins according to time
                                if (180f - timeRemaining <= 60f)
                                {
                                    endResultText.text = "Best";
                                    endCoinText.text = "3";
                                    coinNum += 3;                                    
                                }                            
                                else if(timeRemaining <= 60f)
                                {
                                    endResultText.text = "Good";
                                    endCoinText.text = "1";
                                    coinNum += 1;
                                }
                                else
                                {
                                    endResultText.text = "Better";
                                    endCoinText.text = "2";
                                    coinNum += 2;
                                }

                                PlayerPrefs.SetInt("COIN_OWN", coinNum);
                            }
                        }
                        else
                        {                            
                            lifeNum--;
                            lifeCountText.text = lifeNum.ToString();
                            StopAllCoroutines();
                            StartCoroutine(DelayToShowAlert("Wrong!"));

                            //Fail level
                            if(lifeNum == 0)
                            {
                                failDialog.SetActive(true);
                            }
                        }
                    }
                }                
            }
        }
    }    

    // Arrange colors
    public void RandomColors()
    {
        switch (currentLevelIndex)
        {
            case 1:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 2:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 3:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 4:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 5:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 6:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 7:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 8:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 9:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 10:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 11:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 12:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 13:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 14:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 15:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
            case 16:
                {
                    totalNum = 2;
                    lifeNum = 4;
                    break;
                }
        }

        for (int i = 0; i < totalNum; i++)
        {
            selectedColors[i] = (System.DateTime.Now.Millisecond * UnityEngine.Random.Range(5, 15)) % 8;
            colorImages[i].sprite = colors[selectedColors[i]];           
        }

        if (selectedColors[0] == selectedColors[1])
        {
            selectedColors[1] = (selectedColors[1] + 1) % 8;
            colorImages[1].sprite = colors[selectedColors[1]];
        }        
    }

    // Start game
    public void StartGame()
    {
        if (!isStarted)
        {
            isStarted = true;
            startTime += 8f;
            timeRemaining = startTime;

            for (int i = 0; i < colorImages.Length; i++)
            {
                colorImages[i].sprite = defaultColor;
            }

            StartCoroutine(DelayToStartGame());     
        }               
    }

    // Go to next level
    public void NextLevel()
    {
        SceneManager.LoadScene("Level " + (currentLevelIndex + 1));
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Level " + currentLevelIndex);
    }

    public void BackBtnClick()
    {
        SceneManager.LoadScene("Main");
    }

    // Delay for 3 secs before starting game
    IEnumerator DelayToStartGame()
    {
        beforeStartDialog.SetActive(true);
        yield return new WaitForSeconds(3f);
        beforeStartDialog.SetActive(false);
        gameStatus = 1;
    }

    IEnumerator DelayToShowAlert(string str)
    {        
        alertText.transform.gameObject.SetActive(true);
        alertText.text = str;
        yield return new WaitForSeconds(2f);
        alertText.transform.gameObject.SetActive(false);
    }

    // Timer
    void CountDown()
    {
        timeRemaining = startTime - (Time.timeSinceLevelLoad - levelTagTimer);

        if (timeRemaining <= 0)
        {
            if(gameStatus == 0)
            {
                StartGame(); 
            }
            else
            {
                // End game
                gameStatus = 0;        
                Time.timeScale = 0;
                failDialog.SetActive(true);            
            }                        
        }

        // Showing colors
        if (timeRemaining <= 30)
        {
            countDownTimeText.color = Color.red;
        }
        else
        {
            countDownTimeText.color = Color.white;
        }

        ShowTime();
    }

    // Display timer
    void ShowTime()
    {
        int minutes;
        int seconds;
        string timeString;

        minutes = (int)timeRemaining / 60;
        seconds = (int)timeRemaining % 60;
        timeString = "0" + minutes.ToString() + ":" + seconds.ToString("d2");
        countDownTimeText.text = "0" + minutes.ToString() + ":" + seconds.ToString("d2");
    }
}
