using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerlifes : MonoBehaviour
{

    // Player Health manager script

    public int lifes;
    private bool playerLock;


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
            Destroy(gameObject); // Or call something
        }
    }


    // Function player take damage
    public void PlayerTakeDamage(int damage) {
        playerLock = GetComponent<PlayerController>().getPlayerLock();
        lifes -= damage;
        // When the player take damage, his movimentation will be locked.
        if (playerLock == false) {
            GetComponent<PlayerController>().LockThePlayer();
            // Hurt player animation
            // Animation time
            GetComponent<PlayerController>().LockThePlayer();
        }
    }
}
