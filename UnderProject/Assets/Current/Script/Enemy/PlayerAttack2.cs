using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack2 : MonoBehaviour
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
    private Animator playerAnimator;


    void Start()
    {
        // Geting animator
        playerAnimator = GetComponent<Animator>();
    }


    void Update()
    {
        // Is player locked?
        playerLock = GetComponent<PlayerController>().getPlayerLock();

        // Attack !
        if (Input.GetKey("space") && timeBtwAttack <= 0 && playerLock == false)
        {
            timeBtwAttack = defaultTimeBtwAttack;
            playerAnimator.SetTrigger("Attack");
            //Attack delay
            StartCoroutine(Wait());

        }
        else
        {
            timeBtwAttack = timeBtwAttack - Time.deltaTime;
        }
    }


    // Drawing the attack range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPosition.position, new Vector3(attackRangeX, attackRangeY, 1));
    }

    // Delay function
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(attackDelay);
        Attack();
    }

    private void Attack()
    {
        Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(attackPosition.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemy);
        for (int i = 0; i < enemiesToHit.Length; i++)
        {
            enemiesToHit[i].GetComponent<EnemyHealth2>().TakeDamage(damage);
        }
        Debug.Log("Dealing tons of DAMAGE");
    }
}