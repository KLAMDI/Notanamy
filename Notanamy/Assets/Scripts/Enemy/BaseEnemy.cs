using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour {

    public float health;

    public float gravity;
    public float drag;
    public float airDrag;

    public bool invulnerable;

    private Collider col;
    private float distToWall;
    RaycastHit ray;

    Rigidbody rb;

    //Test if on ground
    public bool IsGrounded()
    {
        bool OnGround1 = (Physics.Raycast(transform.position, -Vector3.up, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnGround2 = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 2 + 0.01f, transform.position.y, transform.position.z), -Vector3.up, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnGround3 = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 2 - 0.01f, transform.position.y, transform.position.z), -Vector3.up, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnGround4 = (Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 4, transform.position.y, transform.position.z), -Vector3.up, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnGround5 = (Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 4, transform.position.y, transform.position.z), -Vector3.up, out ray, distToWall + 0.1f) && ray.transform.tag == "Terrain");
        bool OnGround = (OnGround1 || OnGround2 || OnGround3 || OnGround4 || OnGround5);
        return OnGround;
    }

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<Collider>();
        distToWall = col.bounds.extents.x;
    }

    // Update is called once per frame
    void Update () {

	}

    void FixedUpdate()
    {
        //Simulate drag when velocity is above 0;
        if (rb.velocity.x != 0)
        {
            //Ground and air have different drag values
            if (IsGrounded())
            {
                //Apply ground drag in correct direction
                if (rb.velocity.x > 0)
                {
                    rb.AddForce(-drag, 0, 0);
                }
                else
                {
                    rb.AddForce(drag, 0, 0);
                }
            }
            else
            {
                //Apply air drag in correct direction
                if (rb.velocity.x > 0)
                {
                    rb.AddForce(-airDrag, 0, 0);
                }
                else
                {
                    rb.AddForce(airDrag, 0, 0);
                }
            }

            //Prevent micromovements using a minimum velocity
            if (rb.velocity.x < 0.05f && rb.velocity.x > -0.05f)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
    }
}
