using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    public GameManager gameManager;

    private const string THROWABLE_TAG = "Throwable";

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == THROWABLE_TAG)
        {
            gameManager.HitGoal();
        }
    }
}
