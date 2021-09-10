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

    [SerializeField] private Light2D playerLight;

    [SerializeField] private AudioClip daySteps;
    [SerializeField] private AudioClip nightSteps;
    [SerializeField] private AudioSource footsteps;

    [SerializeField] private ParticleSystem dustParticle;

    private AudioClip lastAudio; 


    private float _currentHealth;

    private void Start()
    {
        footsteps.clip = daySteps;
        player = ReInput.players.GetPlayer(playerID);
        _currentHealth = playerMaxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void PlayerDie()
    {
        this.GetComponent<Animator>().SetTrigger("Death");
        print("Player death");
        _currentHealth = 0;

        if (onDeath != null)
            onDeath.Raise();
    }

    private void FixedUpdate()
    {
        // Mouvements 
        if (!isDead())
        {
            Vector3 movement = new Vector3(-player.GetAxis("HorizontalMove"), player.GetAxis("VerticalMove"), 0f);
            if (movement.sqrMagnitude > 1.0f) movement.Normalize();
            {
                transform.position += movement * Time.deltaTime * moveSpeed;
                if (this.GetComponent<Animator>().GetBool("isWalking"))
                    dustParticle.Play();
            }

            if (movement != Vector3.zero && !this.GetComponent<Animator>().GetBool("isWalking"))
            {
                footsteps.Play();
                this.GetComponent<Animator>().SetBool("isWalking", true);
                this.GetComponent<Animator>().SetBool("isIdle", false);
            }
            else if (movement == Vector3.zero && this.GetComponent<Animator>().GetBool("isWalking"))
            {
                footsteps.Stop();
                this.GetComponent<Animator>().SetBool("isIdle", true);
                this.GetComponent<Animator>().SetBool("isWalking", false);
            }

            Quaternion characterScale = transform.rotation;
            if (player.GetAxis("HorizontalMove") < 0)
                characterScale.y = 0;

            if (player.GetAxis("HorizontalMove") > 0)
                characterScale.y = 180;

            transform.rotation = characterScale;
        }
    }

    public void Damage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            PlayerDie();
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

    public void changeDayFoosteps()
    {
        footsteps.Stop();
        footsteps.clip = daySteps;
        footsteps.Play();
        
    }

    public void changeNightFootsteps()
    {
        footsteps.Stop();
        footsteps.clip = nightSteps;
        footsteps.Play();
    }

    void ReduceLampLight()
    {
        playerLight.intensity -= 0.01f;
        if (playerLight.intensity <= 0)
        {
            CancelInvoke("ReduceLampLight");
            playerLight.intensity = 0;
        }
    }

    void AugmentLampLight()
    {
        playerLight.intensity += 0.01f;
        if (playerLight.intensity >= 1)
        {
            CancelInvoke("AugmentLampLight");
            playerLight.intensity = 1;
        }
    }
}
