using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal, vertical;
        public float mouseX, mouseY;
        public float moveAmount;
        public bool b_input;
        public bool rollFlag;
        public bool isInteracting;

        PlayerControls inputActions;
        CameraHandler cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime; 

            if(cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta, mouseX);
                cameraHandler.HandleCameraRotationVertical(delta, mouseX, mouseY);
            }
        }

        public void OnEnable()
        {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Newaction.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void tickInput(float delta)
        {
            moveInput(delta);
            HandleRollInput(delta);
        }

        private void moveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            b_input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (b_input)
            {
                rollFlag = true;
            }
        }

    }

}
