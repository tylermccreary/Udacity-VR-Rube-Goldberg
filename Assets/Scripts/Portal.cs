using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public GameObject teleportOther;
    public Transform teleportOtherBallPos;

    private const string THROWABLE_TAG = "Throwable";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == THROWABLE_TAG)
        {
            if (other.transform.parent != null)
            {
                return;
            }
            other.transform.position = teleportOtherBallPos.position;
            Rigidbody rigid = other.transform.GetComponent<Rigidbody>();
            Vector3 reflection = Vector3.Reflect(rigid.velocity, transform.forward);

            Vector3 angleDiff = teleportOther.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
            Vector3 angleDiffRadians = new Vector3(angleDiff.x * Mathf.Deg2Rad, angleDiff.y * Mathf.Deg2Rad, angleDiff.z * Mathf.Deg2Rad);
            Vector3 reflectionZY = new Vector3(reflection.x * Mathf.Cos(angleDiffRadians.y) + reflection.z * Mathf.Sin(angleDiffRadians.y),
                reflection.y,
                -reflection.x * Mathf.Sin(angleDiffRadians.y) + reflection.z * Mathf.Cos(angleDiffRadians.y));
            Vector3 reflectionZYX = new Vector3(reflectionZY.x,
                reflectionZY.y * Mathf.Cos(angleDiffRadians.x) - reflectionZY.z * Mathf.Sin(angleDiffRadians.x),
                reflectionZY.y * Mathf.Sin(angleDiffRadians.x) + reflectionZY.z * Mathf.Cos(angleDiffRadians.x));
            rigid.velocity = reflectionZYX;
        }
    }
}
