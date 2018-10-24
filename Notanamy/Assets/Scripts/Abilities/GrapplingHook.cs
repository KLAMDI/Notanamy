using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour {

    private Rigidbody grapplingBody;

    void Start()
    {
        grapplingBody = gameObject.GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Terrain") {
            grapplingBody.constraints = RigidbodyConstraints.FreezePosition;
        }
    }
}
