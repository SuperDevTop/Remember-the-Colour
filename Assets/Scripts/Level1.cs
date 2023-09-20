using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    [SerializeField] private GameObject beforeStartDialog;
    [SerializeField] private GameObject endDialog;
    [SerializeField] private Sprite[] colors;
    [SerializeField] private Sprite defaultColor;
    [SerializeField] private Image[] colorImages;
    [SerializeField] private int[] selectedColors;
    [SerializeField] private float startTime;
    [SerializeField] private float timeRemaining;
    [SerializeField] private int gameStatus;
    [SerializeField] private Text lifeCountText;
    [SerializeField] private Text endResultText;
    [SerializeField] private Text endCoinText;
    [SerializeField] private Text endTimeText;
    [SerializeField] private Text countDownTimeText;
    [SerializeField] private Text alertText;

    private int colorIndex;
    private int panelIndex;
    private int totalNum;
    private int correctNum;
    private int lifeNum;
    bool startCounter;
    float levelTagTimer = 0.5f;

    void Start()
    {
        RandomColors();
        timeRemaining = startTime;
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

                            if(correctNum >= totalNum)
                            {
                                gameStatus = 0;
                                Time.timeScale = 0;
                                endDialog.SetActive(true);
                                endTimeText.text = countDownTimeText.text;

                                if(180f - timeRemaining <= 60f)
                                {
                                    endResultText.text = "Best";
                                    endCoinText.text = "3";
                                }                            
                                else if(timeRemaining <= 60f)
                                {
                                    endResultText.text = "Good";
                                    endCoinText.text = "1";
                                }
                                else
                                {
                                    endResultText.text = "Better";
                                    endCoinText.text = "2";
                                }
                            }
                        }
                        else
                        {
                            lifeNum--;
                            lifeCountText.text = lifeNum.ToString();
                            StopAllCoroutines();
                            StartCoroutine(DelayToShowAlert("Wrong!"));
                        }
                    }
                }                
            }
        }
    }    

    // Arrange colors
    public void RandomColors()
    {
        for(int i = 0; i < 2; i++)
        {
            selectedColors[i] = (System.DateTime.Now.Millisecond * UnityEngine.Random.Range(5, 15)) % 8;
            colorImages[i].sprite = colors[selectedColors[i]];           
        }

        if (selectedColors[0] == selectedColors[1])
        {
            selectedColors[1] = (selectedColors[1] + 1) % 8;
            colorImages[1].sprite = colors[selectedColors[1]];
        }

        totalNum = 2;
        lifeNum = 4;
    }

    // Start game
    public void StartGame()
    { 
        startTime += 183f;
        timeRemaining = startTime;

        for(int i = 0; i < colorImages.Length; i++)
        {
            colorImages[i].sprite = defaultColor;
        }

        StartCoroutine(DelayToStartGame());
    }

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
