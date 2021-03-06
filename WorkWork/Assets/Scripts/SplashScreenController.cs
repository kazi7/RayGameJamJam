﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    float loadTimer = 5.0f;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        loadTimer -= 1.0f * Time.deltaTime;

        if (loadTimer <= 0)
            LoadMenu();

        if (Input.anyKey)
            LoadMenu();
	}

    void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
