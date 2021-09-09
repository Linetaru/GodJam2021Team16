using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerAttack : MonoBehaviour
{
    // Cooldown d'attaque

    [ReadOnly] public float timeBtwAttack;
    public float startBtwAttack;
    [ReadOnly] public bool night;

    // Attaque sur les ennemis 

    public float attackRange;
    public Transform attackPos;
    public LayerMask Ennemy;
    public int damage = 50;

    public bool canAnimationWorkWhenNoEnnemiesInRange;

    public Animator attackAnimation;

    public Vector3 offset;

    // Start is called before the first frame update

    [ReadOnly] [SerializeField] private int playerID = 0;
    [SerializeField] private Player player;

    private void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
    }

    // Update is called once per frame
    void Update()
    {
        if (night)
            Attack();
    }

    public void OnDayStart()
    {
        night = false;
    }

    public void OnNightStart()
    {
        night = true;
    }

    void Attack()
    {
        // Action d'attaque
        if (timeBtwAttack <= 0)
        {
            if (player.GetButton("Attack"))
            {
                if(this.transform.eulerAngles == new Vector3(0, 180, 0 ))
                    attackPos.position = this.transform.position - offset;
                else
                    attackPos.position = this.transform.position + offset;

                Collider2D[] damageEnemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, Ennemy);
                print("Grrrrrr !");

                if(damageEnemies.Length > 0 && !canAnimationWorkWhenNoEnnemiesInRange)
                    attackAnimation.SetTrigger("Attack");
                else if(canAnimationWorkWhenNoEnnemiesInRange)
                    attackAnimation.SetTrigger("Attack");

                for (int i = 0; i < damageEnemies.Length; i++)
                {
                    damageEnemies[i].GetComponent<Enemy>().TakeDamage(damage);

                }
                timeBtwAttack = startBtwAttack;
            }
        }
        else
            timeBtwAttack -= Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
