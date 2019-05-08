using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public bool movemode1;
    public bool movemode2;

    //movemode2
    //Detection bools to avoid redundant checking or double movement
    bool detectedWall;
    bool detectedSlopeUp;
    bool detectedSlopeDown;

    bool slopeLeft;
    bool slopeRight;

    bool steepSlope;
    bool slopeFound;

    float slopeAngle;
    public float maxAngle;

    //Test varaibles 
    float testAngle;


    

    public bool test;

    public float speed;
    public float maxSpeed;
    public float drag;


    public bool isGrounded;
    public float gravity;
    public float jumpSpeed;

    public float movementSpeed;
    public float xSpeed;
    public float YSpeed;

    RaycastHit ray;
    Transform mem;

    Rigidbody rb;

    public float slopeDetectionRange;
    public float downSlopeDetectionRange;

    public float lowSlopeDist;
    public float midSlopeDist;

    bool IsGrounded()
    {

        bool grounded = false;
        //Check if on the ground without slopes
        {
            bool groundedM = false;
            bool groundedL = false;
            bool groundedR = false;
            bool groundedML = false;
            bool groundedMR = false;
            bool groundedMLL = false;
            bool groundedMRR = false;
            bool groundedMML = false;
            bool groundedMMR = false;


            groundedM = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            groundedL = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 2, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            groundedR = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 2, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            groundedML = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 4, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            groundedMR = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 4, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            groundedMLL = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 2 + transform.localScale.x / 8, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            groundedMRR = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 2 - transform.localScale.x / 8, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            groundedMML = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 8, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            groundedMMR = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 8, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");

            grounded = (groundedM || groundedL || groundedR || groundedML || groundedMR || groundedMLL || groundedMRR || groundedMML || groundedMMR);

        }
        
        return grounded;
    }

    bool OnSlope()
    {
        bool lowSL = false;
        bool LML = false;
        bool GL = false;
        bool GML = false;
        bool left = false;
        bool lowSR = false;
        bool LMR = false;
        bool GR = false;
        bool GMR = false;
        bool right = true;

        lowSL = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.x / 2 + 0.001f, transform.position.z), Vector3.left, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");
        LML = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.x / 4, transform.position.z), Vector3.left, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");
        GL = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 2 + 0.001f, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2 + 0.01f) && ray.transform.tag == "Terrain");
        GML = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 2 + transform.localScale.x / 8, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2 + 0.01f) && ray.transform.tag == "Terrain");
        lowSR = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.x / 2 + 0.001f, transform.position.z), Vector3.right, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");
        LMR = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.x / 4, transform.position.z), Vector3.right, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");
        GR = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 2 - 0.001f, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2 + 0.01f) && ray.transform.tag == "Terrain");
        GMR = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 2 - transform.localScale.x / 8, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2 + 0.01f) && ray.transform.tag == "Terrain");



        left = ((lowSL && GL && !LML && !GML));
        right = ((lowSR && GR && !LMR && !GMR));

        if (left)
        {
            if (Input.GetKey(KeyCode.D))
            {
                left = false;
            }
            Debug.Log("left");
        }
        if (right)
        {
            if (Input.GetKey(KeyCode.A))
            {
                right = false;
            }
            Debug.Log("right");
        }

        return (right || left);
    }
    
    /*
     * 
     * 
     */
    void IsGrounded2()
    {

    } 


    /* CheckSlope2(int side)
     * This checks the right coner for a slope and applies the corerct forces based of the selected slope
     * int side
     * 0 = left
     * 1 = right
     * 2 = down left
     * 3 = down right
     * 
     */
    void CheckSlope2(int side)
    {
        slopeFound = false;
        steepSlope = false;
        switch (side)
        {
            case 0:
                //Check from the lower left corner to the left
                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2, transform.position.z), Vector3.left, out ray, transform.localScale.y / 2 + slopeDetectionRange) && ray.transform.tag == "Terrain")
                {
                    slopeAngle = Vector3.Angle(ray.normal, Vector3.up);
                    slopeFound = true;
                    detectedSlopeUp = true;
                    slopeLeft = true;
                }
                break;
            case 1:
                //Check from the lower right conrner to the right
                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2, transform.position.z), Vector3.right, out ray, transform.localScale.y / 2 + slopeDetectionRange) && ray.transform.tag == "Terrain")
                {
                    slopeAngle = Vector3.Angle(ray.normal, Vector3.up);
                    slopeFound = true;
                    detectedSlopeUp = true;
                    slopeRight = true;
                }
                break;
                //Check form the lower left corner downwards
            case 2:
                if (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.y / 2, transform.position.y, transform.position.z), Vector3.down, out ray, transform.localScale.y / 2 + slopeDetectionRange) && ray.transform.tag == "Terrain")
                {
                    slopeAngle = Vector3.Angle(ray.normal, Vector3.up);
                    if (slopeAngle != 0)
                    {
                        slopeFound = true;
                        detectedSlopeDown = true;
                    }
                    slopeLeft = true;
                }
                break;
                //check from the lower right corner downwards
            case 3:
                if (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.y / 2, transform.position.y, transform.position.z), Vector3.down, out ray, transform.localScale.y / 2 + slopeDetectionRange) && ray.transform.tag == "Terrain")
                {
                    slopeAngle = Vector3.Angle(ray.normal, Vector3.up);
                    if (slopeAngle != 0)
                    {
                        slopeFound = true;
                        detectedSlopeDown = true;
                    }
                    slopeRight = true;
                }
                break;
        }
        if (slopeAngle > maxAngle)
        {
            steepSlope = true;
        }
    }

    /* CheckSlope2(int side)
    * This checks the right coner for a slope and applies the corerct forces based of the selected slope
    * int side
    * 0 = left
    * 1 = right
    * 2 = down left
    * 3 = down right
    * 
    */
    void ApplySlopes(int side)
    {
        float neededX = 0;
        float neededY = 0;

        //Apply correct horizontal and vertical forces when on a slope
        if (slopeFound)
        {
            //Determine the nessesary forces
            neededX = Mathf.Abs(movementSpeed) * Mathf.Cos(Mathf.Deg2Rad * slopeAngle);
            neededY = Mathf.Abs(movementSpeed) * Mathf.Sin(Mathf.Deg2Rad * slopeAngle);

            //Apply the forces
            if (movementSpeed < 0)
            {
                xSpeed = -neededX;
            }
            else
            {
                xSpeed = neededX;
            }
            //Side determines if the Y force needs to be up or down
            if (side == 0 || side == 1)
            {
                YSpeed = neededY;
            }
            else
            {
                YSpeed = -neededY;
            }
        }
    }


    /* CheckWall2(int side)
     * This checks if the player hits a wall going or trying to go towards a certain direction
     * int side
     * 0 = going left
     * 1 = going right
     * 2 = trying to go left
     * 3 = trying to go right
     */
    void CheckWall2(int side)
    {
        switch (side)
        {
            case 0:
                if (WallRaycast(Vector3.left))
                {
                    Debug.Log("Wall left");
                    //Stop horizontal movement when hitting a wall
                    movementSpeed = 0;
                }
                break;
            case 1:
                if (WallRaycast(Vector3.right))
                {
                    Debug.Log("Wall right");
                    //Stop horizontal movement when hitting a wall
                    movementSpeed = 0;
                }
                break;
            case 2:
                if (WallRaycast(Vector3.left))
                {
                    Debug.Log("Wall left");
                    //Tell the check that you've hit a wall
                    detectedWall = true;
                    //Stop horizontal movement when hitting a wall
                    movementSpeed = 0;
                }
                break;
            case 3:
                if (WallRaycast(Vector3.right))
                {
                    Debug.Log("Wall right");
                    //Tell the check that you've hit a wall
                    detectedWall = true;
                    //Stop horizontal movement when hitting a wall
                    movementSpeed = 0;
                }
                break;
        }
    }

    /* WallRayCast(Vector3 direction)
     * Checks the given direction if it detects a wall
     * Any direction other then left of right doesn't work correctly
     */
    bool WallRaycast(Vector3 direction)
    {
        //initiate bools
        bool foundWall = false;
        bool bool1 = false;
        bool bool2 = false;
        bool bool3 = false;
        bool bool4 = false;
        bool bool5 = false;

        //Check the top
        bool1 = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2 - 0.001f, transform.position.z), direction, out ray, transform.localScale.y / 2 + slopeDetectionRange) && ray.transform.tag == "Terrain");
        //Check the top-middle
        bool2 = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 4, transform.position.z), direction, out ray, transform.localScale.y / 2 + slopeDetectionRange) && ray.transform.tag == "Terrain");
        //Check the middle
        bool3 = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), direction, out ray, transform.localScale.y / 2 + slopeDetectionRange) && ray.transform.tag == "Terrain");
        //Check the bottem-middle
        bool4 = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 4, transform.position.z), direction, out ray, transform.localScale.y / 2 + slopeDetectionRange) && ray.transform.tag == "Terrain");
        //Check the bottom
        bool5 = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2, transform.position.z), direction, out ray, transform.localScale.y / 2 + slopeDetectionRange)
            && ray.transform.tag == "Terrain" && Vector3.Angle(ray.normal, Vector3.up) > 89.9 && Vector3.Angle(ray.normal, Vector3.up) < 90.1);

        //If any raycast detected a wall it's true
        foundWall = (bool1 || bool2 || bool3 || bool4 || bool5);
        //Return the result
        return foundWall;
    }


    //TooSteep() returns true if the angle of the slope on the right or left is too steep to climb
    bool TooSteep()
    {
        float angle;
        bool tooSteep = false;

        //Check the right side using a raycast
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.x / 2 + 0.001f, transform.position.z), Vector3.right, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain")
        {
            //Round the angle to one decimal 
            angle = Mathf.Round(ray.transform.localEulerAngles.z * 10f) / 10f;
            //If the slope is bigger then 45 degrees (between 45 and 90) the slope is too steep
            if (angle > 45)
            {
                tooSteep = true;
            }
        }

        //Check the right side using a raycast
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.x / 2 + 0.001f, transform.position.z), Vector3.left, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain")
        {
            //Round the angle to one decimal 
            angle = Mathf.Round(ray.transform.localEulerAngles.z * 10f) / 10f;
            //If the slope is smaller then 135 degrees (between 90 and 135) the slope is too steep
            if (angle < 135)
            {
                tooSteep = true;
            }
        }

        //Returns true if the slope on the checked side is too steep
        return tooSteep;
    }



    //Check the left side of the player for a too steep slope or wall, returns true if detected
    bool CheckLeft()
    {
        bool lowS = false;
        bool midS = false;
        bool steepSlope = false;

        //Check if the player is next to a wall or slope at a low point
        lowS = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.x / 2 + 0.001f, transform.position.z), Vector3.left, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");

        //Only check the for the middle when the bottom has detected a wal or slope
        if (lowS)
        {
            //Check if it's a wall or the slope is too steep by checking a distance from the center of the player
            midS = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.left, out ray, transform.localScale.y / 2 + midSlopeDist) && ray.transform.tag == "Terrain");

            //If the distance is too close the slope is too steep
            steepSlope = midS;
        }

        bool wallLM = false;
        bool wallM = false;
        bool wallHM = false;
        bool wallH = false;
        bool detectWall = false;

        //Check if the player is next to a wall on multiple hights
        wallLM = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.x / 4, transform.position.z), Vector3.left, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");
        wallM = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.left, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");
        wallHM = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + transform.localScale.x / 4, transform.position.z), Vector3.left, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");
        wallH = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + transform.localScale.x / 2 + 0.001f, transform.position.z), Vector3.left, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");

        detectWall = (wallLM || wallM || wallHM || wallH);


        //If there's a wall or the slope is too steep return true
        return (detectWall || steepSlope);
    }

    //Check the right of the player for a too steep slope or wall, returns true if detected
    bool CheckRight()
    {
        bool lowS = false;
        bool midS = false;
        bool steepSlope = false;

        //Check if the player is next to a wall or slope at a low point
        lowS = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.x / 2 + 0.001f, transform.position.z), Vector3.right, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");

        //Only check the for the middle when the bottom has detected a wal or slope
        if (lowS)
        {
            //Check if it's a wall or the slope is too steep by checking a distance from the center of the player
            midS = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.right, out ray, transform.localScale.y / 2 + midSlopeDist) && ray.transform.tag == "Terrain");

            //If the distance is too close the slope is too steep
            steepSlope = midS;
        }

        bool wallLM = false;
        bool wallM = false;
        bool wallHM = false;
        bool wallH = false;
        bool detectWall = false;

        //Check if the player is next to a wall on multiple hights
        wallLM = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.x / 4, transform.position.z), Vector3.right, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");
        wallM = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.right, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");
        wallHM = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + transform.localScale.x / 4, transform.position.z), Vector3.right, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");
        wallH = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + transform.localScale.x / 2 + 0.001f, transform.position.z), Vector3.right, out ray, transform.localScale.y / 2 + lowSlopeDist) && ray.transform.tag == "Terrain");

        detectWall = (wallLM || wallM || wallHM || wallH);

        //If there's a wall or the slope is too steep return true
        return (detectWall || steepSlope);
    }

    //Check the slope to the "input 'side' " side of the player and save the transform of the slope
    void checkSlope(string side)
    {
        //If the input is left or Left check on the left side of the player
        if (side == "left" || side == "Left")
        {
            if (Physics.Raycast(transform.position, Vector3.down, out ray)) // unfinished
            {
                //Test if the current memory is empty
                if (mem != null)
                {
                    //Test if the object stored in memory is the same as the slope
                    if (mem == ray.transform)
                    {
                        //if it is display the angle
                        Debug.Log(mem.localEulerAngles.z);
                    }
                    else
                    {
                        Debug.Log("Nope...");
                    }
                }
                mem = ray.transform;
            }
        }
        //If the input is right or Right check on the left side of the player
        else if (side == "right" || side == "Right")
        {
            if (Physics.Raycast(transform.position, Vector3.down, out ray)) // unfinished
            {
                //Test if the current memory is empty
                if (mem != null)
                {
                    if (mem == ray.transform)
                    {
                        Debug.Log(mem.localEulerAngles.z);
                    }
                    else
                    {
                        Debug.Log("Nope...");
                    }
                }
                mem = ray.transform;
            }
        }
        else
        {
            Debug.Log("Input '" + side + "' is incorrect.");
        }
    }


	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (movemode1){
            if (IsGrounded())
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }

            if (!isGrounded)
            {
                YSpeed -= gravity;
            }
            else
            {
                if (YSpeed < 0)
                {
                    YSpeed = 0;
                    rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                checkSlope("left");
            }

            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                YSpeed += jumpSpeed;
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (xSpeed > 0)
                {
                    xSpeed = 0;
                }
                xSpeed -= speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (xSpeed < 0)
                {
                    xSpeed = 0;
                }
                xSpeed += speed;
            }
            if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && xSpeed != 0)
            {
                if (xSpeed < drag && xSpeed > -drag)
                {
                    xSpeed = 0;
                }
                else if (xSpeed > 0)
                {
                    xSpeed -= drag;
                }
                else if (xSpeed < 0)
                {
                    xSpeed += drag;
                }
            }
            if (xSpeed > maxSpeed)
            {
                xSpeed = maxSpeed;
            }
            if (xSpeed < -maxSpeed)
            {
                xSpeed = -maxSpeed;
            }

            if (xSpeed > 0 && CheckRight())
            {
                xSpeed = 0;
                //Debug.Log("testR");
            }
            if (xSpeed < 0 && CheckLeft())
            {
                xSpeed = 0;
                //Debug.Log("testL");
            }

            //Apply calculated movement velocities
            Physics.Raycast(transform.position, Vector3.down, out ray);
            if (ray.distance < YSpeed / 100)
            {
                transform.position = transform.position + new Vector3(xSpeed / 100, -ray.distance, 0);
            }
            else
            {
                transform.position = transform.position + new Vector3(xSpeed / 100, YSpeed / 100, 0);
            }
            //rb.velocity = new Vector3(xSpeed, YSpeed, 0);
        } //Movenemt attempt 1

        if (movemode2)
        {
            xSpeed = 0;


            {
                //Check if the player is on the ground
                if (IsGrounded())
                {
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }

                //If the player isn't on the ground apply gravity
                if (!isGrounded)
                {
                    YSpeed -= gravity;
                }
                //If the player is on the ground reset downward y movement
                else
                {
                    if (YSpeed < 0)
                    {
                        YSpeed = 0;
                        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                    }
                }
            }//Y movement calculations

            //Check for slopes depending on movement direction
            detectedSlopeUp = false;
            detectedSlopeDown = false;
            steepSlope = false;
            slopeLeft = false;
            slopeRight = false;

            if (movementSpeed < 0)
            {
                CheckSlope2(0);
                if (!detectedSlopeUp)
                {

                    CheckSlope2(3);
                }
            }
            if (movementSpeed > 0)
            {
                CheckSlope2(1);
                if (!detectedSlopeUp)
                {

                    CheckSlope2(2);
                }
            }
            //Check both sides if the player isn't moving
            if (movementSpeed == 0)
            {
                CheckSlope2(2);
                CheckSlope2(3);
            }


            //Change speeds
            if (!steepSlope)
            {
                if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                {
                    detectedWall = false;
                    //Check if the player is trying to moce into a wall to the left
                    CheckWall2(2);

                    //Stop trying to move left if a wall is detected
                    if (!detectedWall)
                    {
                        movementSpeed -= speed;
                    }
                }
                if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                {
                    detectedWall = false;
                    //Checks if the player is trying to move into a wall to the right
                    CheckWall2(3);
                    //Stop trying to move left if a wall is detected
                    if (!detectedWall)
                    {
                        movementSpeed += speed;
                    }
                }
                //Apply movement when not trying to move in a particular direction
                if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) || (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)))
                {
                    //Add drag when not intending to move
                    if (movementSpeed < 0)
                    {
                        //If the drag is more then the current speed, set the speed to 0
                        if (movementSpeed < -drag)
                        {
                            movementSpeed += drag;
                        }
                        else
                        {
                            movementSpeed = 0;
                        }
                    }
                    if (movementSpeed > 0)
                    {
                        //If the drag is more then the current speed, set the speed to 0
                        if (movementSpeed > drag)
                        {
                            movementSpeed -= drag;
                        }
                        else
                        {
                            movementSpeed = 0;
                        }
                        movementSpeed -= drag;
                    }
                }
            }
            if (steepSlope)
            {
                //Slide down the slopes
                Debug.Log("SlopeLeft = " + slopeLeft + " & SlopeRight = " + slopeRight);
                if (slopeLeft)
                {
                    movementSpeed += speed;
                }
                if (slopeRight)
                {
                    movementSpeed -= speed;
                }
            }


            //Apply slope movements
            if (movementSpeed < 0)
            {
                ApplySlopes(0);
                if (!detectedSlopeUp)
                {

                    ApplySlopes(3);
                }
            }
            if (movementSpeed > 0)
            {
                ApplySlopes(1);
                if (!detectedSlopeUp)
                {

                    ApplySlopes(2);
                }
            }

            //Jump when on the ground and pressing w
            if (Input.GetKeyDown(KeyCode.W) && isGrounded && !steepSlope)
            {
                YSpeed += jumpSpeed;
            }

            {
                //Keep speed at most maxSpeed
                //Check right movement
                if (movementSpeed > maxSpeed)
                {
                    movementSpeed = maxSpeed;
                }
                //Check left movement
                if (movementSpeed < -maxSpeed)
                {
                    movementSpeed = -maxSpeed;
                }

                //Kill movement if moving into a wall
                if (movementSpeed > 0)
                {
                    //Look right for a wall and kill speed if found
                    CheckWall2(1);
                }
                if (movementSpeed < 0)
                {
                    //Look left for a wall and kill speed if found
                    CheckWall2(0);
                }
                if (!detectedSlopeDown && !detectedSlopeUp && !detectedWall)
                {
                    xSpeed = movementSpeed;
                }



            }//Final movement checks

            

            {
                Physics.Raycast(transform.position, Vector3.down, out ray);
                if (ray.distance < YSpeed / 100)
                {
                    transform.position = transform.position + new Vector3(xSpeed / 100, -ray.distance, 0);
                }
                else
                {
                    transform.position = transform.position + new Vector3(xSpeed / 100, YSpeed / 100, 0);
                }
            }//Apply movement velocities
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.right, out ray);
        //    testAngle = Vector3.Angle(ray.normal, Vector3.up);
        //    Debug.Log("The angle is " + testAngle);
        //}
	}
}
