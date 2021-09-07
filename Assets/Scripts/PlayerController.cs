using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{

    public float speed = 7f;

    [SerializeField] private int playerID = 0;
    [SerializeField] private Player player;

    private void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(player.GetAxis("HorizontalMove"), player.GetAxis("VerticalMove"), 0f);
        transform.position += movement * Time.deltaTime * speed;

    }
}
