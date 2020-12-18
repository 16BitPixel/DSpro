﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public float damagingValue;
    private Animator animator;
    void Start()
    {
        animator = transform.root.GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Hit " + other.collider.name);
        if (other.collider.name == "Player" && animator.GetBool("isAttacking"))
        {
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float percent = normalizedTime - Mathf.Floor(normalizedTime);
            if(percent > 0.5f && percent < 0.9f)
            {
                other.transform.GetComponent<DS.PlayerManager>().takeDamage(damagingValue);
            }            
        }
    }
}