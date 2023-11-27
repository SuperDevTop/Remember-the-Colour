using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEngine : MonoBehaviour
{
    public static GameEngine instance;
    
    [SerializeField] private GameObject beforeStartDialog;
    [SerializeField] private GameObject endDialog;
    [SerializeField] private GameObject failDialog;
    [SerializeField] private GameObject continueDialog;
    [SerializeField] private Image[] colors;
    [SerializeField] private Image[] colorImages;
    [SerializeField] private Button skipBtn;
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
    [SerializeField] private Text continueLifeText;
    [SerializeField] private Text continueCoinText;
    public Text shopCoinText;
    [SerializeField] private Text countDownTimeText;
    [SerializeField] private Text alertText;

    private int colorIndex;
    private int panelIndex;

    private float skipedTime = 0;
    public int coinNum;
    private int totalNum;
    private int correctNum;
    private int lifeNum;
    bool startCounter;
    private bool isStarted;
    float levelTagTimer = 0.5f;

    void Start()
    {
        instance = this;

        RandomColors();
        startTime = 60f;
        timeRemaining = startTime;
        isStarted = false;

        levelIndex = PlayerPrefs.GetInt("LEVEL_PASSED");
        coinNum = PlayerPrefs.GetInt("COIN_OWN");
        currentCoinText.text = coinNum.ToString();    
        Time.timeScale = 1;
        colorIndex = 100;
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

                        if(colorIndex != 100)
                        {
                            if (colorIndex == selectedColors[panelIndex])
                            {
                                colorImages[panelIndex].color = colors[colorIndex].color;
                                colorImages[panelIndex].GetComponent<Collider>().enabled = false;
                                correctNum++;

                                // Pass level
                                if (correctNum >= totalNum)
                                {
                                    gameStatus = 0;
                                    Time.timeScale = 0;
                                    endDialog.SetActive(true);
                                    endTimeText.text = countDownTimeText.text;

                                    // Compare with current level index
                                    if (levelIndex == currentLevelIndex)
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
                                    else if (timeRemaining <= 60f)
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
                                if (lifeNum == 0)
                                {
                                    failDialog.SetActive(true);
                                    gameStatus = 0;
                                    Time.timeScale = 0;
                                }
                            }
                        }
                    }
                }

                colorIndex = 100;
            }
        }
    }    

    // Arrange colors
    public void RandomColors()
    {
        lifeNum = 4;
        totalNum = colorImages.Length;

        for (int i = 0; i < totalNum; i++)
        {
            selectedColors[i] = (System.DateTime.Now.Millisecond * UnityEngine.Random.Range(5, 15)) % 8;
            colorImages[i].color = colors[selectedColors[i]].color;           
        }

        // Check repeated colors
        for (int i = 0; i < totalNum - 1; i++)
        {
            for (int j = i + 1; j < totalNum; j++)
            {
                if (selectedColors[i] == selectedColors[j])
                {
                    selectedColors[j] = (selectedColors[j] + 1) % 8;
                    colorImages[j].color = colors[selectedColors[j]].color;
                }
            }
        }      
    }

    // Start game
    public void StartGame()
    {
        if (!isStarted)
        {
            isStarted = true;
            startTime = startTime + 183f - skipedTime;         
            timeRemaining = startTime;

            for (int i = 0; i < colorImages.Length; i++)
            {
                colorImages[i].color = Color.white;
                colorImages[i].GetComponent<Collider>().enabled = true;
            }

            StartCoroutine(DelayToStartGame());     
        }               
    }

    public void SkipBtnClick()
    {
        skipedTime = float.Parse(countDownTimeText.text.Split(":")[1]);
        StartGame();
        skipBtn.gameObject.SetActive(false);
    }

    // Go to next level
    public void NextLevel()
    {
        SceneManager.LoadScene("Level " + (currentLevelIndex + 1));
    }

    public void RefreshCurrentToContinue()
    {
        continueLifeText.text = lifeCountText.text;
        continueCoinText.text = currentCoinText.text;
        alertText.gameObject.SetActive(false);
    }


    // Continue Dialog
    public void RefreshContinueToCurrent()
    {
        lifeCountText.text = continueLifeText.text;
        currentCoinText.text = continueCoinText.text;
        alertText.gameObject.SetActive(false);
    }

    public void AddTime30()
    {
        if(lifeNum > 0)
        {
            if (coinNum >= 15)
            {
                coinNum -= 15;
                PlayerPrefs.SetInt("COIN_OWN", coinNum);
                currentCoinText.text = coinNum.ToString();
                startTime += 30;
                gameStatus = 1;
                Time.timeScale = 1;
                failDialog.SetActive(false);
                continueDialog.SetActive(false);
            }
            else
            {
                StartCoroutine(DelayToShowAlert("Not enough coin."));
            }
        }
        else
        {
            StartCoroutine(DelayToShowAlert("No life"));
        }                
    }

    public void AddTime90()
    {
        if (lifeNum > 0)
        {
            if (coinNum >= 30)
            {
                coinNum -= 30;
                PlayerPrefs.SetInt("COIN_OWN", coinNum);
                currentCoinText.text = coinNum.ToString();
                startTime += 90;
                gameStatus = 1;
                Time.timeScale = 1;
                failDialog.SetActive(false);
                continueDialog.SetActive(false);
            }
            else
            {
                StartCoroutine(DelayToShowAlert("Not enough coin."));
            }
        }
        else
        {
            StartCoroutine(DelayToShowAlert("No life"));
        }
    }

    public void AddTime180()
    {
        if (lifeNum > 0)
        {
            if (coinNum >= 60)
            {
                coinNum -= 60;
                PlayerPrefs.SetInt("COIN_OWN", coinNum);
                currentCoinText.text = coinNum.ToString();
                startTime += 180;
                gameStatus = 1;
                Time.timeScale = 1;
                failDialog.SetActive(false);
                continueDialog.SetActive(false);
            }
            else
            {
                StartCoroutine(DelayToShowAlert("Not enough coin."));
            }
        }
        else
        {
            StartCoroutine(DelayToShowAlert("No life"));
        }
    }

    public void AddLife()
    {
        if(timeRemaining <= 0)
        {
            StartCoroutine(DelayToShowAlert("You need to add time"));
        }
        else
        {
            if(coinNum < 100)
            {
                StartCoroutine(DelayToShowAlert("Not enough coin"));
            }
            else
            {
                coinNum -= 100;
                PlayerPrefs.SetInt("COIN_OWN", coinNum);
                currentCoinText.text = coinNum.ToString();
                lifeNum += 4;
                lifeCountText.text = lifeNum.ToString();          
                gameStatus = 1;
                Time.timeScale = 1;
                failDialog.SetActive(false);
                continueDialog.SetActive(false);
            }
        }
    }

    public void GoToShopBtnClick()
    {
        shopCoinText.text = currentCoinText.text;
    }

    // Shop Dialog
    public void ShopBackBtnClick()
    {
        continueCoinText.text = shopCoinText.text;
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
