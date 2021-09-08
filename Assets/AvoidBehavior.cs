using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidBehavior : StateMachineBehaviour
{
    private GameObject playerPos;
    private float actualSpeed;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInParent<Pathfinding.AIDestinationSetter>().target = null;
        actualSpeed = animator.GetFloat("actualSpeed");
        playerPos = GameObject.FindGameObjectWithTag("Player");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector2.MoveTowards(animator.transform.position,new Vector2((animator.transform.position.x -playerPos.transform.position.x)*1000, (animator.transform.position.y - playerPos.transform.position.y)*1000), actualSpeed * Time.deltaTime);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

}
