using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class rootWalker : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshAgent agent;
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();       
        agent = GetComponent<NavMeshAgent>();         
    }
    private void OnAnimatorMove() {
        bool isAttacking = anim.GetBool("isAttacking");
        if(!isAttacking)
        {
            agent.speed = (anim.deltaPosition / Time.deltaTime).magnitude;            
            anim.SetBool("isMoving", true);
        }   
        else
        {
            agent.speed = 0;
            anim.SetBool("isMoving", false);
        }   
        transform.position = agent.nextPosition;  
    }
}
