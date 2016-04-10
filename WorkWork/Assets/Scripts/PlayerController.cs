using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private GameObject gm;
    GameObject player;
    Transform playerTF;

    public float moveSpeed = 10.0f;
    public float jumpStrength = 10.0f;
    public float attackTimerMax = 0.2f;
    public float attackTimer;

    private bool canJump;

    private Quaternion targetRotation;
    private Rigidbody rb;
    private int score;

    private bool endGame;

    //ROTATION OF PLAYER
    Vector3 prevLoc;
    Vector3 curLoc;
    float lookSpeed = 100000;

    //ATTACKING
    public float punchingForce;
    float timeSinceLastPunch;
    bool canHit;

    GameObject block;
    int blockHealth;
    bool instantiateB;
    bool changeBlock;
    bool destroyBlock;
    bool isBlockDestroyed;

    //ANIMATIONS
    Animator anim;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        playerTF = player.GetComponent<Transform>();

        rb = player.GetComponent<Rigidbody>();
        gm = GameObject.Find("Manager");

        endGame = false;
        canJump = true;
        attackTimer = attackTimerMax;
        instantiateB = false;
        changeBlock = true;
        destroyBlock = true;
        isBlockDestroyed = false;

        anim = GetComponent<Animator>();
        punchingForce = 0;
        timeSinceLastPunch = 0;

        blockHealth = 200;
    }

    // Update is called once per frame
    void Update()
    {
        if (instantiateB)
        {
            block = (GameObject)Instantiate(Resources.Load("cube"));
            block.transform.position = new Vector3(0, 4, 0);

            instantiateB = false;
        }

        timeSinceLastPunch += 1.0f * Time.deltaTime;

        if (punchingForce > 0)
            punchingForce -= 1.0f * Time.deltaTime;
        if (punchingForce <= 0)
            punchingForce = 0;

        if (!endGame)
        {
            Controls();

            if (attackTimer > 0)
                attackTimer -= 1.0f * Time.deltaTime;
        }
        if (punchingForce > 0)
            anim.SetBool("punching", true);
        else
            anim.SetBool("punching", false);

        if (timeSinceLastPunch > 1.0f)
            punchingForce = 0;

        if (blockHealth < 100)
        {
            if (changeBlock)
            {
                Destroy(block);
                block = (GameObject)Instantiate(Resources.Load("cubeFrac"));
                block.transform.position = new Vector3(0, 3, -1);
                changeBlock = false;
            }
        }
        if (blockHealth <= 0)
        {
            if (destroyBlock)
            {
                Destroy(block);
                block = (GameObject)Instantiate(Resources.Load("cubeDest"));
                block.transform.position = new Vector3(0, 3, -1);
                destroyBlock = false;
                isBlockDestroyed = true;
            }
        }
        print(blockHealth);
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
            case "ground":
                {
                    canJump = true;
                    anim.SetBool("jumping", false);
                }
                break;
        }
    }

    void Controls()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, +Input.GetAxis("Vertical"));

        if (moveDirection.x == 0 && moveDirection.z == 0)
            anim.SetBool("moving", false);
        else
            anim.SetBool("moving", true);

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        Vector3 movTest = moveDirection * moveSpeed * Time.deltaTime;

        if (moveDirection.x != 0 || moveDirection.z != 0)
        playerTF.rotation = Quaternion.LookRotation(moveDirection);

        if (rb.velocity.y >= 0.1)
            anim.SetBool("jumping", true);
        else
            anim.SetBool("jumping", false);

        if (canJump)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                rb.AddForce(Vector3.up * jumpStrength);
                canJump = false;
                print("JUMPING");
            }
        }
        if (attackTimer <= 0)
        {
            if (Input.GetButton("Jump"))
            {
                attackTimer = attackTimerMax;
                punchingForce += 0.5f;
                timeSinceLastPunch = 0.0f;
                
                if (canHit)
                {
                    blockHealth -= 10;
                }
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            attackTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(1);
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

    public bool CalculateScore(int amt)
    {
        bool stopAdjusting = false;

        if (score >= amt)
        {
            score -= amt;
            stopAdjusting = false;
        }
        if (score <= 0)
        {
            score = 0;
            stopAdjusting = true;
        }
        return stopAdjusting;
    }

    public void CanHit(bool canOrNot)
    {
        canHit = canOrNot;
    }

    public void InstantiateBlock()
    {
        print("YEAHEAHEHSEAHSEAHSEASEHAASEH");
        instantiateB = true;
    }

    public bool IsBlockDestroyed()
    {
        return isBlockDestroyed;
    }
}
