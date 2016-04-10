using UnityEngine;
using System.Collections;

public class BALLOONMOVE : MonoBehaviour {

    float moveSpeed;
    bool changeDir;
    float dirX, dirZ;

	// Use this for initialization
	void Start () {
        moveSpeed = Random.Range(6, 16);
        changeDir = false;
        dirX = 0.0f;
        dirZ = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        if(changeDir)
        {
            dirX = Random.Range(-1,1);
            dirZ = Random.Range(-1, 1);            
        }
        Vector3 moveDirection = new Vector3(dirX, 0, dirZ);

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        Vector3 movTest = moveDirection * moveSpeed * Time.deltaTime;
	}

    void OnCollisionEnter(Collision coll)
    {
        if (coll.transform.tag == "wall")
            changeDir = true;
    }
}
