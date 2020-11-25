using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DS
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator anim;
        int vertical, horizontal;
        public bool canRotate;
        public InputHandler inputHandler;
        public PlayerLocomotion playerLoca;

        public void init()
        {
            anim = GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
            inputHandler = GetComponentInParent<InputHandler>();
            playerLoca = GetComponentInParent<PlayerLocomotion>();
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
        {
            #region Vertical
            float v = 0;

            if(verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if(verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if(verticalMovement <0 && verticalMovement > -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal 
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotate()
        {
            canRotate = false;
        }

        public void PlayerTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }

        private void OnAnimatorMove()
        {
            if (inputHandler.isInteracting == false) return;

            float delta = Time.deltaTime;
            playerLoca.RB.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLoca.RB.velocity = velocity;
        }
    }
}
