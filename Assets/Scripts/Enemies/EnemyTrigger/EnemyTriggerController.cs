using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerController : MonoBehaviour
{
    //Different speed variables 
    [SerializeField] private float actualSpeed;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float runSpeed;

    //Aggro range
    [SerializeField] private float triggerRange;

    //Different action durations
    [SerializeField] private float chasingDuration;
    [SerializeField] private float runningDuration;

    //Cooldown between actions
    [SerializeField] public float stoppingTimeAfterChase;
    [SerializeField] private float stoppingTimeAfterRun;

    //Layer detection mask
    [SerializeField] private LayerMask layerMask;

    //Animator link
    private Animator enemyAnimator;

    //Patrol spots
    public Transform[] moveSpots;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = transform.GetComponent<Animator>();
        enemyAnimator.SetFloat("chasingDuration", chasingDuration);
        enemyAnimator.SetFloat("idleDuration", stoppingTimeAfterChase);

    }

    // Update is called once per frame
    void Update()﻿
    {
        // Détection du joueur


        Collider2D playerDetection = Physics2D.OverlapCircle(transform.position, triggerRange, layerMask);
        if (!(playerDetection is null))
        {
            if (!transform.GetComponent<Animator>().GetBool("isChasing") && !enemyAnimator.GetBool("mustIdle"))
            {
                enemyAnimator.SetFloat("actualSpeed", actualSpeed);

                transform.GetComponent<Animator>().SetBool("isChasing", true); 
            }
        
        }
        else
        {
            transform.GetComponent<Animator>().SetBool("isChasing", false);
        }

    }

    //Just some display to check detection
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }
}
