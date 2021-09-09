using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunBehavior : StateMachineBehaviour
{
    private Transform playerPos;
    [SerializeField] private float actualRunningTimer;

    private GameObject moveSpotsList;
    private Transform[] moveSpots;
    private Transform farthestSpot;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moveSpotsList = animator.GetComponentInParent<EnemyTriggerController>().moveSpots;
        moveSpots = moveSpotsList.GetComponentsInChildren<Transform>();
        farthestSpot = moveSpots[0];
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i< moveSpots.Length; i++)
        {
            if (Vector2.Distance(playerPos.position, moveSpots[i].position) >= Vector2.Distance(playerPos.position, farthestSpot.position))
                farthestSpot = moveSpots[i];
        }
        
        actualRunningTimer = animator.GetFloat("RunningDuration");
        animator.GetComponentInParent<Pathfinding.AIDestinationSetter>().target = farthestSpot;
        animator.GetComponentInParent<Pathfinding.AIPath>().maxSpeed = 10; //animator.GetFloat("actualSpeed");

    }

    

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInParent<Pathfinding.AIDestinationSetter>().target = null;
    }


}
