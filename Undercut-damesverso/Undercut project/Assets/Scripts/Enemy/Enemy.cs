using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float startSpeed;
    private float speed;
    public float distanceVision;

    public Transform enemyGFX;

    private Transform transformE;
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 move;

    private float dazedTime;
    public float startDazedTime;
    //private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        //anim.SetBool("isRunning", true);
        //transformE = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dazedTime <= 0 )
        {
            speed = startSpeed;
        } else
        {
            speed = 0;
            dazedTime -= Time.deltaTime;
        }
        //transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (Vector2.Distance(rb.position, player.position) <= distanceVision)
        {
            // move = Vector2.MoveTowards(transformE.position, player.position, speed * Time.deltaTime);
            move = ((Vector2)player.position - rb.position);
            move = move / move.magnitude;
            rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
    
    
    }

    public void TakeDamage(int damage)
    {
        dazedTime = startDazedTime;
        health -= damage;
        Debug.Log("DANO!");
    }
}
