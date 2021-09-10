using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTourelleController : MonoBehaviour
{
    [SerializeField] private float powerDash;
    [SerializeField] private float triggerRange;

    [SerializeField] public float stoppingTimeAfterDash;

    [SerializeField] public ParticleSystem dashEffect;

    Collider2D playerDetection;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private LayerMask layerWallMask;

    [SerializeField] private float damagePlayerOnHit = 50f;

    [SerializeField] private GameObject detectPlayerParticle;

    private bool isPlayerClipped;
    

    [ReadOnly] public bool isInFear;

    private Animator enemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInFear) return;

        int layerMaskRay = 1 << LayerMask.NameToLayer("EnnemyTurret");
        layerMaskRay = ~layerMaskRay;

        playerDetection = Physics2D.OverlapCircle(transform.position, triggerRange, layerMask);
        if (!(playerDetection is null))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(playerDetection.transform.position - transform.position), 500, layerMaskRay);
            Debug.DrawRay(transform.position, transform.TransformDirection(playerDetection.transform.position - transform.position), Color.yellow);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                return;
            else
                if (!enemyAnimator.GetComponent<Animator>().GetBool("isDashing") && !enemyAnimator.GetBool("mustIdle"))
                {
                    enemyAnimator.GetComponent<Animator>().SetBool("isDashing", true);
                    enemyAnimator.SetFloat("powerDash", powerDash);
                    GameObject particle = Instantiate(detectPlayerParticle, new Vector3(transform.position.x, transform.position.y + 3.2f, 2), transform.rotation);
                    particle.transform.SetParent(transform);
                    Destroy(particle, 5);

            }

            if(Vector2.Distance(playerDetection.transform.position, this.transform.position) <= 2 && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player") && !isPlayerClipped)
            {
                isPlayerClipped = true;
                Debug.Log("EnnemyTourelle à dit : Joueur Touché.");
                PlayerController player = hit.collider.gameObject.GetComponent<PlayerController>();
                player.Damage(damagePlayerOnHit);
            }
            else if(Vector2.Distance(playerDetection.transform.position, this.transform.position) > 2 && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player") && isPlayerClipped)
            {
                isPlayerClipped = false;
                Debug.Log("EnnemyTourelle à dit : A survolé un joueur et fini son dash.");
            }
        }
    }

    public void OnDayStarting()
    {
        isInFear = false;

        enemyAnimator.GetComponent<Animator>().SetBool("isInFear", false);
        enemyAnimator.GetComponent<Animator>().SetBool("mustIdle", true);
    }

    public void OnNightStarting()
    {
        isInFear = true;
        enemyAnimator.GetComponent<Animator>().SetBool("isInFear", true);
        enemyAnimator.GetComponent<Animator>().SetBool("isDashing", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TestingCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TestingCollision(collision);
    }
    private void TestingCollision(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            enemyAnimator.GetComponent<Animator>().SetBool("isDashing", false);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            enemyAnimator.SetBool("mustIdle", true);
            enemyAnimator.SetBool("isDashing", false);
        }
    }

    public void DoParticule()
    {
        dashEffect.Play();
    }

    //Just some display to check detection
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }
}
