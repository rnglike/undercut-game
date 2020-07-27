using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
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

    
    public Transform pickablePlace;
    public bool IsPicking;

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
        if (Input.GetButtonDown("Hit") && playerLock == false && timeBtwAttack <= 0)
        {
            float temp_mi = transform.GetComponent<PlayerController>().moveInput;
            float temp_dir;
            if((temp_mi) != 0)
            {
                temp_dir = (temp_mi/Mathf.Abs(temp_mi));
            }
            else
            {
                temp_dir = 0;
            }

            playerAnimator.SetTrigger("Attack");
            
            transform.GetComponent<PlayerController>().rb.velocity = new Vector3(temp_dir * 20,transform.GetComponent<PlayerController>().rb.velocity.y);
            
            // Attack delay
            timeBtwAttack = attackDelay*(Time.deltaTime*60);
            // StartCoroutine(Wait());
            Attack();
        }
        else 
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }


    // Drawing the attack range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPosition.position, new Vector3(attackRangeX, attackRangeY, 1));
    }

    // Delay function
    IEnumerator Wait() {
        // Maybe put an delay, if necessary
        timeBtwAttack = defaultTimeBtwAttack;

        yield return new WaitForSeconds (attackDelay);

        Attack();
    }

    private void Attack() {
        Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(attackPosition.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemy);
        if(enemiesToHit.Length > 0)
        {
            for (int i = 0; i < enemiesToHit.Length; i++)
            {
                if(enemiesToHit[i].gameObject.layer == 12 && GetComponent<PlayerItems>().GetHairCode() == 3)
                {
                    enemiesToHit[i].GetComponent<Breakable>().TakeDamage();
                }
                else
                {
                    if(enemiesToHit[i].name == "calder(Clone)") enemiesToHit[i].GetComponent<EnemyHealth2>().TakeDamage(damage);
                    if(enemiesToHit[i].name == "bomb(Clone)" || enemiesToHit[i].name == "imao(Clone)") enemiesToHit[i].GetComponent<EnemyHealth>().TakeDamage(damage);
                }
            }
        }
        Debug.Log("Dealing tons of DAMAGE");
    }

    private void Pick() {
        Collider2D pickableEnemy = Physics2D.OverlapBox(attackPosition.position, new Vector2(attackRangeX,attackRangeY),0,whatIsEnemy);
        if(!IsPicking)
        {
            if(pickableEnemy != null)
            {
            	bool pickable = pickableEnemy.GetComponent<Pickable>().status;

                if(pickable)
                {
                    IsPicking = true;
                    pickableEnemy.GetComponent<Pickable>().picked = true;
                }
            }
            else
            {
                Debug.Log("jfdsjsd");
            }
        }
        else
        {
            IsPicking = false;
        }
    }
}
