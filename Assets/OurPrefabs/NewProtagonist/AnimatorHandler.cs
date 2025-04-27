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
        float horizontalAmount = horizontalMovement;
        float verticalAmount = verticalMovement;

        if (playerManager.isSprinting)
        {
            verticalAmount = 2;
        }

        anim.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
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
        // apply custom movement 
        if(!playerManager.canMove)
        {
            if (playerManager.isSprinting)
            {
                playerManager.characterController.Move(playerMovement.moveDirection * (playerMovement.sprintingSpeed + 10) * Time.deltaTime);
            }
            else 
            {
                playerManager.characterController.Move(playerMovement.moveDirection * (playerMovement.walkingSpeed + 5) * Time.deltaTime);
            }
            
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
