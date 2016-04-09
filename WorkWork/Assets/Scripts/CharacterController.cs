using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    private GameObject gm;

    public float moveSpeed = 5.0f;
    public float jumpStrength = 10.0f;
    public float interactSpeed = 0.2f;


    private Quaternion targetRotation;
    private Rigidbody rb;

    private int score;

    private bool endGame;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.Find("Manager");
        endGame = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!endGame)
        {
            Controls();



        }
	}

    void OnCollisionEnter(Collision coll)
    {
        switch (coll.transform.tag)
        {
            case "goods":
                {
                    Destroy(coll.gameObject);
                    score += 150;
                    gm.GetComponent<GameManager>().MinusCollectable();
                }
                break;
        }

    }

    void Controls()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, +Input.GetAxis("Vertical"));
        Vector3 moveAngle = new Vector3(0, +0, 45);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpStrength);
           
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void EndGame()
    {
        endGame = true;
    }
}
