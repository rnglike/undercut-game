using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZaWarudo : MonoBehaviour
{
    /*
        Lock the Game Object while Time Stop effect is online
        It must be in each Game Object who move to create the effect
        If another script want to know if the time if stopped, call getIsTimeStopped
        * Momentum of physic *
    */


    // Control variables
    PointInTime freeze;
    private bool isTimeStopped = false;
    public float startDuration;
    private float duration;
    public float startCooldown;
    private float cooldown; // Maybe, set in the code the preset
    Rigidbody2D rb;

    // Player item status
    private GameObject player;
    private int playerHair;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        // Check the player player hair
        playerHair = player.GetComponent<PlayerItems>().GetHairCode();
        if (playerHair == 1) {

            // Call the time stopper
            if (Input.GetKeyDown(KeyCode.Return) && isTimeStopped == false && cooldown <= 0) {
                StopTheTime();
            }
        }
        if (isTimeStopped == true) {
            // Call freeze in each frame until duration be equals 0 or less
            Freeze();
            if (duration <= 0) {
                ReturnTheTime();
            }
        }

        cooldown -= Time.deltaTime;
    }


    void FixedUpdate() {
        // While the time runs, call record
        if (!isTimeStopped) {
            Record();
        }
    }


    // It will reload the last object momentum before the time stop
    private void Freeze() {
        transform.position = freeze.position;
        transform.rotation = freeze.rotation;
        rb.velocity = new Vector2(0, 0);
        rb.velocity = freeze.rbVelocity;
        
        duration -= Time.deltaTime;
    }


    // Save the object momentum
    private void Record() {
        freeze = new PointInTime(transform.position, transform.rotation, rb.velocity);
    }


    // Set the variables to the time stop
    private void StopTheTime() {
        duration = startDuration;
        isTimeStopped = true;
        rb.isKinematic = true;
    }


    // Set the variables to return the time
    private void ReturnTheTime() {
        isTimeStopped = false;
        rb.isKinematic = false;
        cooldown = startCooldown;
    }


    // Call it if you want know about the time
    public bool getIsTimeStopped() {
        return isTimeStopped;
    }
}
