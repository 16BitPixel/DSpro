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
    }
    private void OnAnimatorMove() {
        agent.speed = (anim.deltaPosition / Time.deltaTime).magnitude;
        transform.position = agent.nextPosition;
        anim.SetFloat("moveSpeed", agent.speed);
    }
}
