using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetRotate : MonoBehaviour {

    public float rotateSpeed;
    public GameObject player;

    private GameObject planet;
    private Movement movement;
    private float playerVelocity;

    // Use this for initialization
    void Start () {
        planet = GameObject.Find("Planet");
        movement = player.GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        playerVelocity = movement.xSpeed*rotateSpeed;
        player.transform.position = new Vector3(0, player.transform.position.y, player.transform.position.z);
        planet.transform.Rotate(0, 0, playerVelocity);

    }
}
