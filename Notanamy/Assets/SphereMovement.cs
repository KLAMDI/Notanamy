using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : MonoBehaviour {

    public bool allowMovement;
    public float speed;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        //rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        //rb.AddTorque(0, speed, 0, ForceMode.Force);
	}
}
