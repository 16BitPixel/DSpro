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
        public Transform cameraHolderTransform;
        public Transform cameraPivotTransform;
        private Transform myTransform;

        public static CameraHandler singleton;

        private Vector3 cameraTransformPosition;
        private Vector3 cameraFollowVelocity = Vector3.zero;
        private Vector3 safeDistance;
        private LayerMask ignoreLayers;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        public float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minPivot = -30f;
        public float maxPivot = 30f;


        private void Awake()
        {
            singleton = this;
            myTransform = this.transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        private void Start()
        {
            safeDistance = cameraHolderTransform.position - targetTransform.position; 
        }

        public void FollowTarget(float delta, float mouseX) // Full code in Line 106. This is replacement. 
        {
            cameraHolderTransform.transform.position = targetTransform.position + safeDistance;
            Quaternion rot = Quaternion.AngleAxis(mouseX, targetTransform.up);
            cameraHolderTransform.transform.rotation = rot;
            safeDistance = rot * safeDistance;
            cameraHolderTransform.LookAt(targetTransform);
        } 

        public void HandleCameraRotationVertical(float delta, float mouseX, float mouseY) // handles vertical look
        {
            lookAngle += (mouseX * lookSpeed) / delta;
            pivotAngle -= (mouseY * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);


            Vector3 rotation = Vector3.zero;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            rotation = Vector3.zero;
            rotation.x = pivotAngle;
            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

    }
}

// 
//

// public void PlayerCamera(float delta, float mouseX, float mouseY)  // Test Function...
// {
//     lookAngle += (mouseX * lookSpeed) / delta;
//     pivotAngle -= (mouseY * pivotSpeed) / delta;
//     pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);
//     Quaternion rotatePlayer = Quaternion.AngleAxis(lookAngle, playerAvatar.transform.up);
//     myTransform.rotation = rotatePlayer;
// }


//public void FollowTarget(float delta) // Original follow player Function...
//{
//    // Vector3 targetPosition = Vector3.Lerp(myTransform.position, targetTransform.position, delta / followSpeed);
//    // Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, 
//    //     ref cameraFollowVelocity, delta / followSpeed);
//    // myTransform.position = targetPosition;
//}

//public void HandleCameraRotation(float delta, float mouseX, float mouseY) // Original camera Function...
//{
//    lookAngle += (mouseX * lookSpeed) / delta;
//    pivotAngle -= (mouseY * pivotSpeed) / delta;
//    pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);
//
//    Vector3 rotation = Vector3.zero;
//    rotation.y = lookAngle;
//
//    Quaternion targetRotation = Quaternion.Euler(rotation);
//    myTransform.rotation = targetRotation;
//
//    rotation = Vector3.zero;
//    rotation.x = pivotAngle;
//
//    targetRotation = Quaternion.Euler(rotation);
//    cameraPivotTransform.localRotation = targetRotation;
//   
//}
