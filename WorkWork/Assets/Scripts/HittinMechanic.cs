using UnityEngine;
using System.Collections;

public class HittinMechanic : MonoBehaviour {

    GameObject player;
    PlayerController pc;

	// Use this for initialization
	void Start () {

        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.transform.tag == "block")
        {
            pc.CanHit(true);
        }
    }

    void OnTriggerExit(Collider coll)
    {

        if (coll.transform.tag == "block")
        {
            pc.CanHit(false);
        }
    }
}
