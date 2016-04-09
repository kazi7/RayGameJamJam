using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    public GameObject player;
    public Transform target;

    public bool lookAtPlayer;

	// Use this for initialization
	void Start () {

        player = GameObject.Find("Player");
        target = player.transform;
        transform.Rotate(Vector3.forward, 0.500f, Space.World);
	}
	
	// Update is called once per frame
	void Update () {

        if (lookAtPlayer)
        {
            if (transform.rotation.x < 0.535f && transform.rotation.x > 0.495f)
            {
                transform.LookAt(target);
            }

        }
	}
}
