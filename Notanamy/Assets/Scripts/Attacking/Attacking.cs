using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour {

    //Variables

    //player componets
    Rigidbody rb;
    Collider col;
    Player player;

    //Prefabs
    public GameObject attackHitbox;
    GameObject hitboxClone;
    Hitbox hb;

    //Mouse variables
    Vector3 mouseP;
    float anglePlayerMouse;

    //Attack type 2
    public bool attackV2On;
    public int comboCounter;
    int comboState;
    int comboTimer;
    //False = left | True = right
    bool attackSide;
    public int detectionAngle;
    /* Attack direction
     * 1 = TopRight
     * 2 = MiddleRight
     * 3 = DownRight
     * 4 = TopLeft
     * 5 = Middleleft
     * 6 = DownLeft */
    int attackDirection;
    bool usingAttack = false;
    int attackTimer;
    bool downAttack = false;
    int downTimer;



    //Functions
    /* AngleBetweenPoints(pointA, pointB)
     * Angle between points uses right-angled triangle geomatry to calculate the angle between point A and point B
     * Vector3 Point A
     * Vector3 Point B
     */
    public float AngleBetweenPoints(Vector3 pointA, Vector3 pointB)
    {
        float Ad = pointA.x - pointB.x;
        float Op = pointA.y - pointB.y;
        float angle = 0;
        angle = Mathf.Atan(Op / Ad);
        return angle;
    }

    public GameObject spawnAttack(Vector3 posOffset, int atkTimer, bool spin = false)
    {
        GameObject hitbox = Instantiate(attackHitbox);
        hitbox.transform.position = gameObject.transform.position + posOffset;
        hitbox.transform.parent = gameObject.transform;
        Hitbox boxScript = hitbox.GetComponent<Hitbox>();
        boxScript.attackTimer = atkTimer;
        boxScript.isScytheSpin = spin;
        return hitbox;
    }

    // Use this for initialization
    void Start () {
        player = gameObject.GetComponent<Player>();
        rb = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update () {

        mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseP = new Vector3(mouseP.x, mouseP.y, 0);

        

        //Predetermind attack combos 
        if (attackV2On)
        {
            if (attackTimer > 0)
            {
                attackTimer--;
            }
            if (attackTimer <= 0)
            {
                usingAttack = false;
                player.allowMovement = true;
            }

            if (!usingAttack)
            {
                if (comboTimer > 0)
                {
                    comboTimer--;
                }
                if (comboTimer <= 0)
                {
                    comboCounter = 0;
                }

                if (downTimer >= 0)
                {
                    downTimer--;
                }
                else
                {
                    downAttack = false;
                }
            }
            if (usingAttack)
            {
                player.allowMovement = false;
            }
            

            //An attack will be triggered on mouse
            if ((Input.GetMouseButtonDown(0)) && (!usingAttack))
            {
                //Calculate attack direction based on player and mouse position
                {
                    //Calculate the angle between player and mouse
                    anglePlayerMouse = AngleBetweenPoints(gameObject.transform.position, mouseP);

                    //Attack Side default right
                    attackSide = true;

                    //Detect if clicking on the left
                    if (gameObject.transform.position.x > mouseP.x)
                    {
                        //Invert angle when clicking left
                        anglePlayerMouse = -anglePlayerMouse;
                        //When clicking left attack side is left
                        attackSide = false;
                    }

                    //Change radian to degrees
                    anglePlayerMouse = Mathf.Rad2Deg * anglePlayerMouse;

                    //If you attack to the right
                    if (attackSide)
                    {
                        //Above detectionAngle degrees you attack up
                        if (anglePlayerMouse > detectionAngle)
                        {
                            //Attack up right
                            attackDirection = 1;
                        }
                        //Below -detectionAngle degees you attack down
                        else if (anglePlayerMouse < -detectionAngle)
                        {
                            //Attack down right
                            attackDirection = 3;
                        }
                        //Between -detectionAngle and detectionAngle degrees you attack middle
                        else
                        {
                            //Attack middle right
                            attackDirection = 2;
                        }
                    }
                    //If you attack to the left
                    else
                    {
                        //Above detectionAngle degrees you attack up
                        if (anglePlayerMouse > detectionAngle)
                        {
                            attackDirection = 4;
                        }
                        //Below -detectionAngle degees you attack down
                        else if (anglePlayerMouse < -detectionAngle)
                        {
                            attackDirection = 6;
                        }
                        //Between -detectionAngle and detectionAngle degrees you attack middle
                        else
                        {
                            attackDirection = 5;
                        }
                    }
                }

                //Use attack based on calculated attack direction
                {
                    if (player.IsGrounded())
                    {
                        if (!downAttack)
                        {
                            switch (attackDirection)
                            {
                                case 1:
                                    {
                                        attackTimer = 20;
                                        hitboxClone = spawnAttack(new Vector3(1, -0.3f, 0), attackTimer);
                                        hb = hitboxClone.GetComponent<Hitbox>();
                                        hb.duration = attackTimer / 2;
                                        hb.movement = (0.7f / hb.duration) * Vector3.up;
                                        hb.knockback = new Vector3(100, 500, 0);
                                        rb.AddForce(new Vector3(200, 600, 0));
                                        usingAttack = true;
                                        comboCounter = 0;
                                        comboTimer = 15;
                                    }
                                    break;
                                case 2:
                                    {
                                        switch (comboCounter)
                                        {
                                            case 0:
                                                attackTimer = 10;
                                                hitboxClone = spawnAttack(new Vector3(1, 0, 0), attackTimer);
                                                hb = hitboxClone.GetComponent<Hitbox>();
                                                hb.knockback = new Vector3(50, 0, 0);

                                                rb.AddForce(new Vector3(200, 0, 0));
                                                usingAttack = true;
                                                comboCounter += 1;
                                                comboTimer = 15;
                                                break;
                                            case 1:
                                                attackTimer = 10;
                                                spawnAttack(new Vector3(1, 0, 0), attackTimer);
                                                rb.AddForce(new Vector3(200, 0, 0));
                                                usingAttack = true;
                                                comboCounter = 2;
                                                comboTimer = 15;
                                                break;
                                            case 2:
                                                attackTimer = 40;
                                                spawnAttack(new Vector3(1, 0, 0), attackTimer, true);
                                                usingAttack = true;
                                                comboCounter = 3;
                                                comboTimer = 15;
                                                break;
                                            case 3:
                                                attackTimer = 20;
                                                spawnAttack(new Vector3(1, 0, 0), attackTimer);
                                                rb.AddForce(new Vector3(400, 0, 0));
                                                usingAttack = true;
                                                comboCounter = 0;
                                                comboTimer = 0;
                                                break;
                                        }
                                    }
                                    break;
                                case 3:
                                    {
                                        attackTimer = 10;
                                        hitboxClone = spawnAttack(new Vector3(0, 1, 0), attackTimer);
                                        hb = hitboxClone.GetComponent<Hitbox>();
                                        hb.overHead = true;
                                        hb.duration = attackTimer / 2;
                                        hb.turnSpeed = 10;
                                        usingAttack = true;
                                        comboCounter = 0;
                                        comboTimer = 15;
                                        downAttack = true;
                                        downTimer = 15;
                                    }
                                    break;
                                case 4:
                                    {
                                        attackTimer = 20;
                                        hitboxClone = spawnAttack(new Vector3(-1, -0.3f, 0), attackTimer);
                                        hb = hitboxClone.GetComponent<Hitbox>();
                                        hb.duration = attackTimer / 2;
                                        hb.movement = (0.7f / hb.duration) * Vector3.up;
                                        hb.knockback = new Vector3(-100, 500, 0);
                                        rb.AddForce(new Vector3(-200, 600, 0));
                                        usingAttack = true;
                                        comboCounter = 0;
                                        comboTimer = 15;
                                    }
                                    break;
                                case 5:
                                    {
                                        switch (comboCounter)
                                        {
                                            case 0:
                                                attackTimer = 10;
                                                spawnAttack(new Vector3(-1, 0, 0), attackTimer);
                                                rb.AddForce(new Vector3(-200, 0, 0));
                                                usingAttack = true;
                                                comboCounter += 1;
                                                comboTimer = 25;
                                                break;
                                            case 1:
                                                attackTimer = 10;
                                                spawnAttack(new Vector3(-1, 0, 0), attackTimer);
                                                rb.AddForce(new Vector3(-200, 0, 0));
                                                usingAttack = true;
                                                comboCounter = 2;
                                                comboTimer = 25;
                                                break;
                                            case 2:
                                                attackTimer = 40;
                                                spawnAttack(new Vector3(-1, 0, 0), attackTimer, true);
                                                usingAttack = true;
                                                comboCounter = 3;
                                                comboTimer = 25;
                                                break;
                                            case 3:
                                                attackTimer = 20;
                                                spawnAttack(new Vector3(-1, 0, 0), attackTimer);
                                                rb.AddForce(new Vector3(-400, 0, 0));
                                                usingAttack = true;
                                                comboCounter = 0;
                                                comboTimer = 0;
                                                break;
                                        }
                                    }
                                    break;
                                case 6:
                                    {
                                        attackTimer = 10;
                                        hitboxClone = spawnAttack(new Vector3(0, 1, 0), attackTimer);
                                        hb = hitboxClone.GetComponent<Hitbox>();
                                        hb.overHead = true;
                                        hb.duration = attackTimer / 2;
                                        hb.turnSpeed = -10;
                                        usingAttack = true;
                                        comboCounter = 0;
                                        comboTimer = 15;
                                        downAttack = true;
                                        downTimer = 15;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            switch (attackDirection)
                            {
                                case 1:
                                    break;
                                case 2:
                                    break;
                                case 3:
                                    break;
                                case 4:
                                    break;
                                case 5:
                                    break;
                                case 6:
                                    break;
                            }
                        }
                    }
                }

            }
        }
    }
}
