using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public PlayerController cc;

    private int finalScore;
    bool loadNextScene;

    bool startTimer;
    public float timer;
    bool startBonusTimer;
    private float bonusTimer;
    private bool startFinalScoreCalc;
    private float startFinalScoreTimer;
    private bool startGradeTimer;
    private float GradeTimer;
    private bool skipFinalScoreCalc;


    public GameObject canvasTimerText;
    public Text timerText;
    public GameObject canvasScoreText;
    public Text scoreText;

    public int origFontSize;

    private bool endGame;

    public List<GameObject> collectables;
    public int collectablesAmount = 10;

    private int collectablesLeft;

    public List<string> grades;

    //CANVAS FOR SHOWING THE FINAL LEVEL SCORE
    private bool createCanvas;
    private GameObject endCanvasGO;
    private GameObject endScoreTextGO;
    private Text endScoreTextTXT;
    private GameObject endGradeTextGO;
    private Text endGradeTextTXT;
    private GameObject endScoreTitleGO;
    private Text endScoreTitleTXT;
    private GameObject endBonusTitleGO;
    private Text endBonusTitleTXT;

    bool waitForNextScene;

    //SCENE DATA
    private int sceneToLoad;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        cc = Player.GetComponent<PlayerController>();

        canvasTimerText = GameObject.Find("TimerText");
        timerText = canvasTimerText.GetComponent<Text>();
        canvasScoreText = GameObject.Find("ScoreText");
        scoreText = canvasScoreText.GetComponent<Text>();

        createCanvas = true;
        endGame = false;
        startTimer = false;
        startBonusTimer = false;
        loadNextScene = false;
        startFinalScoreCalc = false;
        startGradeTimer = false;
        skipFinalScoreCalc = false;
        waitForNextScene = false;

        timer = 10.0f;
        origFontSize = 40;
        finalScore = 0;
        bonusTimer = 1.0f;
        startFinalScoreTimer = 1.0f;
        GradeTimer = 1.0f;
        sceneToLoad = 1;

        collectablesLeft = collectablesAmount;

        CreateListOfGrades();
    }

    // Update is called once per frame
    void Update()
    {
        if (!waitForNextScene)
        {
            if (!loadNextScene)
            {
                if (!endGame)
                {
                    TimerBehavior();

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        SpawnCollectables();
                        startTimer = true;
                    }

                    if (collectablesLeft == 0 && startTimer)
                    {
                        startTimer = false;
                        endGame = true;
                    }

                }
                if (endGame)
                {
                    //do stuff on endgame

                    int tempCounter = 5;
                    cc.EndGame();

                    if (createCanvas && !startTimer)
                        CreateEndCanvas();

                    if (startBonusTimer && bonusTimer > 0)
                        bonusTimer -= 1.0f * Time.deltaTime;

                    if (startFinalScoreCalc && startFinalScoreTimer > 0)
                        startFinalScoreTimer -= 1.0f * Time.deltaTime;

                    if (startGradeTimer && GradeTimer > 0)
                        GradeTimer -= 1.0f * Time.deltaTime;

                    bool calculationFinished = cc.CalculateScore(tempCounter);
                    if (!calculationFinished)
                    {
                        finalScore += tempCounter;
                    }
                    else
                    {
                        startBonusTimer = true;
                    }
                    int bonus = 0;
                    if (calculationFinished && bonusTimer <= 0)
                    {
                        endScoreTitleTXT.text = "SCORE";
                        bonus = 1 + (int)timer;
                        string bonusCoefficent = bonus.ToString() + "x";
                        endBonusTitleTXT.text = bonusCoefficent;
                        startFinalScoreCalc = true;
                        calculationFinished = false;
                    }
                    if (startFinalScoreTimer <= 0 && !skipFinalScoreCalc)
                    {
                        finalScore *= bonus;
                        startGradeTimer = true;
                        skipFinalScoreCalc = true;
                    }
                    if (GradeTimer <= 0)
                    {
                        ChooseGradeText();
                        loadNextScene = true;
                    }
                    endScoreTextTXT.text = finalScore.ToString();
                }
                scoreText.text = cc.GetScore().ToString();
            }
            else
            {
                if (sceneToLoad != 0)
                {
                    SceneManager.LoadScene(sceneToLoad);
                }
                waitForNextScene = true;
            }
        }
        else
        {

        }


    }

    void CreateListOfGrades()
    {
        grades.Add("NO");
        grades.Add("HORRIBLE");
        grades.Add("DISGUSTING");
        grades.Add("AWFUL");
        grades.Add("GHASTLY");
        grades.Add("DREADFUL");
        grades.Add("YES");
        grades.Add("OK");
        grades.Add("MEDIOCRE");
        grades.Add("HURRAH");
        grades.Add("AMAZING");
        grades.Add("FABULOUS");
        grades.Add("FANTASTIC");
        grades.Add("BEAUTIFUL");
        grades.Add("MARVELOUS");
        grades.Add("ASTONISHING");
    }

    void ChooseGradeText()
    {
        float good = 3500;
        float mediocre = 2000;
        float bad = 1000;

        string gradeToText = "";

        if (finalScore >= good)
        {
            gradeToText = GetGrade(3);
        }
        else if (finalScore < mediocre && finalScore >= bad)
        {
            gradeToText = GetGrade(2);
        }
        else
        {
            gradeToText = GetGrade(1);
        }

        endGradeTextTXT.text = gradeToText + "!";
    }

    void DetermineSceneToLoad()
    {

    }

    string GetGrade(int result)
    {
        string finalGrade = "";
        int rand = 0;
        // 3 = good
        // 2 = mediocre
        // 1 = bad

        switch (result)
        {
            case 3:
                rand = Random.Range(9, 15);
                break;
            case 2:
                rand = Random.Range(6, 8);
                break;
            case 1:
                rand = Random.Range(0, 5);
                break;
        }
        finalGrade = grades[rand];

        return finalGrade;
    }

    void SpawnCollectables()
    {
        for (int i = 0; i < collectablesAmount; i++)
        {
            GameObject go = (GameObject)Instantiate(Resources.Load("orb"));
            float tempX = Random.Range(-15, 15);
            float tempZ = Random.Range(-10, 10);
            go.transform.position = new Vector3(tempX, 0, tempZ);
            collectables.Add(go);
        }
    }

    void TimerBehavior()
    {
        if (startTimer)
        {
            if (timer > 0)
                timer -= 1.0f * Time.deltaTime;
            if (timer <= 0)
                timer = 0;
        }


        if (timer < 5.05f && timer > 4.95f)
            timerText.fontSize = 60;
        if (timer < 4.95f)
            timerText.fontSize = origFontSize;

        if (timerText != null)
            timerText.text = timer.ToString("F2");

        if (timer <= 0)
            endGame = true;
    }

    void CreateEndCanvas()
    {
        //Canvas for showing the end score
        GameObject canvasPF = Resources.Load<GameObject>("EndCanvas");
        endCanvasGO = GameObject.Instantiate(canvasPF) as GameObject;

        endScoreTextGO = GameObject.Find("EndScoreText");
        endScoreTextTXT = endScoreTextGO.GetComponent<Text>();
        endGradeTextGO = GameObject.Find("EndGradeText");
        endGradeTextTXT = endGradeTextGO.GetComponent<Text>();
        endScoreTitleGO = GameObject.Find("EndScoreTitle");
        endScoreTitleTXT = endScoreTitleGO.GetComponent<Text>();
        endBonusTitleGO = GameObject.Find("EndBonusText");
        endBonusTitleTXT = endBonusTitleGO.GetComponent<Text>();

        createCanvas = false;
    }

    public void MinusCollectable()
    {
        collectablesLeft--;
    }
}
