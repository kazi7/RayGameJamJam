using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    public float loadTimer = 3.0f;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        loadTimer -= 1.0f * Time.deltaTime;

        if (loadTimer <= 0)
            SceneManager.LoadScene("MainMenu");
	}
}
