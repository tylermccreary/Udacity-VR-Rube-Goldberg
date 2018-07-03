using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public List<GameObject> collectibles;
    public LineTutorial lineTutorial;
    public SteamVR_LoadLevel loadLevel;

    private int collectCount;

    private void Start()
    {
        collectCount = 0;
    }

    public void Collect(GameObject collectible)
    {
        collectCount++;
    }

    public void Reset()
    {
        foreach (GameObject col in collectibles)
        {
            col.SetActive(true);
            collectCount = 0;
            if (lineTutorial != null)
            {
                lineTutorial.Reset();
            }
        }
    }

    public void HitGoal()
    {
        if (collectCount == collectibles.Count)
        {

            loadLevel.Trigger();
        } else
        {
            Reset();
        }
    }
}
