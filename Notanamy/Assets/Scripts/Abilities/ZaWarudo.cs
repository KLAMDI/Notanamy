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
	void FixedUpdate () {

        foreach (var item in slowedObjects)
        {
            if (item != null)
            {
                item.GetComponent<Rigidbody>().AddForce(-Physics.gravity * (1.0f - Mathf.Pow(timeSlowStrength, 2)), ForceMode.Acceleration);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Rigidbody>().velocity * timeSlowStrength;
            other.gameObject.GetComponent<Rigidbody>().mass = other.gameObject.GetComponent<Rigidbody>().mass / timeSlowStrength;
            other.gameObject.GetComponent<Rigidbody>().drag = other.gameObject.GetComponent<Rigidbody>().drag * timeSlowStrength;

            slowedObjects.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag != "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Rigidbody>().velocity / timeSlowStrength;
            other.gameObject.GetComponent<Rigidbody>().mass = other.gameObject.GetComponent<Rigidbody>().mass * timeSlowStrength;
            other.gameObject.GetComponent<Rigidbody>().drag = other.gameObject.GetComponent<Rigidbody>().drag / timeSlowStrength;

            slowedObjects.Remove(other.gameObject);
        }
    }

    public void removeSlow()
    {
        foreach (var item in slowedObjects)
        {
            if (item != null)
            {
                item.GetComponent<Rigidbody>().velocity = item.GetComponent<Rigidbody>().velocity / timeSlowStrength;
                item.GetComponent<Rigidbody>().mass = item.GetComponent<Rigidbody>().mass * timeSlowStrength;
                item.gameObject.GetComponent<Rigidbody>().drag = item.gameObject.GetComponent<Rigidbody>().drag / timeSlowStrength;
            }
        }
    }
}

