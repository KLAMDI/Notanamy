﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Mouse possition
    Vector3 mouseP;

    //Rigidbody
    private Rigidbody rb;

    //Restrictions
    public bool allowMovement = true;

    //X movement
    public float speed;
    public float minSpeed;
    public float maxSpeed;

    //Y movement
    public float jumpSpeed;
    public float airJumpMultiplier;
    public int maxAirJumps;
    int airJumps;

    //drag
    public float drag;
    public bool airDragTest;

    //gravity
    public float gravity;
    public float wallGravity;
    public float fallingMultiplier;
    public float lowJumpMultiplier;
    float fallingMultInit;
    float lowJumpMultInit;
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

    //WallJump
    public bool wallJumpAbility;
    public float wallJumpX;
    public float wallJumpY;


    //Grappling Hook
    public bool GrHAbl;
    public float grThrowSpeed;
    public float grappleLength;
    public float grapplePullStrength;
    public Rigidbody grapplingHook;
    Rigidbody rigidGrHook;
    Vector3 lastVel;
    bool currentlyGrappling;
    float deltaGrapTime;
    float lastGrapTime;

    //Slow Time
    public bool TimeAbl;
    public float timeSlowStrength;
    public float timeSlowDur;
    public Transform zaWaruMaru;
    Transform colTimeSlow;
    public float timeSlowRadius;
    bool timeSlowStart;
    float currentRadius;

    //collision ditection
    private Collider col;
    private float distToGround;
    private float distToWall;
    RaycastHit ray;

    //Attack
    float anglePlayerMouse;


    // Use this for initialization
    void Start() {

        //Get the nessesarie components
        rb = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<Collider>();
        // get the distance to ground

        distToGround = col.bounds.extents.y;
        distToWall = col.bounds.extents.x;

        //For effects that alter gravity
        normalGravity = gravity;
      
        //Dashing initial values
        dashTimerInit = dashTimer;
        dashLength = maxDashLength;

        //Grappling initial values
        fallingMultInit = fallingMultiplier;
        lowJumpMultInit = lowJumpMultiplier;
    }

    //Checks if the player in on the ground using 5 raycast to minimize the area not checked and returns a boolian
    public bool IsGrounded() {
         
        bool OnGround1 = (Physics.Raycast(transform.position, -Vector3.up, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnGround2 = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 2 + 0.01f, transform.position.y, transform.position.z), -Vector3.up, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnGround3 = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 2 - 0.01f, transform.position.y, transform.position.z), -Vector3.up, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnGround4 = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 4, transform.position.y, transform.position.z), -Vector3.up, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnGround5 = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 4, transform.position.y, transform.position.z), -Vector3.up, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnGround = (OnGround1 || OnGround2 || OnGround3 || OnGround4 || OnGround5);
        return OnGround;
    }

    //Checks if the player is on the left wall
    public bool OnLeftWall()
    {
        bool OnWall1 = (Physics.Raycast(transform.position, Vector3.left, out ray,  distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnWall2 = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2 + 0.01f, transform.position.z), Vector3.left, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnWall3 = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2 + 0.01f, transform.position.z), Vector3.left, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnWall = (OnWall1 || OnWall2 || OnWall3);
        return OnWall;
    }

    //Checks if the player is on the right wall
    public bool OnRightWall()
    {
        bool OnWall1 = (Physics.Raycast(transform.position, Vector3.right, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnWall2 = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2 + 0.01f, transform.position.z), Vector3.right, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnWall3 = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2 + 0.01f, transform.position.z), Vector3.right, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnWall = (OnWall1 || OnWall2 || OnWall3);
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

    public void CheckSlopes()
    {
        //Getting the rotation of the slope if there is any
        float rightSlopeRotation = 0;
        float leftSlopeRotation = 0;
        float x = 0;
        float y = 0;
        //Check if there is a slope right and give its rotation if it detects one
        Debug.DrawRay(new Vector3(transform.position.x + transform.localScale.x/2, transform.position.y - transform.localScale.y / 2 + 0.01f, transform.position.z), Vector3.right, Color.red);
        if (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x/2, transform.position.y - transform.localScale.y / 2 + 0.01f, transform.position.z), Vector3.right, out ray, 0.1f) && ray.transform.tag == "Terrain")
        {
            rightSlopeRotation = ray.transform.rotation.eulerAngles.z;
            Debug.Log((rightSlopeRotation));
        }
        //Check if there is a slope left and give its rotation if it detects one
        else if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2 + 0.01f, transform.position.z), Vector3.left, out ray, distToWall + 0.01f) && ray.transform.tag == "Terrain")
        {
            leftSlopeRotation = ray.transform.gameObject.transform.rotation.z;
        }

        //Change velocoties and apply forces based on rotations
        //When moving up a slope to the right
        if (rb.velocity.x > 0 && rightSlopeRotation > 0)
        {
            
        }
        //When moving up a slope to the left
        if (rb.velocity.x < 0 && leftSlopeRotation > 0)
        {

        }
        /*
        if (Input.GetKey(KeyCode.A) && !(Input.GetKey(KeyCode.D)))
        {
            rb.AddForce(-speed, 0, 0);
        }
        */
        if (Input.GetKey(KeyCode.D) && !(Input.GetKey(KeyCode.A)))
        {
            rb.AddForce(speed * x, speed * y, 0);
        }


    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("test");
        //Mouse Position
        mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseP = new Vector3(mouseP.x, mouseP.y, 0);

        //Reset doublejumps when on the ground
        if (IsGrounded())
        {
            airJumps = maxAirJumps;
        }

        //Controls
        {
            if (allowMovement)
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
                    if (!(OnLeftWall() || OnRightWall()) || IsGrounded())
                    {
                        //Jumping while on the ground is higher
                        if (IsGrounded())
                        {
                            Debug.Log("test");
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
                    else if (wallJumpAbility)
                    {
                        //On Left wall jump right
                        if (OnLeftWall())
                        {
                            rb.AddForce(new Vector3(wallJumpX, wallJumpY, 0) - new Vector3(0, rb.velocity.y, 0), ForceMode.VelocityChange);
                        }
                        //On right wall jumo left
                        if (OnRightWall())
                        {
                            rb.AddForce(new Vector3(-wallJumpX, wallJumpY, 0) - new Vector3(0, rb.velocity.y, 0), ForceMode.VelocityChange);
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

                //Grappling Hook spawn
                if (GrHAbl)
                {
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        grapplingHookSpawn();
                    }
                }

                //Time Slow Ability
                if (TimeAbl)
                {
                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        zaWarudoSpawn();
                    }

                    if (Input.GetKeyUp(KeyCode.LeftShift))
                    {
                        zaWarudoSpawn();
                    }
                }

                
                //Slopes
                CheckSlopes();
            }
        }
    }

    void FixedUpdate()
    {

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

            //Without input stop micromovements
            if (!((Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.A))) || ((Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.A))) || allowMovement == false)
            {
                //Minimal speed to avoid micromovements instead of stopping
                if (rb.velocity.x < minSpeed && rb.velocity.x > -minSpeed)
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                }
            }

            //Apply drag when on ground or when airDrag is turned on
            if ((IsGrounded() || airDragTest) && !(currentlyGrappling))
            {
                if (rb.velocity.x >= minSpeed && !Input.GetKey(KeyCode.D))
                {
                    rb.AddForce(-drag, 0, 0);
                }
                if (rb.velocity.x <= -minSpeed && !Input.GetKey(KeyCode.A))
                {
                    rb.AddForce(drag, 0, 0);
                }
            }

            //Alter gravity if timeslow is active
            if (GameObject.Find("ZaWaruMaru(Clone)") != null)
            {
                gravity = normalGravity * timeSlowStrength;
            }
            else
            {
                gravity = normalGravity;
            }

        }

        //Throw a grappling hook using the R button
        if (GrHAbl)
        {

            //If the grappling hook exists and is frozen, pull the player toward it
            if (rigidGrHook)
            {

                float grappleplayerDistance = Vector3.Distance(rb.transform.position, rigidGrHook.transform.position);

                if (rigidGrHook.constraints == RigidbodyConstraints.FreezePosition)
                {
                    currentlyGrappling = true;

                    float grapAngle = Mathf.Abs(Mathf.Atan((rigidGrHook.transform.position.x - rb.transform.position.x) / (rigidGrHook.transform.position.y - rb.transform.position.y)));

                    //Setting all gravity multipliers to 1 for correct physics simulation
                    fallingMultiplier = 1;
                    lowJumpMultiplier = 1;

                    //Simulating forces that occur when the rope reaches its maximum length
                    if (grappleplayerDistance >= grappleLength)
                    {

                        //rb.transform.position = rigidGrHook.transform.position + (rb.transform.position - rigidGrHook.transform.position) * grappleLength / grappleplayerDistance;
                        Vector3 grapDir = (rigidGrHook.transform.position - rb.transform.position).normalized;
                        Vector3 grapAngleDir;

                        //Horizontal force due to gravity
                        if (rigidGrHook.transform.position.x - rb.transform.position.x < 0)
                        {
                            grapAngleDir = new Vector3(-grapDir.y, grapDir.x, 0);
                        }
                        else
                        {
                            grapAngleDir = new Vector3(grapDir.y, -grapDir.x, 0);
                        }

                        deltaGrapTime = Time.time - lastGrapTime;

                        rb.AddForce(gravity * Mathf.Cos(grapAngle) * grapAngleDir);

                        //Tension force due to rope length and gravity
                        float grapTension1 = rb.velocity.sqrMagnitude / grappleplayerDistance;
                        float grapTension2 = gravity * Mathf.Cos(grapAngle);

                        rb.AddForce((grapTension1 + grapTension2) * grapDir);

                        //Slow down the player, simulating the finite rope
                        float currentVelGrap = Vector3.Dot(rb.velocity, grapDir);
                        rb.AddForce(-(currentVelGrap * grapDir) / deltaGrapTime);

                        lastGrapTime = Time.time;
                    }
                }

                //Pulls the grappling hook towards the player to simulate rope length
                else if (grappleplayerDistance > grappleLength)
                {
                    rigidGrHook.transform.position = rb.transform.position + (rigidGrHook.transform.position - rb.transform.position) * grappleLength / grappleplayerDistance;
                }

            }

        }

        //Makes sure to turn drag and the gravity multipliers back on if there's no grappling hook 
        if (GameObject.Find("Grappling Hook(Clone)") == null)
        {
            currentlyGrappling = false;
            fallingMultiplier = fallingMultInit;
            lowJumpMultiplier = lowJumpMultInit;
        }

        if (TimeAbl)
        {
            if (GameObject.Find("ZaWaruMaru(Clone)") != null)
            {
                colTimeSlow.transform.position = rb.transform.position;
            }
        }

    }

    //Grappling hook ability spawn grappling hook
    void grapplingHookSpawn()
    {

        //Recalls the grappling hook if you press R again
        if (GameObject.Find("Grappling Hook(Clone)") != null)
        {
            Destroy(GameObject.Find("Grappling Hook(Clone)"));
            return;
        }

        //The hook cannot be spawned past given maximum range, calculating where that is
        float grapAngle = Mathf.Atan((rb.transform.position.y - mouseP.y) / (rb.transform.position.x - mouseP.x));
        Vector3 grapplePos = new Vector3(rb.transform.position.x, rb.transform.position.y, 0);

        rigidGrHook = Instantiate(grapplingHook, grapplePos, rb.rotation) as Rigidbody;

        //Adding Pi to the angle in the negative x quadrant to make sure the hook always moves away from the player
        if (mouseP.x - rb.transform.position.x < 0)
        {
            grapAngle += Mathf.PI;
        }

        rigidGrHook.AddForce(grThrowSpeed * Mathf.Cos(grapAngle), grThrowSpeed * Mathf.Sin(grapAngle), 0);

    }

    //Time Slow ability spawn sphere
    void zaWarudoSpawn()
    {

        if (GameObject.Find("ZaWaruMaru(Clone)") == null)
        {

            colTimeSlow = Instantiate(zaWaruMaru, rb.position, rb.rotation);
            colTimeSlow.GetComponent<ZaWarudo>().timeSlowStrength = timeSlowStrength;

            timeSlowStart = true;
            currentRadius = 0.0f;
            StartCoroutine(WaitTimeSlow());

        }

        else
        {
            timeSlowStart = false;
            StartCoroutine(WaitTimeSlow());
        }
        
    }

    IEnumerator WaitTimeSlow()
    {
        Time.timeScale = 0.0f;

        if (timeSlowStart)
        {
            while (currentRadius < timeSlowRadius && GameObject.Find("ZaWaruMaru(Clone)") != null && timeSlowStart)
            {
                yield return new WaitForSecondsRealtime(0.1f / 20.0f);

                if (GameObject.Find("ZaWaruMaru(Clone)") != null)
                {

                    currentRadius += 0.05f * timeSlowRadius;

                    //Change radius of time slow
                    colTimeSlow.localScale = new Vector3(currentRadius, currentRadius, currentRadius);

                }
            }
        }
        else
        {
            while (currentRadius > 0.0f && GameObject.Find("ZaWaruMaru(Clone)") != null)
            {
                yield return new WaitForSecondsRealtime(0.1f / 20.0f);

                if (GameObject.Find("ZaWaruMaru(Clone)") != null)
                {

                    currentRadius -= 0.05f * timeSlowRadius;

                    //Change radius of time slow
                    colTimeSlow.localScale = new Vector3(currentRadius, currentRadius, currentRadius);

                }
            }

            if (GameObject.Find("ZaWaruMaru(Clone)") != null)
            {
                colTimeSlow.GetComponent<ZaWarudo>().removeSlow();
                Destroy(GameObject.Find("ZaWaruMaru(Clone)"));
            }


        }

        Time.timeScale = 1.0f;
    }

}

