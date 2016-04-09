using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public CharacterController cc;

    private int finalScore;

    bool startTimer;
    public float timer;

    public GameObject canvasTimerText;
    public Text timerText;
    public GameObject canvasScoreText;
    public Text scoreText;

    public int origFontSize;

    private bool endGame;

    public List<GameObject> collectables;
    public int collectablesAmount;

    private int collectablesLeft;


    //CANVAS FOR SHOWING THE FINAL LEVEL SCORE
    private GameObject endCanvasGO;
    private RectTransform endCanvasRT;
    private Canvas endCanvasCV;
    private GameObject scoreTextGO;
    private RectTransform scoreTextRT;
    private Text scoreTextTXT;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

	// Use this for initialization
	void Start ()
    {
        Player = GameObject.Find("Player");
        cc = Player.GetComponent<CharacterController>();

        canvasTimerText = GameObject.Find("TimerText");
        timerText = canvasTimerText.GetComponent<Text>();
        canvasScoreText = GameObject.Find("ScoreText");
        scoreText = canvasScoreText.GetComponent<Text>();

        endGame = false;
        startTimer = false;
        timer = 10.0f;
        origFontSize = 40;
        collectablesAmount = 10;
        collectablesLeft = collectablesAmount;

	}
	
	// Update is called once per frame
	void Update ()
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

            scoreText.text = cc.GetScore().ToString();
        }      
        if (endGame)
        {
            cc.EndGame();
            finalScore = cc.GetScore();


            //Canvas for showing the end score
            endCanvasGO = new GameObject();
            endCanvasGO.name = "EndCanvas";
            endCanvasRT = endCanvasGO.AddComponent<RectTransform>();
            endCanvasCV = endCanvasGO.AddComponent<Canvas>();
            endCanvasCV.renderMode = RenderMode.ScreenSpaceCamera;
            Vector3 pos = Camera.main.transform.position;
            pos += Camera.main.transform.forward * 10.0f;
            endCanvasCV.worldCamera = Camera.main;

            scoreTextGO = new GameObject();
            scoreTextGO.name = "ScoreText";
            scoreTextRT = scoreTextGO.AddComponent<RectTransform>();
            scoreTextGO.transform.SetParent(endCanvasGO.transform);
            scoreTextTXT = scoreTextGO.AddComponent<Text>();
            scoreTextTXT.text = "TOIMII";




            //do stuff on endgame
        }
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

    public void MinusCollectable()
    {
        collectablesLeft--;
    }
}
