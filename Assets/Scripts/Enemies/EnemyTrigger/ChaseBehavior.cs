using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehavior : StateMachineBehaviour
{

    private Transform playerPos;
    [SerializeField] private float actualChasingTimer;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        actualChasingTimer = animator.GetFloat("chasingDuration");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        actualChasingTimer -= Time.deltaTime;
        
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, playerPos.position, animator.GetFloat("actualSpeed") * Time.deltaTime);
        
        if (actualChasingTimer <= 0)
        {
            animator.SetBool("mustIdle", true);
            animator.SetBool("isChasing", false);
            
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

  

   
}
