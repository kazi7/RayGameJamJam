using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayGame()
    {
        SceneManager.LoadScene("level1");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
