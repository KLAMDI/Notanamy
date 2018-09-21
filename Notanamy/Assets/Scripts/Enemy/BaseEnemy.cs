using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour {

    public float health;

    public float gravity;
    public float drag;
    public float airDrag;

    public bool invulnerable;

    Rigidbody rb;


    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
