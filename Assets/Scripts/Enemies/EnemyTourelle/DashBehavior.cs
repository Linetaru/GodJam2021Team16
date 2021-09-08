using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBehavior : StateMachineBehaviour
{
    private Vector3 playerPos;
    private Vector3 posBeforeDash;
    EnemyTourelleController controller;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponentInParent<EnemyTourelleController>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        posBeforeDash = animator.transform.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(controller.isInFear)
            animator.SetBool("isDashing", false);

        animator.transform.position = Vector2.MoveTowards(animator.transform.position, playerPos - ((posBeforeDash - playerPos)), animator.GetFloat("powerDash") * Time.deltaTime);
        
        if(animator.transform.position == playerPos - ((posBeforeDash - playerPos)))
            animator.SetBool("isDashing", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
