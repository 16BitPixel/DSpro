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

        public RectTransform healthBar;
        public RectTransform staminaBar;
        
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


            // Stamina bar tests
            // replenishStamina(45f);            
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

            // Update UI
            updateBar(stamina, staminaCap, staminaBar);
        }
        
        // Function is called once every time the player object is hit/damaged by an enemy. Object is destroyed when health hits 0.
        public void takeDamage(float damageVal)
        {                        
            float target = health - damageVal;
            while (health > target && target > 0)
            {
                health -= Time.deltaTime;
                updateBar(health, healthCap, healthBar);
            }
            health = target > 0 ? target : 0;
            if (health == 0)
            {
                Destroy(this.gameObject);
            }
            updateBar(health, healthCap, healthBar);
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
            // Update UI
            updateBar(stamina, staminaCap, staminaBar);
        }

        public void removeStamina(float drain)
        {
            stamina = (stamina - drain > 0) ? stamina - drain : 0;
            
            updateBar(stamina, staminaCap, staminaBar);
        }
        
        public void heal(float healAmount)
        {
            // TODO: Smooth out healing linearly based on heal amount.
            health = Mathf.Clamp(health + healAmount, 0, healthCap);     
            // Update UI
            updateBar(health, healthCap, healthBar);
        }

        public float getStam()
        {
            return stamina;
        }

        public float getHealth()
        {
            return health;
        }
        #endregion

        #region UI stuff

        private void updateBar(float numerator, float denominator, RectTransform bar)
        {
            float percentage = 1f * numerator / denominator;
            bar.localScale = new Vector3(percentage, bar.localScale.y, bar.localScale.z);
        }

        public void testDamage()
        {
            Debug.Log("Damaged!");
        }
        #endregion
    }
}
