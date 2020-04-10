using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOld : MonoBehaviour
{
    // General variables
    public int health;
    public float startSpeed;
    private float speed;
    public float distanceVision;

    // Player detection and movementation control
    private Transform transformE;
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 move;

    // Dazed time
    private float dazedTime;
    public float startDazedTime;


    void Start()
    {
        // Getting components
        transformE = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        if (dazedTime <= 0 )
        {
            speed = startSpeed;
        } else
        {
            speed = 0;
            dazedTime -= Time.deltaTime;
        }
        //transform.Translate(move.x * speed * Time.deltaTime);
        if (Vector2.Distance(transformE.position, player.position) <= distanceVision)
        {
            //move = Vector2.MoveTowards(player.position, transformE.position, speed * Time.deltaTime); Legacy
            transform.Translate(move * Time.deltaTime);
            move = player.position - transformE.position;
            move = move / move.magnitude;
            rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }


    // Taking damage
    public void takeDamage (int damage)
    {
        dazedTime = startDazedTime;
        health -= damage;
        Debug.Log("TAKEN!");
    }
}