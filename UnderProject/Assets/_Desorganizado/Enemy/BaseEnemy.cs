using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    /*
     * ----------------------------------------------------------------------------------------
     *                                        ZA WARUDO
     * ----------------------------------------------------------------------------------------
    */
    private ZaWarudo zw;


    /*
     * ----------------------------------------------------------------------------------------
     *                                      GROUND SEEKER
     * ----------------------------------------------------------------------------------------
    */
    // Movimentation variables
    public GameObject player;
    public float standardSpeed;
    private float currentSpeed;
    private Rigidbody2D rb;
    private Vector2 direction;
    // Range of vision
    public float visionRange;


    /*
     * ----------------------------------------------------------------------------------------
     *                                          ATTACK
     * ----------------------------------------------------------------------------------------
    */
    public int damage;
    public LayerMask whatIsEnemy;
    private float timeBtwAtk = 0f;
    private ParticleScript ps;
    // Circle
    public float circleAttackRadius;
    public float startTimeBtwAtkCircle;



    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // Za Warudo
        zw = GetComponent<ZaWarudo>();

        // Mov
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = standardSpeed;

        // Atk
        ps = GetComponentInChildren<ParticleScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (zw.getIsTimeStopped() == false)
        {
            GroundSeeker();

            AttackTimer("circle");

            timeBtwAtk -= Time.deltaTime;
        }
    }




















    /*
     * ----------------------------------------------------------------------------------------
     *                                      GROUND SEEKER
     * ----------------------------------------------------------------------------------------
    */
    protected void GroundSeeker()
    {
        direction = (Vector2)(player.transform.position - transform.position);
        if (direction.magnitude <= visionRange)
        {
            direction = direction.normalized;
            rb.velocity = new Vector2(direction.x * currentSpeed, rb.velocity.y);
        }
    }



















    /*
     * ----------------------------------------------------------------------------------------
     *                                          ATTACK
     * ----------------------------------------------------------------------------------------
    */
    protected void CircleAttack(int dmg)
    {
        Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll((Vector2)transform.position, circleAttackRadius, whatIsEnemy);
        if (enemiesToHit.Length > 0)
        {
            ps.PlayParticle();
            for (int i = 0; i < enemiesToHit.Length; i++)
            {
                enemiesToHit[i].GetComponent<Playerlifes>().PlayerTakeDamage(dmg);
            }
        }
    }

    protected void AttackTimer(string mode)
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleAttackRadius);
    }
}
