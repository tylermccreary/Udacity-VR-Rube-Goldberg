using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatArea : MonoBehaviour {
    public Material cheatingMaterial;
    public Material okMaterial;
    public ControllerInputManager controllerInputManagerLeft;
    public ControllerInputManager controllerInputManagerRight;

    private const string THROWABLE_TAG = "Throwable";
    private const string DEFAULT_LAYER = "Default";
    private const string INACTIVE_LAYER = "Inactive";

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == THROWABLE_TAG) {
            if (other.transform.parent != null)
            {
                other.gameObject.GetComponent<Renderer>().material = cheatingMaterial;
                other.gameObject.layer = LayerMask.NameToLayer(INACTIVE_LAYER);
            }
            controllerInputManagerLeft.SetCheating(true);
            controllerInputManagerRight.SetCheating(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == THROWABLE_TAG)
        {
            Debug.Log(other.transform.parent);
            controllerInputManagerLeft.SetCheating(false);
            controllerInputManagerRight.SetCheating(false);
        }
        if (other.gameObject.tag == THROWABLE_TAG && other.transform.parent != null)
        {
            other.gameObject.GetComponent<Renderer>().material = okMaterial;
            other.gameObject.layer = LayerMask.NameToLayer(DEFAULT_LAYER);
        }
    }
}
