using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCam : MonoBehaviour {

    //The player to which the camera is bound
    GameObject pl;

    //Let the player move without movining the camera
    public float maxOfsetX;
    public float maxOfsetY;

    //The calculated next coordinates of the camera
    float nextX;
    float nextY;

    //Select if the camera is bound between coordinates
    public bool xBound;
    public bool yBound;

    //The values inbetween which the camera is bound
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

	// Use this for initialization
	void Start () {
        pl = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {


        //Make the camera follow the player with the given ofset
        if (pl.transform.position.x > transform.position.x + maxOfsetX)
        {
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

        //Keep the x position inbetween the given coordinates when on
        if (xBound)
        {
            if (nextX < minX)
            {
                nextX = minX;
            }
            if (nextX > maxX)
            {
                nextX = maxX;
            }
        }
        //Keep the y position inbetween the given coordinates when on
        if (yBound)
        {
            if (nextY < minY)
            {
                nextY = minY;
            }
            if (nextY > maxY)
            {
                nextY = maxY;
            }
        }

        //Apply the calculated next coordinates
        transform.position = new Vector3(nextX, nextY, -10);
    }
}
