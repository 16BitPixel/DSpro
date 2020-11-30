using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DS {
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private float health;
        public float healthCap;
        [SerializeField]
        private float stamina;
        public float staminaCap;
        public bool isRunning;
        InputHandler inputHandler;
        Animator anim;

        
        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            
            health = healthCap;
            stamina = staminaCap;
        }

        // Update is called once per frame
        void Update()
        {
            inputHandler.isInteracting = anim.GetBool("isInteracting");
            inputHandler.rollFlag = false;
            
            replenishStamina(10f);
        }

        #region stat modifiers
        // drainStamina is called once per frame while sprinting, i.e. - isRunning = true
        public void drainStamina(float drainRate)
        {            
            
            if (stamina <= 0)
            {
                stamina = 0;
            }
            else
            {                
                stamina -= drainRate * Time.deltaTime;
            }
        }
        
        // Function is called once every time the player object is hit/damaged by an enemy. Object is destroyed when health hits 0.
        public void takeDamage(float damageVal)
        {
            
            
            health -= damageVal;
            if (health <= 0)
            {
                // TODO: Death animation.
            }
        }

        // Function is called once per frame for when the character is not sprinting, ie - isRunning = false;
        public void replenishStamina(float regenRate)
        {
            if (stamina < staminaCap)
            {
                stamina += regenRate * Time.deltaTime;
            }
            else
            {
                stamina = staminaCap;
            }
        }
        
        public void heal(float healAmount)
        {
            // TODO: Smooth out healing linearly based on heal amount.
            health = Mathf.Clamp(health + healAmount, 0, healthCap);            
        }
        #endregion
    }
}
