using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {


    bool startTimer;
    public float timer;

    public GameObject canvasText;
    public Text scoreText;

    public int origFontSize;

	// Use this for initialization
	void Start () {
        canvasText = GameObject.Find("ScoreText");
        scoreText = canvasText.GetComponent<Text>();

        startTimer = false;
        timer = 10.0f;
        origFontSize = 40;
	}
	
	// Update is called once per frame
	void Update () {
        TimerBehaviour();


	}

    void TimerBehaviour()
    {
        if (timer > 0)
            timer -= 1.0f * Time.deltaTime;
        if (timer <= 0)
            timer = 0;


        if (timer < 5.05f && timer > 4.95f)
            scoreText.fontSize = 60;
        if (timer < 4.95f)
            scoreText.fontSize = origFontSize;

        if (scoreText != null)
            scoreText.text = timer.ToString("F2");
    }
}
