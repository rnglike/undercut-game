using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    // Attack timer
    private float timeBtwAttack;
    public float defaultTimeBtwAttack;


    // Attack settings
    public int damage;
    public Transform attackPosition;
    public LayerMask whatIsEnemy;
    public float attackRangeX;
    public float attackRangeY;

    // Attack animation delay
    public float attackDelay;

    //Player lock
    private bool playerLock;


    // Animations
    public Animator anim;
    public float animationTime;

    private int hairCode;



    void Update()
    {
        hairCode = gameObject.GetComponentInParent<PlayerItems>().GetHairCode();
        playerLock = GetComponentInParent<PlayerController>().getPlayerLock();

        // Attack !
        if (Input.GetKey("z") && timeBtwAttack <= 0 && playerLock == false && hairCode == 5)
        {
            //animator.SetTrigger("pew", false);
            // animação de ataque
            //Attack delay
            StartCoroutine(Wait()); 

        } else 
        {
            timeBtwAttack = timeBtwAttack - Time.deltaTime;
        }
    }




    // Delay function
    IEnumerator Wait() {
        // Maybe put an delay, if necessary
        timeBtwAttack = defaultTimeBtwAttack;

        yield return new WaitForSeconds (attackDelay);

        Attack();

        yield return new WaitForSeconds (animationTime);

        
        anim.SetBool("pew", false);
    }

    private void Attack() {
        anim.SetBool("pew", true);

        Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(attackPosition.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemy);
        if(enemiesToHit.Length > 0)
        {
            for (int i = 0; i < enemiesToHit.Length; i++)
            {
                if(enemiesToHit[i].gameObject.layer == 8)
                {
                    enemiesToHit[i].GetComponent<Breakable>().TakeDamage();
                }
                else
                {
                    enemiesToHit[i].GetComponent<EnemyHealth>().TakeDamage(damage);
                }
            }
        }
        Debug.Log("Kamehamehaaaaa!");
    }


    // Drawing the attack range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPosition.position, new Vector3(attackRangeX, attackRangeY, 1));
    }
}
