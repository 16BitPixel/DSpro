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
            // This block covers the if/else statements for vertical movements.
            float absVert = Mathf.Abs(verticalMovement) * 1f;
            float vSign = verticalMovement < 0 ? -1f : 1f;
            v = absVert == 0? 0f : absVert > 0 && absVert < 0.55f ? 0.5f : 1f;
            v *= vSign;
            // End block

           
            #endregion

            #region Horizontal 
            // This block covers the if/else for horizontal movements
            float h = 0;
            float absHz = Mathf.Abs(horizontalMovement);
            float hzSign = horizontalMovement < 0 ? -1 : 1;
            h = absHz == 0? 0f : absHz > 0 && absHz < 0.55f ? 0.5f : 1f;
            h *= hzSign;
            // End Block

           
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

        public void PlayerAttackAnimation(bool result)
        {
            anim.applyRootMotion = result;
            anim.SetBool("attacking", result);
            anim.CrossFade("Attack", 0.2f);
            StartCoroutine(delay());
        }

        IEnumerator delay()
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            anim.SetBool("attacking", false);
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
