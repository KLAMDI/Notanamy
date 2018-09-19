using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    public int attackTimer;
    public bool isScytheSpin = false;
    public Vector3 movement;
    public int duration;
    public bool overHead;
    float angle = 90;
    public int turnSpeed;
    public Vector3 knockback;
    GameObject player;

    List<Collider> enemiesHit;


    public Vector3 AngleToPosition(float angle, float Hy, float posZ = 0)
    {
        angle = Mathf.Deg2Rad * angle;
        float Op;
        float Ad;
        Op = Mathf.Sin(angle) * Hy;
        Ad = Mathf.Cos(angle) * Hy;
        Vector3 pos = new Vector3(Ad, Op, posZ);
        return pos;
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

        if (duration > 0)
        {
            if (overHead)
            {
                angle -= turnSpeed;
                transform.position = player.transform.position + AngleToPosition(angle, 1);
            }
            else
            {
                transform.position += movement;
                duration--;
            }
        }
        else
        {
            overHead = false;
        }


        attackTimer--;
        if(attackTimer <= 0)
        {
            Destroy(gameObject);
        }

        
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            other.GetComponent<Rigidbody>().AddForce(knockback);
        }
    }
}
