using UnityEngine;
using System.Collections;

public class BALLOONMOVE : MonoBehaviour
{

    float moveSpeed;
    bool changeDir;
    float dirX, dirZ;

    float timer;

    // Use this for initialization
    void Start()
    {
        timer = 0;

        moveSpeed = Random.Range(6, 16);
        changeDir = false;
        int rand = Random.Range(1, 10);

        if (rand >= 5)
        {
            dirX = -1;
            dirZ = -1;
        }
        if (rand < 5)
        {
            dirX = 1;
            dirZ = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += 1.0f * Time.deltaTime;

        if (changeDir)
        {
            if (dirX == -1 && dirZ == -1)
            {
                dirX = 1;
                dirZ = 1;
                changeDir = false;
            }
            else if (dirX == 1 && dirZ == 1)
            {
                dirX = -1;
                dirZ = -1;
                changeDir = false;
            }
        }
        if (timer >= 1.0f)
        {
            Vector3 moveDirection = new Vector3(dirX, 0, dirZ);

            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        
    }

    void OnCollisionEnter(Collision coll)
    {
        print("COLLIDED");
        if (coll.transform.tag == "wall")
            changeDir = true;
        if (coll.transform.tag == "balloon")
            timer = 0;
    }
}
