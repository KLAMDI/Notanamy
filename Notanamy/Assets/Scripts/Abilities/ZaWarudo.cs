using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZaWarudo : MonoBehaviour {

    public float timeSlowStrength;
    public List<GameObject> slowedObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        foreach (var item in slowedObjects)
        {
            item.GetComponent<Rigidbody>().AddForce(-Physics.gravity*(1 - timeSlowStrength) * item.GetComponent<Rigidbody>().mass);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Rigidbody>().velocity*timeSlowStrength;
            other.gameObject.GetComponent<Rigidbody>().mass = other.gameObject.GetComponent<Rigidbody>().mass / timeSlowStrength;

            slowedObjects.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag != "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Rigidbody>().velocity / timeSlowStrength;
            other.gameObject.GetComponent<Rigidbody>().mass = other.gameObject.GetComponent<Rigidbody>().mass * timeSlowStrength;

            slowedObjects.Remove(other.gameObject);
        }
    }
}

