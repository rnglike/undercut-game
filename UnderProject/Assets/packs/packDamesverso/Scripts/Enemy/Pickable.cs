using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{ 
    Transform player;
    Rigidbody2D rb;

    public bool status;
    public bool picked;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
    	if(!player.GetComponent<PlayerAttack>().IsPicking && picked)
        {
            picked = false;
        }

        if(picked)
        {
            transform.position = player.GetComponent<PlayerAttack>().pickablePlace.position;
            if(player.GetComponent<Rigidbody2D>().velocity.normalized.y != -1)
            {
                rb.velocity = (player.GetComponent<Rigidbody2D>().velocity.normalized*30);
            }
        }
        
    }
}
