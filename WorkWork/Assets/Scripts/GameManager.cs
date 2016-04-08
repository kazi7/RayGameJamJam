using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public CharacterController pc;

    private int finalScore;

    bool startTimer;
    public float timer;

    public GameObject canvasText;
    public Text scoreText;

    public int origFontSize;

    private bool endGame;

    public List<GameObject> collectables;
    public int collectablesAmount;


    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

	// Use this for initialization
	void Start ()
    {
        Player = GameObject.Find("Player");
        pc = Player.GetComponent<CharacterController>();

        canvasText = GameObject.Find("ScoreText");
        scoreText = canvasText.GetComponent<Text>();

        endGame = false;
        startTimer = false;
        timer = 10.0f;
        origFontSize = 40;
        collectablesAmount = 10;

	}
	
	// Update is called once per frame
	void Update ()
    {
        TimerBehavior();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawnCollectables();
            startTimer = true;
        }

        if (endGame)
        {
            finalScore = Player.GetComponent<CharacterController>().GetScore();
            print("FINAL SCORE IS " + finalScore);
            //do stuff on endgame
        }

	}

    void SpawnCollectables()
    {
        for (int i = 0; i < collectablesAmount; i++)
        {
            GameObject collectable = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            collectable.AddComponent<SphereCollider>();
            float tempX = Random.Range(-15, 15);
            float tempZ = Random.Range(-15, 15);
            collectable.transform.position = new Vector3(tempX, 0, tempZ);
            collectables.Add(collectable);
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
            scoreText.fontSize = 60;
        if (timer < 4.95f)
            scoreText.fontSize = origFontSize;

        if (scoreText != null)
            scoreText.text = timer.ToString("F2");

        if (timer <= 0)
            endGame = true;
    }
}
