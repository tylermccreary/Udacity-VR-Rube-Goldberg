using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTutorial : MonoBehaviour {

    public Transform ball;
    public GameObject collectible;
    public Transform goal;
    public LineRenderer line;

    private void Start()
    {
        line.SetVertexCount(3);
        line.SetPosition(0, ball.position);
        line.SetPosition(1, collectible.transform.position);
        line.SetPosition(2, goal.position);

    }

    // Update is called once per frame
    void Update ()
    {
        line.SetPosition(0, ball.position);
        if (!collectible.gameObject.activeInHierarchy)
        {
            line.SetPosition(1, ball.position);
        }
	}

    public void Reset()
    {
        line.SetPosition(1, collectible.transform.position);
    }
}
