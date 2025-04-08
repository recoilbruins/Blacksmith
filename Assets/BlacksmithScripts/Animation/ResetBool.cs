using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public string isAnimationLockedBool;
    public bool isAnimationLockedStatus;

    public string isUsingRootMotionBool;
    public bool isUsingRootMotionStatus;

    public string isDodgingBool;
    public bool isDodgingStatus;

    public string isHitBool;
    public bool isHitStatus;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isAnimationLockedBool, isAnimationLockedStatus);
        animator.SetBool(isUsingRootMotionBool, isUsingRootMotionStatus);
        animator.SetBool(isDodgingBool, isDodgingStatus);
        animator.SetBool(isHitBool, isHitStatus);
    }
}
