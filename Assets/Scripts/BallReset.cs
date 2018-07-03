using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {
    public GameManager gameManager;
    public Material defaultMaterial;
    public Rigidbody rigid;

    private Vector3 ballResetPosition;
    private const string GROUND_TAG = "Ground";
    private const string GOAL_TAG = "Goal";
    private const string DEFAULT_LAYER = "Default";

    // Use this for initialization
    void Start () {
        ballResetPosition = transform.position;
	}

    private void Update()
    {
        if (transform.position.y < -1.0f)
        {
            Reset();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == GROUND_TAG)
        {
            Reset();
        }
    }

    private void Reset()
    {
        transform.position = ballResetPosition;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        gameObject.GetComponent<Renderer>().material = defaultMaterial;
        gameObject.layer = LayerMask.NameToLayer(DEFAULT_LAYER);
        gameManager.Reset();
    }
}
