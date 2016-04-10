using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour {

    int health;
    bool isDead;

	// Use this for initialization
	void Start () {

        isDead = false;
        health = 200;
	}
	
	// Update is called once per frame
	void Update () {

        int half = (int)(health * 0.5f);
        if (health <= half)
        {

        }
        if (health <= 0)
        {
            isDead = true;
        }
	}

    public void GotHit()
    {
        health -= 10;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
