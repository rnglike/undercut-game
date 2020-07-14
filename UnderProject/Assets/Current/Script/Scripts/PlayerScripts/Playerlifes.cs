using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerlifes : MonoBehaviour
{

    // Player Health manager script

    public int lifeAmount;
    public int lifes;
    private bool playerLock;
    private Rigidbody2D rb;

    public brain brain;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lifes = lifeAmount;
    }

    void Update()
    {
        // Kill the player
        if (lifes <= 0) {
            if (playerLock == false) {
                GetComponent<PlayerController>().LockThePlayer();
                // Die player animation
                // Animation time
                GetComponent<PlayerController>().LockThePlayer();
            }            
            brain.Over(); // Or call something
        }
    }


    // Function player take damage
    public void PlayerTakeDamage(int damage) {
        // playerLock = GetComponent<PlayerController>().getPlayerLock();
        rb.AddForce((transform.up + transform.right) * 10 * 100);
        lifes -= damage;
        // // When the player take damage, his movimentation will be locked.
        // if (playerLock == false) {
        //     GetComponent<PlayerController>().LockThePlayer();
        //     // Hurt player animation
        //     // Animation time
        //     GetComponent<PlayerController>().LockThePlayer();
        // }
    }

    public void ResetLifes()
    {
        lifes = lifeAmount;
    }
}
