using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DS
{
    public class PlayerLocomotion : MonoBehaviour
    {
        Transform cameraObject;
        InputHandler inputHandler;
        Vector3 moveDirection;

        PlayerManager _pm;
        public float rollCost;
        public float sprintCost;
        public float staminaRegen;
        public Transform myTransform;
        public Rigidbody RB;
        public GameObject normalCamera;

        [HideInInspector] public AnimatorHandler AnimeHandler;

        [SerializeField] float movementSpeed = 2f;
        [SerializeField] float rotationSpeed = 2f;
        float _speed;


        void Start()
        {
            RB = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            AnimeHandler = this.transform.GetChild(0).transform.GetComponent<AnimatorHandler>();
            cameraObject = Camera.main.transform;
            myTransform = this.transform;
            AnimeHandler.init();
            _speed = 2f;
            _pm = GetComponent<PlayerManager>();
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            
            inputHandler.tickInput(delta);
            
            
            HandleMovement(delta);

            HandleRollingAndSprinting(delta);

           
        }

        public void HandleMovement(float delta)
        {
            if(AnimeHandler.anim.GetBool("isInteracting"))
                return;

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;
            moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            RB.velocity = projectedVelocity * _speed;

            AnimeHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);
            if (AnimeHandler.canRotate)
            {
                HandleRotation(delta);
            }
            if(!AnimeHandler.anim.GetBool("isInteracting"))
            {
                _pm.replenishStamina(staminaRegen);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (AnimeHandler.anim.GetBool("isInteracting")) return;

            if (inputHandler.rollFlag && _pm.getStam() > 0)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                if (inputHandler.moveAmount > 0)
                {
                    _pm.removeStamina(rollCost);
                    AnimeHandler.PlayerTargetAnimation("Rolling", true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                }
                else
                {
                    Debug.Log("Implement BackStep Animation");                    
                }
            }
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPos; 

        void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverRide = inputHandler.moveAmount;
            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;
            targetDir.Normalize();
            targetDir.y = 0;

            if(targetDir == Vector3.zero)
            {
                targetDir = myTransform.forward;
            }

            float RS = rotationSpeed;
            Quaternion LR = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, LR, RS * delta);

            myTransform.rotation = targetRotation; 
        }
        #endregion

    }
}
