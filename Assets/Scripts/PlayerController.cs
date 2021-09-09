using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using PackageCreator.Event;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerController : MonoBehaviour
{

    [Header("Player Settings")]
    [SerializeField] private float playerMaxHealth = 1f;
    

    public float moveSpeed = 7f;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private int playerID = 0;
    [SerializeField] private Player player;

    [SerializeField] private GameEvent onDeath;
    [SerializeField] private Animation deathAnimation;

    [SerializeField] private Light2D playerLight;

    private float _currentHealth;

    private void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
        _currentHealth = playerMaxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void PlayerDie()
    {
        this.GetComponent<Animator>().SetTrigger("Dead");
    }

    private void FixedUpdate()
    {
        // Mouvements 
        if (!isDead())
        {
            Vector3 movement = new Vector3(-player.GetAxis("HorizontalMove"), player.GetAxis("VerticalMove"), 0f);
            if (movement.sqrMagnitude > 1.0f) movement.Normalize();
            transform.position += movement * Time.deltaTime * moveSpeed;

        Vector3 movement = new Vector3(-player.GetAxis("HorizontalMove"), player.GetAxis("VerticalMove"), 0f);
        if(movement != Vector3.zero && !this.GetComponent<Animator>().GetBool("isWalking"))
        {
            this.GetComponent<Animator>().SetBool("isWalking", true);
            this.GetComponent<Animator>().SetBool("isIdle", false);
        }
        else if(movement == Vector3.zero && this.GetComponent<Animator>().GetBool("isWalking"))
        {

            this.GetComponent<Animator>().SetBool("isIdle", true);
            this.GetComponent<Animator>().SetBool("isWalking", false);
        }
        if (movement.sqrMagnitude > 1.0f) movement.Normalize();
        transform.position += movement * Time.deltaTime * moveSpeed;

            Vector3 characterScale = transform.localScale;
            if (player.GetAxis("HorizontalMove") > 0)
            {
                characterScale.x = -7;
            }

        Quaternion characterScale = transform.rotation;
        if(player.GetAxis("HorizontalMove") > 0)
        {
            characterScale.y = 180;
        }
    }

    public bool isDead()
    {
        return _currentHealth <= 0;
    }

    public void OnDayStart()
    {
        InvokeRepeating("ReduceLampLight", 0, 0.01f);
    }

    public void OnNightStart()
    {
        InvokeRepeating("AugmentLampLight", 0, 0.01f);
    }

    void ReduceLampLight()
    {
        playerLight.intensity -= 0.01f;
        if (playerLight.intensity <= 0)
        {
            characterScale.y = 0;
        }
    }

        transform.rotation = characterScale;
    }
}
