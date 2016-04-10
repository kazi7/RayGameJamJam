using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public PlayerController cc;

    int cumulativeScore;
    int finalScore;
    bool loadNextScene;

    //TIMERS - GAME START
    bool startGame;
    bool startGameTimerStart;
    float startGameTimer;

    //TIMERS - IN GAME
    bool startTimer;
    float timer;
    bool startBonusTimer;
    float bonusTimer;
    bool startFinalScoreCalc;
    float startFinalScoreTimer;
    bool startGradeTimer;
    float GradeTimer;
    bool skipFinalScoreCalc;

    //GAME UI
    GameObject canvasTimerText;
    Text timerText;
    GameObject canvasScoreText;
    Text scoreText;
    GameObject CollectA;
    Image collectImageA;
    GameObject CollectB;
    Image collectImageB;
    GameObject DestroyA;
    Image destroyImageA;
    GameObject DestroyB;
    Image destroyImageB;
    GameObject AvoidA;
    Image avoidImageA;
    GameObject AvoidB;
    Image avoidImageB;
    //GAME UI TIMERS
    int flickerTimer;

    //COLLECTABLES
    bool spawnCollectables;
    public List<GameObject> collectables;
    public int collectablesAmount = 10;
    int collectablesLeft;

    //GAME END
    public int origFontSize;
    private bool endGame;
    bool waitForNextScene;
    public List<string> grades;

    //CANVAS FOR SHOWING THE FINAL LEVEL SCORE
    private bool createCanvas;
    private GameObject endCanvasGO;
    private GameObject finishTitleGO;
    private Text finishTitleTXT;
    private GameObject endScoreTextGO;
    private Text endScoreTextTXT;
    private GameObject endGradeTextGO;
    private Text endGradeTextTXT;
    private GameObject endScoreTitleGO;
    private Text endScoreTitleTXT;
    private GameObject endBonusTitleGO;
    private Text endBonusTitleTXT;

    //SCENE DATA
    private int LastLoadedScene;
    private float sceneLoadTimer;
    private bool startSceneLoadTimer;
    private int sceneToLoad;

    //SCENE 3
    bool wasDestroyed;
    GameObject block;
    BlockController bc;
    bool playerShouldInst;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        CreateListOfGrades();
        playerShouldInst = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region Scene2
        if (LastLoadedScene == 2)
        {
            if (startGameTimerStart)
            {
                if (startGameTimer > 0)
                {
                    flickerTimer++;
                    startGameTimer -= 1.0f * Time.deltaTime;                    
                    
                    int randomizer = Random.Range(1, 10);

                    switch(LastLoadedScene)
                    {
                        case 2:
                            {
                                if (randomizer >= 5)
                                {
                                    if (collectImageB.enabled == true)
                                        collectImageB.enabled = false;
                                    else
                                        collectImageB.enabled = true;
                                }
                            }
                            break;
                        case 3:
                            {
                                if (randomizer >= 5)
                                {
                                    if (destroyImageB.enabled == true)
                                        destroyImageB.enabled = false;
                                    else
                                        destroyImageB.enabled = true;
                                }
                            }
                            break;
                        case 4:
                            {
                                if (randomizer >= 5)
                                {
                                    if (avoidImageB.enabled == true)
                                        avoidImageB.enabled = false;
                                    else
                                        avoidImageB.enabled = true;
                                }
                            }
                            break;
                    }
                }
                if (startGameTimer <= 0)
                {
                    spawnCollectables = true;
                    startGameTimerStart = false;
                }
            }

            if (startGameTimer <= 0)
            {
                HideIntro();

                if (!waitForNextScene)
                {
                    if (!loadNextScene)
                    {
                        if (!endGame)
                        {
                            TimerBehavior();

                            if (spawnCollectables)
                            {
                                SpawnCollectables();
                                startTimer = true;
                                spawnCollectables = false;
                            }

                            if (collectablesLeft == 0 && startTimer)
                            {
                                GameOver();
                            }
                        }
                        if (endGame)
                        {
                            //END GAME

                            int tempCounter = 5;
                            cc.EndGame();

                            if (createCanvas && !startTimer)
                                CreateEndCanvas();
                            if (!wasDestroyed)
                                finishTitleTXT.text = "END";

                            CalculateTimers();

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
                                endScoreTextTXT.fontSize = 40;
                                startGradeTimer = true;
                                skipFinalScoreCalc = true;
                            }
                            if (GradeTimer <= 0)
                            {
                                ChooseGradeText();
                                loadNextScene = true;
                                startSceneLoadTimer = true;
                            }
                            endScoreTextTXT.text = finalScore.ToString();
                        }
                        if (Player != null)
                            scoreText.text = cc.GetScore().ToString();
                    }
                    else
                    {
                        LoadNextScene();
                    }
                }
                else
                {
                    if (collectablesLeft <= 0)
                        sceneToLoad = LastLoadedScene + 1;
                    else
                        sceneToLoad = 1;

                    endGradeTextTXT.fontSize += 2;
                    if (sceneLoadTimer > 0)
                        sceneLoadTimer -= 1.0f * Time.deltaTime;
                    if (sceneToLoad != 0 && sceneLoadTimer <= 0)
                    {
                        SceneManager.LoadScene(sceneToLoad);
                    }
                }
            }
        }
        #endregion

        #region Scene3
        ///
        ////// SCENE 3
        ///

        if (LastLoadedScene == 3)
        {
            if(playerShouldInst)
            {
                cc.InstantiateBlock();
                playerShouldInst = false;
            }

            if (startGameTimerStart)
            {
                if (startGameTimer > 0)
                {
                    flickerTimer++;
                    startGameTimer -= 1.0f * Time.deltaTime;

                    int randomizer = Random.Range(1, 10);

                    if (randomizer >= 5)
                    {
                        if (destroyImageB.enabled == true)
                            destroyImageB.enabled = false;
                        else
                            destroyImageB.enabled = true;
                    }
                }
                if (startGameTimer <= 0)
                {
                    startGameTimerStart = false;
                }
            }

            if (startGameTimer <= 0)
            {
                HideIntro();

                if (!waitForNextScene)
                {
                    if (!loadNextScene)
                    {
                        if (!endGame)
                        {
                            TimerBehavior();

                            startTimer = true;

                            if (cc.IsBlockDestroyed())
                            {
                                GameOver();
                            }
                        }
                        if (endGame)
                        {
                            //END GAME

                            int tempCounter = 5;
                            cc.EndGame();

                            if (createCanvas && !startTimer)
                                CreateEndCanvas();
                            if (!cc.IsBlockDestroyed())
                                finishTitleTXT.text = "END";

                            CalculateTimers();

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
                                endScoreTextTXT.fontSize = 40;
                                startGradeTimer = true;
                                skipFinalScoreCalc = true;
                            }
                            if (GradeTimer <= 0)
                            {
                                ChooseGradeText();
                                loadNextScene = true;
                                startSceneLoadTimer = true;
                            }
                            endScoreTextTXT.text = finalScore.ToString();
                        }
                        if (Player != null)
                            scoreText.text = cc.GetScore().ToString();
                    }
                    else
                    {
                        LoadNextScene();
                    }
                }
                else
                {
                    if (cc.IsBlockDestroyed())
                        sceneToLoad = LastLoadedScene + 1;
                    else
                        sceneToLoad = 1;

                    endGradeTextTXT.fontSize += 2;
                    if (sceneLoadTimer > 0)
                        sceneLoadTimer -= 1.0f * Time.deltaTime;
                    if (sceneToLoad != 0 && sceneLoadTimer <= 0)
                    {
                        SceneManager.LoadScene(sceneToLoad);
                    }
                }
            }
        }
        #endregion

        #region Scene4
        ///
        ////// SCENE 4
        ///

        if (LastLoadedScene == 3)
        {

        }
        #endregion
    }

    void OnLevelWasLoaded(int level)
    {
        print("LOADED LEVEL: " + level);

        LastLoadedScene = level;

        if (level >= 2)
        {
            SceneStart();
            startGameTimerStart = true;

            AudioSource audio;
            switch (level)
            {
                case 2:
                    {
                        collectImageA.enabled = true;
                        collectImageB.enabled = false;
                        audio = CollectA.GetComponent<AudioSource>();
                        audio.Play();
                    }
                    break;
                case 3:
                    {
                        destroyImageA.enabled = true;
                        destroyImageB.enabled = false;
                        audio = destroyImageA.GetComponent<AudioSource>();
                        audio.Play();
                        playerShouldInst = true;
                    }
                    break;
                case 4:
                    {
                        avoidImageA.enabled = true;
                        avoidImageB.enabled = false;
                        audio = avoidImageA.GetComponent<AudioSource>();
                        audio.Play();
                    }
                    break;
            }
        }

        LastLoadedScene = level;
    }

    void SceneStart()
    {
        CreateGameUI();

        Player = GameObject.Find("Player");
        if (Player != null)
            cc = Player.GetComponent<PlayerController>();

        //Initialize bools
        createCanvas = true;
        endGame = false;
        startTimer = false;
        startBonusTimer = false;
        loadNextScene = false;
        startFinalScoreCalc = false;
        startGradeTimer = false;
        skipFinalScoreCalc = false;
        waitForNextScene = false;
        startSceneLoadTimer = false;
        startGameTimerStart = false;
        spawnCollectables = false;

        //Initialize values
        timer = 10.0f;
        origFontSize = 40;
        finalScore = 0;
        float timersValues = 1.0f;
        bonusTimer = timersValues;
        startFinalScoreTimer = timersValues;
        GradeTimer = timersValues;
        sceneLoadTimer = timersValues;
        startGameTimer = 3.7f;
        sceneToLoad = 1;
        flickerTimer = 0;

        collectablesLeft = collectablesAmount;
    }

    void CreateGameUI()
    {
        //Canvas for showing the end score
        GameObject gameUI = Resources.Load<GameObject>("GameUI");
        endCanvasGO = GameObject.Instantiate(gameUI) as GameObject;

        canvasTimerText = GameObject.Find("TimerText");
        timerText = canvasTimerText.GetComponent<Text>();
        canvasScoreText = GameObject.Find("ScoreText");
        scoreText = canvasScoreText.GetComponent<Text>();

        CollectA = GameObject.Find("CollectA");
        collectImageA = CollectA.GetComponent<Image>();
        CollectB = GameObject.Find("CollectB");
        collectImageB = CollectB.GetComponent<Image>();
        DestroyA = GameObject.Find("DestroyA");
        destroyImageA = DestroyA.GetComponent<Image>();
        DestroyB = GameObject.Find("DestroyB");
        destroyImageB = DestroyB.GetComponent<Image>();
        AvoidA = GameObject.Find("AvoidA");
        avoidImageA = AvoidA.GetComponent<Image>();
        AvoidB = GameObject.Find("AvoidB");
        avoidImageB = AvoidB.GetComponent<Image>();
    }

    void CalculateTimers()
    {
        if (startBonusTimer && bonusTimer > 0)
            bonusTimer -= 1.0f * Time.deltaTime;

        if (startFinalScoreCalc && startFinalScoreTimer > 0)
            startFinalScoreTimer -= 1.0f * Time.deltaTime;

        if (startGradeTimer && GradeTimer > 0)
            GradeTimer -= 1.0f * Time.deltaTime;
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

    void LoadNextScene()
    {
        cumulativeScore += finalScore;
        waitForNextScene = true;
    }

    void GameOver()
    {
        print("GAME OVER GAME OVER");
        startTimer = false;
        endGame = true;
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
            int tempX = Random.Range(-14, 14);
            int tempZ = Random.Range(-14, 14);
            go.transform.position = new Vector3(tempX, 1, tempZ);
            collectables.Add(go);
        }
    }

    void TimerBehavior()
    {
        bool roundUp = false;

        if (startTimer)
        {
            if (timer > 0)
                timer -= 1.0f * Time.deltaTime;
            if (timer <= 0)
                timer = 0;
        }
        if (timer < 3.10f && timer > 2.95f)
        {
            timerText.fontSize = 60;
            roundUp = true;
        }
        if (timer < 2.95f && timer > 2.10f)
        {
            timerText.fontSize = origFontSize;
        }
        if (timer < 2.10f && timer > 1.95f)
        {
            timerText.fontSize = 60;
            roundUp = true;
        }
        if (timer < 1.95f && timer > 1.10f)
        {
            timerText.fontSize = origFontSize;
        }
        if (timer < 1.10f && timer > 0.95f)
        {
            timerText.fontSize = 60;
            roundUp = true;
        }
        if (timer < 0.95f && timer > 0)
        {
            timerText.fontSize = origFontSize;
        }

        if (roundUp)
        {
            if (timerText != null)
                timerText.text = timer.ToString("F0");
        }
        else
        {
            if (timerText != null)
                timerText.text = timer.ToString("F2");
        }

        if (timer <= 0)
            GameOver();
    }

    void CreateEndCanvas()
    {
        //Canvas for showing the end score
        GameObject canvasPF = Resources.Load<GameObject>("EndCanvas");
        endCanvasGO = GameObject.Instantiate(canvasPF) as GameObject;

        finishTitleGO = GameObject.Find("FinishTitle");
        finishTitleTXT = finishTitleGO.GetComponent<Text>();
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

    void HideIntro()
    {
        collectImageA.enabled = false;
        collectImageB.enabled = false;
        destroyImageA.enabled = false;
        destroyImageB.enabled = false;
    }

    public void MinusCollectable()
    {
        collectablesLeft--;
    }
}
