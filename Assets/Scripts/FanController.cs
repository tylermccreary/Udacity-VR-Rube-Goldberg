using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : MonoBehaviour {

    public float fanForce = 20.0f;

    void OnTriggerStay (Collider other)
    {
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        if (rigid != null)
        {
            float distanceAway = Vector3.Magnitude(transform.position - other.transform.position);
            rigid.AddForce(transform.forward * fanForce / distanceAway);
        }
    }
}
