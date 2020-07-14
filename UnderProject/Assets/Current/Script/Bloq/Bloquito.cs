using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloquito : BaseEnemy
{
    private ZaWarudo zw;

    public int mode = 0;
    private Animator anim;
    private Rigidbody2D rb2d;
    public GameObject legs;

    private bool key1 = false;

    private bool touchingEnemy;
    private bool touchingGround;
    public LayerMask whatIsGround;
    public float wallCheckDistance;

    private SpriteRenderer sr;

    private bool activated = true;
    private int dir = 1;

    //Atk
    public float timeBtwAtk;
    // Start is called before the first frame update
    void Start()
    {
        // Za Warudo
        zw = GetComponent<ZaWarudo>();

        anim = GetComponentInChildren<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    	if(sr.flipX)
    	{
    		dir = 1;
    	}
    	else
    	{
    		dir = -1;
    	}

        modeManager();
        touchingEnemy = Physics2D.Raycast(transform.position, transform.up, wallCheckDistance, whatIsEnemy);
        touchingGround = Physics2D.Raycast(transform.position, transform.up, -wallCheckDistance*2, whatIsGround);

        Debug.Log(touchingEnemy);

        if (touchingEnemy && activated)
        {
        	activated = false;
            StartCoroutine(WaitDown());
        }

        if(touchingGround && mode != 2)
        {
        	transform.localScale += new Vector3(1,1,0);
        	rb2d.velocity = Vector2.up * 10;
        	legs.SetActive(true);
        	mode = 2;
        }

        if (rb2d.velocity.x < -1)
        {
            sr.flipX = false;
            anim.SetBool("SeeingPlayer", true);
        }
        else if (rb2d.velocity.x > 1)
        {
            sr.flipX = true;
            anim.SetBool("SeeingPlayer", true);
        }
        else
        {
            anim.SetBool("SeeingPlayer", false);
        }

        timeBtwAtk -= Time.deltaTime;
    }

    private void modeManager()
    {
        if (zw.getIsTimeStopped() == false)
        {
            if (mode == 1 && key1 == false)
            {
                key1 = true;
                DownMode();
            }

            if (mode == 2)
            {
                GroundSeeker();

                AttackTimer("circle");
            }
        }  
    }

    // public void setMode(int aux)
    // {
    //     mode = aux;
    // }

    private void DownMode()
    {
        anim.SetTrigger("GetDown");
        //StartCoroutine(WaitLegs());
    }

    IEnumerator WaitDown()
    {
    	yield return new WaitForSeconds(1f);
    	transform.localScale -= new Vector3(1,1,0);
    	rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    	CircleAttack(damage);
    	rb2d.velocity = Vector2.up * 15;
        legs.SetActive(true);
        mode = 1;
    }

    IEnumerator WaitLegs()
    {
        yield return new WaitForSeconds(3f);
    }

    // Attack
    protected new void CircleAttack(int dmg)
    {
        Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(transform.position + transform.right * 1.5f * dir, new Vector3(1, 1, 1), 90, whatIsEnemy);
        if (enemiesToHit.Length > 0)
        {
            //ps.PlayParticle();
            for (int i = 0; i < enemiesToHit.Length; i++)
            {
                enemiesToHit[i].GetComponent<Playerlifes>().PlayerTakeDamage(dmg);
                Debug.Log("atak");
            }
        }
    }

    protected new void AttackTimer(string mode)
    {
        if (timeBtwAtk <= 0)
        {
            if (mode == "circle")
            {
                timeBtwAtk = startTimeBtwAtkCircle;
                CircleAttack(damage);
            }
        }
    }

    // Draw attack range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(transform.position * 2 * dir, circleAttackRadius);
        Gizmos.DrawCube(transform.position + transform.right * 1.5f, new Vector3(1, 1, 1));
    }
}
