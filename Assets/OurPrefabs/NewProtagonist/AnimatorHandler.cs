using System;
using Unity.VisualScripting;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    public Animator anim;
    public PlayerManager playerManager;

    PlayerMovement playerMovement;
    int vertical;
    int horizontal;
    public bool canRotate;

    public void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Initialize()
    {
        anim = GetComponent<Animator>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
    {
        // #region vertical
        // float v = 0;

        // if (verticalMovement > 0 && verticalMovement < 0.55f)
        // {
        //     v = 0.5f;
        // }
        // else if (verticalMovement > 0.55f)
        // {
        //     v = 1;
        // }
        // else if (verticalMovement < 0 && verticalMovement > -0.55f)
        // {
        //     v = -0.5f;
        // }
        // else if (verticalMovement < -0.55f)
        // {
        //     v = -1;
        // }
        // else 
        // {
        //     v = 0;
        // }
        // #endregion
        // #region horizontal
        // float h = 0;

        // if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        // {
        //     h = 0.5f;
        // }
        // else if (horizontalMovement > 0.55f)
        // {
        //     h = 1;
        // }
        // else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        // {
        //     h = -0.5f;
        // }
        // else if (horizontalMovement < -0.55f)
        // {
        //     h = -1;
        // }
        // else 
        // {
        //     h = 0;
        // }
        // #endregion

        anim.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
    }

    public void PlayerTargetActionAnimation(String targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canMove = false, bool canRotate = false)
    {
        anim.applyRootMotion = applyRootMotion;
        anim.CrossFade(targetAnimation, 0.2f);
        playerManager.isPerformingAction = isPerformingAction;
        playerManager.canMove = canMove;
        playerManager.canRotate = canRotate;
    }

    private void OnAnimatorMove()
    {
        if(playerManager.isPerformingAction)
        {
            playerManager.characterController.Move(playerMovement.moveDirection * 10 * Time.deltaTime);
        }
    }

    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }
}
