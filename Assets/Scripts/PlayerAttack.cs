using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerAttack : MonoBehaviour
{
    // Cooldown d'attaque

    public float timeBtwAttack;
    public float startBtwAttack;

    // Attaque sur les ennemis 

    public float attackRange;
    public Transform attackPos;
    public LayerMask Ennemy;
    public int damage = 50;
    // Start is called before the first frame update

    [SerializeField] private int playerID = 0;
    [SerializeField] private Player player;

    private void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
    }
    

    // Update is called once per frame
    void Update()
    {
        
        Attack();
    }

    void Attack()
    {
        // Action d'attaque
        if (timeBtwAttack <= 0)
        {
            if (player.GetButton("Attack"))
            {
                Collider2D[] damageEnemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, Ennemy);
                print("Grrrrrr !");

                for (int i = 0; i < damageEnemies.Length; i++)
                {
                    damageEnemies[i].GetComponent<Enemy>().TakeDamage(damage);
                }

                timeBtwAttack = startBtwAttack;
            }


        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
