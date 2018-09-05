using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Mouse possition
    Vector3 mouseP;

    //Rigidbody
    private Rigidbody rb;

    //X movement
    public float speed;
    public float minSpeed;
    public float maxSpeed;

    //Y movement
    public float jumpSpeed;
    public float airJumpMultiplier;
    public int maxAirJumps;
    int airJumps;

<<<<<<< HEAD
    //drag
    public float drag;
    public bool airDragTest;



=======
>>>>>>> ed427b88180701feebfcfd8ca82ede78788b3b1c
    //gravity
    public float gravity;
    public float wallGravity;
    public float fallingMultiplier;
    public float lowJumpMultiplier;
    float normalGravity;

    //Dash
    public bool dashAbl1;

    public bool dashAbl2;
    public float maxDashLength;
    float dashLength;
    bool isDashing;

    public float dashTimer;
    float dashTimerInit;
    public float dashSpeed;
    int dashTapCount = 0;

    //Grappling Hook
    public bool GrHAbl;
    public float grThrowSpeed;
    public Rigidbody grapplingHook;

    //collision ditection
    private Collider col;
    private float distToGround;
    private float distToWall;

    //Attack
    float anglePlayerMouse;
    //Attack type 1
    public bool attackOn;
    public GameObject attack;
    public float attackRange;
    
    //Attack type 2
        public bool attackV2On;
        int comboCounter;
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
        bool usingAttackV2;


    // Use this for initialization
    void Start() {
        //Get the nessesarie components
        rb = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<Collider>();
        // get the distance to ground
        distToGround = col.bounds.extents.y;
        distToWall = col.bounds.extents.x;
        //Dashing
        dashTimerInit = dashTimer;
        dashLength = maxDashLength;
    }

    //Checks if the player in on the ground using 5 raycast to minimize the area not checked and returns a boolian
    bool IsGrounded() {
        bool OnGround1 = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        bool OnGround2 = Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 2 + 0.01f, transform.position.y, transform.position.z), -Vector3.up, distToGround + 0.1f);
        bool OnGround3 = Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 2 - 0.01f, transform.position.y, transform.position.z), -Vector3.up, distToGround + 0.1f);
        bool OnGround4 = Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 4, transform.position.y, transform.position.z), -Vector3.up, distToGround + 0.1f);
        bool OnGround5 = Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 4, transform.position.y, transform.position.z), -Vector3.up, distToGround + 0.1f);
        bool OnGround = (OnGround1 || OnGround2 || OnGround3 || OnGround4 || OnGround5);
        return OnGround;
    }

    //Checks if the player is on the left wall
    public bool OnLeftWall()
    {
        bool OnWall1 = Physics.Raycast(transform.position, Vector3.left, distToWall + 0.1f);
        bool OnWall = OnWall1;
        return OnWall;
    }

    //Checks if the player is on the right wall
    public bool OnRightWall()
    {
        bool OnWall1 = Physics.Raycast(transform.position, Vector3.right, distToWall + 0.1f);
        bool OnWall = OnWall1;
        return OnWall;
    }

    public float AngleBetweenPoints(Vector3 pointA, Vector3 pointB)
    {
        float Ad = pointA.x - pointB.x;
        float Op = pointA.y - pointB.y;
        float angle = 0;
        angle = Mathf.Atan(Op / Ad);
        return angle;
    }

    public Vector3 AngleToPosition(float angle, float Hy, float posZ = 0)
    {
        float Op;
        float Ad;
        Op = Mathf.Sin(angle) * Hy;
        Ad = Mathf.Cos(angle) * Hy;
        Vector3 pos = new Vector3(Ad, Op, posZ);
        return pos;
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse Position
        mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseP = new Vector3(mouseP.x, mouseP.y, 0);

        //physics
        {
            //Simulate gravity for player only 
            //Gravity on a wall is lower while moving down
            if ((OnLeftWall() || OnRightWall()) && (rb.velocity.y < 0))
            {
                rb.AddForce(0, -wallGravity, 0);
            }
            else
            {

                //Change gravity strength to make jumps less floaty
                if (rb.velocity.y < 0)
                {
                    rb.AddForce(0, -gravity * fallingMultiplier, 0);
                }
                //Holding up makes you jump higher
                else if (Input.GetKey(KeyCode.W))
                {
                    rb.AddForce(0, -gravity, 0);
                }
                //Higher gravity when not holding up
                else
                {
                    rb.AddForce(0, -gravity * lowJumpMultiplier, 0);
                }
            }

            //Limmit the speed a player can move at
            if (rb.velocity.x > maxSpeed)
            {
                rb.velocity = new Vector3(maxSpeed, rb.velocity.y, 0);
            }
            if (rb.velocity.x < -maxSpeed)
            {
                rb.velocity = new Vector3(-maxSpeed, rb.velocity.y, 0);
            }

            //If no or both directions are pressed slow down using drag
            if (!((Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.A))) || ((Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.A))))
            {
                //Minimal speed to avoid micromovements instead of stopping
                if (rb.velocity.x < minSpeed && rb.velocity.x > -minSpeed)
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                }

                //Apply drag when on ground or when airDrag is turned on
                if (IsGrounded() || airDragTest)
                {
                    if (rb.velocity.x >= minSpeed)
                    {
                        rb.AddForce(-drag, 0, 0);
                    }
                    if (rb.velocity.x <= -minSpeed)
                    {
                        rb.AddForce(drag, 0, 0);
                    }
                }
            }
        }

        //Reset doublejumps when on the ground
        if (IsGrounded())
        {
            airJumps = maxAirJumps;
        }

        //Controls
        {
            //Movement controls
            if (Input.GetKey(KeyCode.A) && !(Input.GetKey(KeyCode.D)))
            {
                rb.AddForce(-speed, 0, 0);
            }
            if (Input.GetKey(KeyCode.D) && !(Input.GetKey(KeyCode.A)))
            {
                rb.AddForce(speed, 0, 0);
            }

            //Press up to jump
            if (Input.GetKeyDown(KeyCode.W))
            {
                //Check if the player not on a wall in the air or if the player in on the ground
                if (!(OnLeftWall() || OnRightWall()) || IsGrounded()) {
                    //Jumping while on the ground is higher
                    if (IsGrounded())
                    {
                        rb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.VelocityChange);
                    }
                    //Check if any air jumps are remaining and airjumps are effected by airjump multiplier
                    else if (airJumps > 0)
                    {
                        rb.AddForce(new Vector3(0, jumpSpeed * airJumpMultiplier, 0) - new Vector3(0, rb.velocity.y, 0), ForceMode.VelocityChange);
                        airJumps--;
                    }
                }
                //If on a wall in the air do a walljump
                else
                {
                    //On Left wall jump right
                    if (OnLeftWall())
                    {
                        rb.AddForce(new Vector3(jumpSpeed, jumpSpeed, 0) - new Vector3(0, rb.velocity.y, 0), ForceMode.VelocityChange);
                    }
                    //On right wall jumo left
                    if (OnRightWall())
                    {
                        rb.AddForce(new Vector3(-jumpSpeed, jumpSpeed, 0) - new Vector3(0, rb.velocity.y, 0), ForceMode.VelocityChange);
                    }
                }
            }
            //While in the air press down to cancel jump and fall down faster
            if ((Input.GetKeyDown(KeyCode.S)) && !IsGrounded())
            {
                //Limmit the downwards speed achieved with downfall
                if (rb.velocity.y > (-3 * jumpSpeed))
                {
                    rb.velocity = new Vector3(rb.velocity.x, -3 * jumpSpeed, 0);
                }
            }

            //Double tap to dash, type 1
            if (dashAbl1 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
            {
                //Is true if double tapped, count is number of taps minus 1
                if (dashTimer > 0 && dashTapCount == 1)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        rb.AddForce(-dashSpeed, 0, 0);
                    }

                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        rb.AddForce(dashSpeed, 0, 0);
                    }
                }

                else
                {
                    dashTimer = dashTimerInit;
                    dashTapCount += 1;
                }
            }

            //Double tap to dash, type 2
            if (dashAbl2 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
            {
                //Is true if double tapped, count is number of taps minus 1
                if (dashTimer > 0 && dashTapCount == 1)
                {
                    //Makes dash go left
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        dashSpeed = -dashSpeed;
                    }

                    isDashing = true;
                }

                else
                {
                    dashTimer = dashTimerInit;
                    dashTapCount += 1;
                }
            }

            //Updating dashtimer
            if (dashTimer > 0 && (dashAbl1 || dashAbl2))
            {
                dashTimer -= 1 * Time.deltaTime;
            }

            else
            {
                dashTapCount = 0;
            }

            //Stops dash once max length has been reached
            if (isDashing && dashLength > 0)
            {
                dashLength -= 1 * Time.deltaTime;
                rb.AddForce(dashSpeed, 0, 0);
                rb.constraints = ~RigidbodyConstraints.FreezePositionX;
            }
            else
            {
                dashLength = maxDashLength;
                isDashing = false;
                rb.constraints &= ~RigidbodyConstraints.FreezePositionY;

                //Resets dash to right side
                dashSpeed = Mathf.Abs(dashSpeed);
            }

            //Throw a grappling hook using the R button
            if (GrHAbl && Input.GetKeyDown(KeyCode.R))
            {
                grapplingHookSpawn();
            }

            //Mouse controles
            {
                if (attackOn)
                {

                    if (Input.GetMouseButton(0))
                    {
                        Vector3 tempPos = new Vector3(0, 0, 0);
                        float tempAngle = AngleBetweenPoints(gameObject.transform.position, mouseP);
                        if (gameObject.transform.position.x < mouseP.x)
                        {
                            tempPos = AngleToPosition(tempAngle, attackRange) + gameObject.transform.position;
                        }
                        else
                        {
                            tempPos = -AngleToPosition(tempAngle, attackRange) + gameObject.transform.position;
                        }
                        GameObject tempAttack = Instantiate(attack);
                        tempAttack.transform.position = tempPos;

                    }
                }

                //Predetermind attack combos 
                if (attackV2On)
                {
                    //An attack will be triggered on mouse
                    if ((Input.GetMouseButtonDown(0)) && (!usingAttackV2))
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

                    }
                }
            }

        }
    }

    //Grappling hook ability
    void grapplingHookSpawn()
    {

        //Recalls the grappling hook if you press R again
        if (GameObject.Find("Grappling Hook(Clone)") != null)
        {
            Destroy(GameObject.Find("Grappling Hook(Clone)"));
            return;
        }

        Rigidbody rigidGrHook;

        //The hook cannot be spawned past given maximum range, calculating where that is
        float grappleAngle = Mathf.Atan((rb.transform.position.y - mouseP.y) / (rb.transform.position.x - mouseP.x));
        Vector3 grapplePos = new Vector3(rb.transform.position.x, rb.transform.position.y, 0);

        rigidGrHook = Instantiate(grapplingHook, grapplePos, rb.rotation) as Rigidbody;

        //Reversing magnitude in the negative x quadrant to make sure the hook always moves away from the player
        if (mouseP.x - rb.transform.position.x < 0)
        {
            grThrowSpeed = -grThrowSpeed;
        }

        rigidGrHook.AddForce(grThrowSpeed * Mathf.Cos(grappleAngle), grThrowSpeed * Mathf.Sin(grappleAngle), 0);
        grThrowSpeed = Mathf.Abs(grThrowSpeed);

    }

}
