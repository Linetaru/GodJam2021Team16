using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameOver : MonoBehaviour
{
    bool isDead;

    [SerializeField] private int playerID = 0;
    [SerializeField] private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {

        }
    }

    void playerIsDead()
    {
        isDead = true;

    }
}
