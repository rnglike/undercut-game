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

    public string type;

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

    //Stop the animation
    public Animator anim;

    public brain brain;


    void Start()
    {
        if(type == "obj") rb = GetComponent<Rigidbody2D>();
        if(type == "brain") brain = GetComponent<brain>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        // Check the player player hair
        playerHair = player.GetComponent<PlayerItems>().GetHairCode();
        if (playerHair == 1) {

            // Call the time stopper
            if (Input.GetKeyDown("z") && isTimeStopped == false && cooldown <= 0) {
                if(type == "brain")
                {
                    brain.timerPaused = true;
                    brain.theme.pitch = .5f;
                }
                StopTheTime();
            }
        }
        if (isTimeStopped == true) {
            // Call freeze in each frame until duration be equals 0 or less
            Freeze();
            if (duration <= 0) {
                if(type == "brain")
                {
                    brain.timerPaused = false;
                    brain.theme.pitch = 1f;
                }
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
        if(type == "obj")
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = new Vector2(0, 0);
            rb.velocity = freeze.rbVelocity;
        }
        
        duration -= Time.deltaTime;
    }


    // Save the object momentum
    private void Record() {
        if(type == "obj")
        {
            freeze = new PointInTime(transform.position, transform.rotation, rb.velocity);
        }
    }


    // Set the variables to the time stop
    private void StopTheTime() {
        if(type == "obj")
        {
            rb.isKinematic = true;
            anim.speed = 0f;
        }

        duration = startDuration;
        isTimeStopped = true;
    }


    // Set the variables to return the time
    private void ReturnTheTime() {
        if(type == "obj")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.isKinematic = false;
            anim.speed = 1f;
        }

        isTimeStopped = false;
        cooldown = startCooldown;
    }


    // Call it if you want know about the time
    public bool getIsTimeStopped() {
        return isTimeStopped;
    }
}
