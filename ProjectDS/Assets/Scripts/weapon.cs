﻿using System.Collections;
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

        // To print something, Ngork test
        Debug.Log("Hit " + other.collider.name); 
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
