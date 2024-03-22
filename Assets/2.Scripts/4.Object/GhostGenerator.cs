using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostGenerator : MonoBehaviour
{
    [SerializeField] Transform _ghostSpawnPoints;
    GameObject[] _ghosts;

    public void GhostInit()
    {
        _ghosts = GameObject.FindGameObjectsWithTag("Obstacle");
        GhostsAllActive();
    }

    public void GhostsAllActive()
    {
        for(int i =0; i<_ghosts.Length; i++)
        {
#if UNITY_ANDROID
             _ghosts[i].GetComponent<Ghost>().Init(0.1f);
#else
             _ghosts[i].GetComponent<Ghost>().Init(0.2f);
#endif
            _ghosts[i].transform.position = _ghostSpawnPoints.GetChild(i).position;
             _ghosts[i].SetActive(true);
        }
    }
    public void GhostsMove()
    {
        foreach (GameObject ghost in _ghosts)
        {
            ghost.GetComponent<Ghost>().Move();
        }
    }
}
