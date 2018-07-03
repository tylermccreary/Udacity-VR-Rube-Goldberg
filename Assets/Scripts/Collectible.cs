using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
    public GameManager gameManager;

    private const string THROWABLE_TAG = "Throwable";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == THROWABLE_TAG)
        {
            gameObject.SetActive(false);
            gameManager.Collect(gameObject);
        }
    }
}
