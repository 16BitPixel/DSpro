using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class creepwalk : MonoBehaviour
{
    public Camera cam;
    private NavMeshAgent agent;
    public Transform player;
    public Transform parentCheckpoints;
    private List<Transform> checkpoints = new List<Transform>();
    private int nextCheckpoint;
    private bool hasAcquiredPlayer;
    // Update is called once per frame
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        for (int i = 0; i < parentCheckpoints.childCount; i++)
        {
            Debug.Log(parentCheckpoints.GetChild(i).name);
            checkpoints.Add(parentCheckpoints.GetChild(i));
        }
        nextCheckpoint = 0;
        hasAcquiredPlayer = false;
    }
    void Update()
    {        
        if (!hasAcquiredPlayer)
        {
            Patrol();
        }     
    }
    void Patrol()
    {
        if(!agent.hasPath && checkpoints != null)
        {
            agent.SetDestination(checkpoints[nextCheckpoint].position);
            nextCheckpoint++;
            if (nextCheckpoint >= checkpoints.Count)
            {
                nextCheckpoint = 0;
            }
        }   
    }
}
