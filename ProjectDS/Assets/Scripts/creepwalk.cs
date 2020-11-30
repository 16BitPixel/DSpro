using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
public class creepwalk : MonoBehaviour
{
    #region Enemy derived calculations
    public class Stats{
        public int health;
        public float stamina;
        public float baseDamage;
    }
    static Stats creepstats;
    public Transform eyes;
    #endregion

    #region transforms

    public Camera cam;
    private NavMeshAgent agent;
    public Transform player;
    public Transform parentCheckpoints;
    private List<Transform> checkpoints = new List<Transform>();
    private int nextCheckpoint;

    #endregion

    #region player derived calculations

    [SerializeField]
    private bool hasAcquiredPlayer;
    [SerializeField]
    private float distance;
    public float distanceFromPlayer;
    public Transform playerNeck;
    public float waitTime = 2f;
    [SerializeField]
    private bool los = false;
    private float angularSpeed;

    #endregion
    
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        for (int i = 0; i < parentCheckpoints.childCount; i++)
        {            
            checkpoints.Add(parentCheckpoints.GetChild(i));
        }
        nextCheckpoint = 0;
        hasAcquiredPlayer = false;        
        creepstats = new Stats();
        angularSpeed = agent.angularSpeed;        

    }
    void Update()
    {
        
        StartCoroutine(Patrol());        
        distance = Vector3.Distance(player.position, transform.position);
        
        if (distance < distanceFromPlayer)
        {            
            hasAcquiredPlayer  = true;                
        }
        else if(distance > 3 * distanceFromPlayer)
        {            
            hasAcquiredPlayer = false;
            los = false;
            agent.angularSpeed = angularSpeed;
            StartCoroutine(Patrol());            
        } 
    }
    IEnumerator Patrol()
    {
        if (hasAcquiredPlayer && !los)
        {            
            StartCoroutine(waitOnNext());
            los = inLineOfSight();
            if (los)
            {
                StartCoroutine(Engage());
            }                                   
        }
        if(!agent.hasPath && checkpoints != null)
        {           
            nextCheckpoint = Random.Range(0, checkpoints.Count);               
            agent.SetDestination(checkpoints.ElementAt(nextCheckpoint).position);            
            yield return new WaitForSeconds(waitTime);            
            StartCoroutine(Patrol());
        }
    }

    IEnumerator Engage()
    {
        agent.angularSpeed = 0;
        
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && creepstats.stamina > 0)
        {            
            faceTarget(player.position);
            agent.stoppingDistance = 4f;
            agent.SetDestination(player.position);
            // TODO: Attack?
        }


        if (hasAcquiredPlayer)
        {
            faceTarget(player.position);
            agent.stoppingDistance = 10f;            
            agent.SetDestination(player.position);
            // Set animation to walk/run?
            yield return new WaitForSeconds(1f);                        
            StartCoroutine(Engage());
        }        
        else
        {
            Debug.Log("Stopping");            
            agent.stoppingDistance = 0f;
            agent.ResetPath();            
        }                                
    }

    private bool inLineOfSight()
    {        
        if(los)
        {
            return true;
        }
        Vector3 fromPosition = eyes.transform.position;
        Vector3 toPosition = playerNeck.transform.position;
        Vector3 direction = toPosition - fromPosition;
        direction.Normalize();
        
        RaycastHit hit;
        if (Physics.Raycast(fromPosition, direction, out hit, distanceFromPlayer))
        {
            Debug.DrawLine(fromPosition, hit.transform.position, Color.red);
            if (hit.collider.name == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
        }        
        return false;
    }

    private void faceTarget(Vector3 target)
    {
        Vector3 lookAt = target - transform.position;
        lookAt.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookAt);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.9f);
    }

    IEnumerator waitOnNext()
    {
        yield return new WaitForSeconds(waitTime);        
    }
}
