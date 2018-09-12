using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    public int attackTimer;
    public bool isScytheSpin = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        attackTimer--;
        if(attackTimer <= 0)
        {
            Destroy(gameObject);
        }
	}
}
