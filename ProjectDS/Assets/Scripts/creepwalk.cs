using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.Events;

public class creepwalk : MonoBehaviour
{
    #region Enemy derived calculations
    
    public class CreepStats
    {
        // Ssssss
        private float health;

        public float healthCap;

        [SerializeField]
        private float stamina; 

        [SerializeField]
        public float staminaCap;
        
        public void onSpawn()
        {
            health = healthCap;
            stamina = staminaCap;
        }
        public void drainStamina(float drainRate)
        {
            if (stamina <=0)
            {
                stamina = 0;
            }
            else
            {
                stamina -= drainRate * Time.deltaTime;
            }
        }

        public void removeStamina(float amount)
        {
            stamina = (stamina - amount <=0) ? 0 : stamina - amount;
        }

        public void takeDamage(float damage)
        {
            health = (health - damage <= 0) ? 0 : health - damage;
        }

        public void heal(float healAmount)
        {
            health = (health + healAmount >= healthCap) ? healthCap : health + healAmount;
        }

        public void replenishStamina(float replenishRate)
        {
            if (stamina >= staminaCap)
            {
                stamina = staminaCap;
            }
            else
            {
                stamina += replenishRate * Time.deltaTime;
            }
        }

        public float getStamina()
        {
            return stamina;
        }
        public float getHealth()
        {
            return health;
        }
    }

    public Transform eyes;

    [SerializeReference]
    public CreepStats stats;    

    public float dmgVal;
    public float rotationSpeed;

    public float leashThreshold;
    public float combatThreshold;
    #endregion

    #region transforms
    
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
        player = GameObject.Find("Player").transform;
        parentCheckpoints = GameObject.Find("Checkpoints").transform;
        anim = GetComponent<Animator>();
        playerNeck = player.Find("Neck");
        eyes = transform.Find("eyes");
        for (int i = 0; i < parentCheckpoints.childCount; i++)
        {            
            checkpoints.Add(parentCheckpoints.GetChild(i));
        }
        nextCheckpoint = 0;
        hasAcquiredPlayer = false;        
        angularSpeed = agent.angularSpeed;
        stats = new CreepStats();
        stats.healthCap = 100f;
        stats.staminaCap = 100f;
        stats.onSpawn();
        // agent.updateRotation = false;
        StartCoroutine(Patrol());
        
    }
    void Update()
    {
        if (player != null)      
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
        bool patrolling = true;
        while(patrolling)
        {
            stats.replenishStamina(10f);
            if (hasAcquiredPlayer && !los)
            {                            
                los = inLineOfSight();
                if (los)
                {
                    // StartCoroutine(Engage());
                    patrolling = false;

                    break;
                }                                   
            }
            if (!agent.isStopped)
            {
                // faceTarget(checkpoints.ElementAt(nextCheckpoint).transform.position);
            }
            if(!agent.hasPath && checkpoints != null)
            {
                nextCheckpoint = Random.Range(0, checkpoints.Count);               
                // faceTarget(checkpoints.ElementAt(nextCheckpoint).transform.position);
                agent.SetDestination(checkpoints.ElementAt(nextCheckpoint).position);                        
            }
            yield return new WaitForSeconds(waitTime);
        }
        StartCoroutine(Engage());
        yield return null;                
    }

    IEnumerator Engage()
    {
        rotationSpeed = 9000;
        bool engaging = true;        
        while(engaging)
        {
            if (player != null)
                faceTarget(player.position);
            if (!los)
            {
                engaging = false;        
                agent.ResetPath();        
                StartCoroutine(Patrol());                
                break;
            }
            if (distance > leashThreshold)
            {
                
                anim.SetBool("isAttacking", false);
                anim.SetInteger("attackType", -1);                     
                anim.SetBool("isMoving", true);
                agent.ResetPath();
                agent.isStopped = false;
                agent.speed = (anim.deltaPosition / Time.deltaTime).magnitude;           
                agent.stoppingDistance = leashThreshold;
                agent.SetDestination(player.position);
            }
            else
            {
                // Commence attacking
                if (distance >= combatThreshold)
                {
                    anim.SetBool("isAttacking", false);
                    anim.SetInteger("attackType", -1);
                    anim.SetBool("isMoving", true);
                    agent.ResetPath();
                    agent.isStopped = false;
                    agent.speed = (anim.deltaPosition / Time.deltaTime).magnitude;
                    // faceTarget(player.position);
                    // The distance at which to stop from the player.
                    agent.stoppingDistance = combatThreshold;
                    agent.SetDestination(player.position);                    
                }
                else
                {
                    anim.SetBool("isAttacking", true);                    
                    // agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    agent.ResetPath();
                    if(stats.getStamina() > 0 && distance <= combatThreshold)
                    {
                        int attackType = getAttack();
                        anim.SetInteger("attackType", attackType);                    
                        // yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
                        
                        // continue;                     
                    }                    
                }
            }                                                
            yield return new WaitForSeconds(waitTime);
            anim.SetInteger("attackType", -1);
        }
        yield return null;
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
        if (Mathf.Abs(Quaternion.Angle(transform.rotation, rotation)) >= 0.9f)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * rotationSpeed);               
    }

    private int getAttack()
    {
        bool jump = Random.value > 0.96f;
        if (jump)
        {
            return 2;
        }
        else
        {
            return Random.Range(0, 2);
        }
    }

    
}
