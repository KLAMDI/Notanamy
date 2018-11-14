using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCam : MonoBehaviour {

    GameObject pl;

    public float maxOfsetX;
    public float maxOfsetY;

    float nextX;
    float nextY;


	// Use this for initialization
	void Start () {
        pl = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

        if (pl.transform.position.x > transform.position.x + maxOfsetX)
        {
            Debug.Log(pl.transform.position.x);
            Debug.Log(transform.position.x + maxOfsetX);
            Debug.Log(pl.transform.position.x - maxOfsetX);


            nextX = pl.transform.position.x - maxOfsetX;
        }
        if (pl.transform.position.x < transform.position.x - maxOfsetX)
        {
            nextX = pl.transform.position.x + maxOfsetX;
        }
        if (pl.transform.position.y > transform.position.y + maxOfsetY)
        {
            nextY = pl.transform.position.y - maxOfsetY;
        }
        if (pl.transform.position.y < transform.position.y - maxOfsetY)
        {
            //Debug.Log(pl.transform.position.y);
            //Debug.Log(transform.position.y - maxOfsetY);
            //Debug.Log(pl.transform.position.y + maxOfsetY);
            nextY = pl.transform.position.y + maxOfsetY;
        }





        if (nextX < 0)
        {
            nextX = 0;
        }
        if (nextY < 1)
        {
            nextY = 1;
        }
        transform.position = new Vector3(nextX, nextY, -10);
    }
}
