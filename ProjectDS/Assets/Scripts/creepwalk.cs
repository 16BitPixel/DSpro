using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
public class creepwalk : MonoBehaviour
{
    #region Enemy derived calculations
  
    public Transform eyes;
    static CreepStats stats;
    public float rotationSpeed;
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
    
    #region  misc
    public Animator anim;
    #endregion
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        for (int i = 0; i < parentCheckpoints.childCount; i++)
        {            
            checkpoints.Add(parentCheckpoints.GetChild(i));
        }
        nextCheckpoint = 0;
        hasAcquiredPlayer = false;        
        angularSpeed = agent.angularSpeed;
        stats = GetComponent<CreepStats>();
        StartCoroutine(Patrol());
        agent.updateRotation = false;
    }
    void Update()
    {
                 
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
        } 
    }
    IEnumerator Patrol()
    {        
        // Replenish stamina when not in engage mode
        stats.replenishStamina(40f);
        if (hasAcquiredPlayer && !los)
        {            
            StartCoroutine(waitOnNext());
            los = inLineOfSight();
            if (los)
            {
                StartCoroutine(Engage());
            }                                   
        }
        if (!agent.isStopped)
        {
            faceTarget(checkpoints.ElementAt(nextCheckpoint).transform.position);
        }
        if(!agent.hasPath && checkpoints != null)
        {
            nextCheckpoint = Random.Range(0, checkpoints.Count);               
            faceTarget(checkpoints.ElementAt(nextCheckpoint).transform.position);
            agent.SetDestination(checkpoints.ElementAt(nextCheckpoint).position);                        
        }
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(Patrol());
        
    }

    IEnumerator Engage()
    {
        // agent.angularSpeed = 0;
        Debug.Log("Engage");
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && stats.getStamina() > 0)
        {            
            faceTarget(player.position);
            agent.stoppingDistance = 4f;
            agent.SetDestination(player.position);
            // TODO: Attack?
            bool attackType = Random.value > 0.55f;
            if (attackType)
            {
                // TODO: light attack
                stats.removeStamina(35f);
            }
            else
            {
                // TODO: heavy attack
                stats.removeStamina(60f);
            }
            StartCoroutine(Engage());
        }
        else if(stats.getStamina() < 0.05f * stats.staminaCap)
        {
            while (stats.getStamina() <= 0.95f * stats.staminaCap)
            {
                faceTarget(player.position);
            // Stay a safe distance from the player.
                agent.stoppingDistance = 20f;
                agent.SetDestination(player.position);
                stats.replenishStamina(35f);
            }
            StartCoroutine(Engage());
        }
        else if (hasAcquiredPlayer)
        {
            faceTarget(player.position);
            agent.stoppingDistance = 10f;            
            agent.SetDestination(player.position);
            // Set animation to walk/run?
            yield return new WaitForSeconds(0f);                        
            StartCoroutine(Engage());
        }        
        else
        {
            Debug.Log("Stopping");            
            agent.stoppingDistance = 0f;
            agent.ResetPath();            
            StartCoroutine(Patrol());
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
        Debug.Log(rotation);
        if (Mathf.Abs(Quaternion.Angle(transform.rotation, rotation)) >= 0.9f)
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime / rotationSpeed);        

       
    }

    IEnumerator waitOnNext()
    {
        yield return new WaitForSeconds(waitTime);        
    }
}
