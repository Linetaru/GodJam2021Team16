using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyFX : MonoBehaviour
{

    [SerializeField] private AIPath aiPath;
    private float actualPosX;
    private float previousPosX;
    private void Start()
    {
        actualPosX = transform.position.x;
    }
    // Update is called once per frame
    void Update()
    {
        
        previousPosX = transform.position.x;

        if ( previousPosX<= actualPosX)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        } else 
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        previousPosX = actualPosX;
    }
}
