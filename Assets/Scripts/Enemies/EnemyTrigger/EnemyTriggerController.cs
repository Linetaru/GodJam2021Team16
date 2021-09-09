using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerController : MonoBehaviour
{
    //Different speed variables 
    [SerializeField] private float actualSpeed;
    public float patrolSpeed;
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
    public GameObject moveSpots;
    public int actualSpot;


    private Transform playerPos;
    private float actualRunningTime;

    [SerializeField] private DayNightCycleManager dayNightCycle;
    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = transform.GetComponent<Animator>();
        enemyAnimator.SetFloat("chasingDuration", chasingDuration);
        enemyAnimator.SetFloat("idleDuration", stoppingTimeAfterChase);

        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()﻿
    {
        // Détection du joueur

        
        Collider2D playerDetection = Physics2D.OverlapCircle(transform.position, triggerRange, layerMask);
        if (!dayNightCycle.isDay())
        {
            transform.GetComponent<Animator>().SetBool("isPatrolling", false);
            transform.GetComponent<Animator>().SetBool("isChasing", false);
            transform.GetComponent<Animator>().SetBool("isRunning", true);
            if (!(playerDetection is null) && !enemyAnimator.GetBool("mustIdle")) {

                actualRunningTime = runningDuration;

                enemyAnimator.SetFloat("actualSpeed", runSpeed);

                
                transform.GetComponent<Animator>().SetBool("isRunning", false);
                transform.GetComponent<Animator>().SetBool("isAvoiding", true);
                
            }
            else
            {
                actualRunningTime -= Time.deltaTime;

                if (actualRunningTime <= 0)
                {
                    transform.GetComponent<Animator>().SetBool("isRunning", true);
                    transform.GetComponent<Animator>().SetBool("isAvoiding", false);
                }
                
            }
        }
        else if (dayNightCycle.isDay())
        {
            if (!(playerDetection is null))
            {
                    if (!transform.GetComponent<Animator>().GetBool("isChasing") && !enemyAnimator.GetBool("mustIdle"))
                    {
                        enemyAnimator.SetFloat("actualSpeed", chaseSpeed);
                        transform.GetComponent<Animator>().SetBool("isRunning", false);
                        transform.GetComponent<Animator>().SetBool("isAvoiding", false);
                        transform.GetComponent<Animator>().SetBool("isPatrolling", false);
                        transform.GetComponent<Animator>().SetBool("isChasing", true);
                    }

            }
            else
            {
                enemyAnimator.SetFloat("actualSpeed", patrolSpeed);
                transform.GetComponent<Animator>().SetBool("isPatrolling", true);
                transform.GetComponent<Animator>().SetBool("isRunning", false);
                transform.GetComponent<Animator>().SetBool("isAvoiding", false);
                transform.GetComponent<Animator>().SetBool("isChasing", false);
            }
        }

    }

    //Just some display to check detection
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }
}
