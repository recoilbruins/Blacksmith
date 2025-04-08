using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTrigger : StateMachineBehaviour
{

    [SerializeField] private string leftAttackName = string.Empty;
    [SerializeField] private string rightAttackName = string.Empty;

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(leftAttackName);
        animator.ResetTrigger(rightAttackName);
    }
}
