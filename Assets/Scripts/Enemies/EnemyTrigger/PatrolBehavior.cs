using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : StateMachineBehaviour
{
    private GameObject moveSpotsList;
    private Transform[] moveSpots;
    private int actualSpot;
    private float patrolSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moveSpotsList = animator.GetComponentInParent<EnemyTriggerController>().moveSpots;
        moveSpots = moveSpotsList.GetComponentsInChildren<Transform>();
        actualSpot = animator.GetComponentInParent<EnemyTriggerController>().actualSpot;
        patrolSpeed = animator.GetComponentInParent<EnemyTriggerController>().patrolSpeed;
        animator.GetComponentInParent<Pathfinding.AIPath>().maxSpeed = patrolSpeed;
        animator.GetComponentInParent<Pathfinding.AIDestinationSetter>().target = moveSpots[actualSpot];
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (actualSpot >= moveSpots.Length) actualSpot = 0;

        animator.GetComponentInParent<EnemyTriggerController>().actualSpot = actualSpot;

        animator.GetComponentInParent<Pathfinding.AIDestinationSetter>().target = moveSpots[actualSpot];

        if (Vector2.Distance(animator.transform.position, moveSpots[actualSpot].position) < 0.2f)
        {
            actualSpot += 1;
        }
       
    }

    
}
