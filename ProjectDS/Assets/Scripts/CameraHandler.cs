using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DS
{
    public class CameraHandler : MonoBehaviour
    {
        public GameObject playerAvatar;
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        private Vector3 cameraTransformPosition;
        private LayerMask ignoreLayers;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        public float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minPivot = -35f;
        public float maxPivot = 35f;


        private void Awake()
        {
            singleton = this;
            myTransform = this.transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        public void FollowTarget(float delta)
        {
            // Vector3 targetPosition = Vector3.Lerp(myTransform.position, targetTransform.position, delta / followSpeed);
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, 
                ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition;
        } 

        public void HandleCameraRotation(float delta, float mouseX, float mouseY)
        {
            lookAngle += (mouseX * lookSpeed) / delta;
            pivotAngle -= (mouseY * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;
            rotation = Vector3.zero;
            rotation.x = pivotAngle;
            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
           
        }

        public void PlayerCamera(float delta, float mouseX, float mouseY)  // Test Function...
        {
            lookAngle += (mouseX * lookSpeed) / delta;
            pivotAngle -= (mouseY * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);
            Quaternion rotatePlayer = Quaternion.AngleAxis(lookAngle, playerAvatar.transform.up);
            myTransform.rotation = rotatePlayer;
        }
    }
}

// 
//
