using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepStats : MonoBehaviour
{
    
    private float health;
    public float healthCap;

    private float stamina;
    public float staminaCap;
    private void Awake() {
        stamina = staminaCap;
        health = healthCap;        
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
