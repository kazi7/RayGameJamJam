using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    public float moveSpeed = 5.0f;
    public float jumpStrength = 10.0f;
    public float interactSpeed = 0.2f;


    private Quaternion targetRotation;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	
	}
	
	// Update is called once per frame
	void Update () {
        Controls();


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
}
