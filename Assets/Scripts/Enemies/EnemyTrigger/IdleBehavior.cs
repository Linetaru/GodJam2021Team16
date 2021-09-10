using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    private float idleDuration;
    [SerializeField] private GameObject searchingParticle;
    [SerializeField] private GameObject interrogationParticle;

    private bool _isParticleSpawned;
    private GameObject particle;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EnemyTriggerController controller = animator.GetComponentInParent<EnemyTriggerController>();
        animator.SetFloat("idleDuration", controller.stoppingTimeAfterChase);
        idleDuration = animator.GetFloat("idleDuration");

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_isParticleSpawned)
        {
            particle = Instantiate(searchingParticle, new Vector3(animator.transform.position.x, animator.transform.position.y + 3.2f, 2), animator.transform.rotation);
            particle.transform.SetParent(animator.transform);
            _isParticleSpawned = true;
        }
        if (idleDuration > 0)
        {
            idleDuration -= Time.deltaTime;
        }
        else
        {
            animator.SetBool("mustIdle", false);
            animator.SetBool("isPatrolling", true);
            Destroy(particle);
            _isParticleSpawned = false;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(particle);
        _isParticleSpawned = false;
        GameObject particleInterrogation = Instantiate(interrogationParticle, new Vector3(animator.transform.position.x, animator.transform.position.y + 3.2f, 2), animator.transform.rotation);
        particleInterrogation.transform.SetParent(animator.transform);
        Destroy(particleInterrogation, 5);
    }

}
