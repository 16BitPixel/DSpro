using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public float damagingValue;
    private Animator animator;
    public float animationTime = 0.3f;
    void Start()
    {
        animator = transform.root.GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Hit " + other.collider.name);
         Debug.Log("Hit " + other.collider.name); // this is added for a push-check, safe to delete this. 
        if (other.collider.name == "Player" && animator.GetBool("isAttacking"))
        {
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float percent = normalizedTime - Mathf.Floor(normalizedTime);
            if(percent > animationTime)
            {
                other.transform.GetComponent<DS.PlayerManager>().takeDamage(damagingValue);
            }            
        }
        else if (other.collider.tag == "Enemy")
        {
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float percent = normalizedTime - Mathf.Floor(normalizedTime);
            if(percent > animationTime)
            {
                other.transform.GetComponent<creepwalk>().takeDamage(damagingValue);
            }  
        }
    }
}
