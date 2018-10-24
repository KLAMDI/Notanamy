using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZaWarudo : MonoBehaviour {

    public float timeSlowStrength;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Rigidbody>().velocity*timeSlowStrength;
            other.gameObject.GetComponent<Rigidbody>().mass = other.gameObject.GetComponent<Rigidbody>().mass * timeSlowStrength;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag != "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Rigidbody>().velocity / timeSlowStrength;
            other.gameObject.GetComponent<Rigidbody>().mass = other.gameObject.GetComponent<Rigidbody>().mass / timeSlowStrength;
        }
    }
}

