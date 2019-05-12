using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    //Detection bools to avoid redundant checking or double movement
    bool detectedWall;
    bool detectedSlopeUp;
    bool detectedSlopeDown;

    //Slope booleans
    bool slopeLeft;
    bool slopeRight;
    bool steepSlope;
    bool slopeFound;

    //Slope angle values
    float slopeAngle;
    public float maxAngle;

    //How far from the player is checked for a slope
    public float slopeDetectionRange;

    //X variables
    public float speed;
    public float maxSpeed;
    public float drag;

    //Y variables
    public bool isGrounded;
    public float gravity;
    public float jumpSpeed;

    //Movement speeds
    public float movementSpeed;
    public float xSpeed;
    public float YSpeed;

    RaycastHit ray;
    Rigidbody rb;


    /* bool IsGrounded()
     * This fuction checks if the player in currently on the ground using multiple raycasts 
     */
    bool IsGrounded()
    {
        bool outGrounded = false;
        //Check if on the ground without slopes
        {
            bool[] grounded = new bool[9];

            //Raycast multiple places under the player
            grounded[0] = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            grounded[1] = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 2, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            grounded[2] = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 2, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            grounded[3] = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 4, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            grounded[4] = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 4, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            grounded[5] = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 2 + transform.localScale.x / 8, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            grounded[6] = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 2 - transform.localScale.x / 8, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            grounded[7] = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 8, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");
            grounded[8] = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 8, transform.position.y, transform.position.z), -Vector3.up, out ray, transform.localScale.y / 2) && ray.transform.tag == "Terrain");

            //If any raycast detects the ground return true
            outGrounded = (grounded[0] || grounded[1] || grounded[2] || grounded[3] || grounded[4] || grounded[5] || grounded[6] || grounded[7] || grounded[8]);
        }
        //Output the result
        return outGrounded;
    }

    /* CheckSlope(int side)
     * This checks the right coner for a slope and applies the corerct forces based of the selected slope
     * int side
     * 0 = left
     * 1 = right
     * 2 = down left
     * 3 = down right
     */
    void CheckSlope(int side)
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
        //Test if the detected slope is too steep
        if (slopeAngle > maxAngle)
        {
            steepSlope = true;
        }
    }

    /* ApplySlopes(int side)
    * This applies the forces calculated in CheckSlope()
    * int side
    * 0 = left
    * 1 = right
    * 2 = down left
    * 3 = down right
    */
    void ApplySlopes(int side)
    {
        //Create temporary floats to save calculation
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


    /* CheckWall(int side)
     * This checks if the player hits a wall going or trying to go towards a certain direction
     * int side
     * 0 = going left
     * 1 = going right
     * 2 = trying to go left
     * 3 = trying to go right
     */
    void CheckWall(int side)
    {
        switch (side)
        {
            case 0:
                if (WallRaycast(Vector3.left))
                {
                    //Stop horizontal movement when hitting a wall
                    movementSpeed = 0;
                }
                break;
            case 1:
                if (WallRaycast(Vector3.right))
                {
                    //Stop horizontal movement when hitting a wall
                    movementSpeed = 0;
                }
                break;
            case 2:
                if (WallRaycast(Vector3.left))
                {
                    //Tell the check that you've hit a wall
                    detectedWall = true;
                    //Stop horizontal movement when hitting a wall
                    movementSpeed = 0;
                }
                break;
            case 3:
                if (WallRaycast(Vector3.right))
                {
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

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {
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
            CheckSlope(0);
            if (!detectedSlopeUp)
            {

                CheckSlope(3);
            }
        }
        if (movementSpeed > 0)
        {
            CheckSlope(1);
            if (!detectedSlopeUp)
            {

                CheckSlope(2);
            }
        }
        //Check both sides if the player isn't moving
        if (movementSpeed == 0)
        {
            CheckSlope(2);
            CheckSlope(3);
        }

        //Change speeds
        if (!steepSlope)
        {
            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                detectedWall = false;
                //Check if the player is trying to moce into a wall to the left
                CheckWall(2);

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
                CheckWall(3);
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

        //Jump when on the ground and pressing w (Not allowed when on a steep slope)
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
                CheckWall(1);
            }
            if (movementSpeed < 0)
            {
                //Look left for a wall and kill speed if found
                CheckWall(0);
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
}