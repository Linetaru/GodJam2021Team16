using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTourelleBehavior : StateMachineBehaviour
{

    private float idleDuration;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EnemyTourelleController controller = animator.GetComponentInParent<EnemyTourelleController>();
        animator.SetFloat("idleDuration", controller.stoppingTimeAfterDash);
        idleDuration = animator.GetFloat("idleDuration");

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (idleDuration > 0)
        {
            idleDuration -= Time.deltaTime;
        }
        else
            animator.SetBool("mustIdle", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
