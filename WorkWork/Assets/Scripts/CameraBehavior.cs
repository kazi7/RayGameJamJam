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
	}
	
	// Update is called once per frame
	void Update () {

        if (lookAtPlayer)
            transform.LookAt(target);
	}
}
