using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 7f;

    public Rigidbody2D rb;

    [SerializeField] private int playerID = 0;
    [SerializeField] private Player player;

    private void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
    }

    // Update is called once per frame
    private void Update()
    {
        

    }

    private void FixedUpdate()
    {
        // Mouvements 

        Vector3 movement = new Vector3(player.GetAxis("HorizontalMove"), player.GetAxis("VerticalMove"), 0f);
        if (movement.sqrMagnitude > 1.0f) movement.Normalize();
        transform.position += movement * Time.deltaTime * moveSpeed;

        // Flip character

        Vector3 characterScale = transform.localScale;
        if(player.GetAxis("HorizontalMove") < 0)
        {
            characterScale.x = -7;
        }

        if (player.GetAxis("HorizontalMove") > 0)
        {
            characterScale.x = 7;
        }

        transform.localScale = characterScale;
    }
}
