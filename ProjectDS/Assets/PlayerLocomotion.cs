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

        public Transform myTransform;
        public Rigidbody RB;
        public GameObject normalCamera;

        [SerializeField] float movementSpeed = 2f;
        [SerializeField] float rotationSpeed = 2f;
        float _speed;


        void Start()
        {
            RB = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            cameraObject = Camera.main.transform;
            myTransform = this.transform;
            _speed = 2f;

        }

        private void Update()
        {
            float delta = Time.deltaTime;
            
            inputHandler.tickInput(delta);
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;
            moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            RB.velocity = projectedVelocity * _speed;
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
