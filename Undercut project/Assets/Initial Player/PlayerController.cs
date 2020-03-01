using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movimentation variables
    public float speed;
    public float jumpForce;
    private float moveInput;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool playerMovementLock = false;


    // Variables of ground check
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;


    // Multiple jumps variables
    public int defaultJumps;
    private int extraJumps;


    // Animator variable
    private Animator playerAnimator;
    // isGrounded is used here too
    private bool combateIdle = false;
    private bool isDead = false;


    void Awake()
    {
        // Assigning variables
        rb = GetComponent<Rigidbody2D>();
        extraJumps = defaultJumps;
        playerAnimator = GetComponent<Animator>();
    }

    
    void FixedUpdate()
    {
        // Horizontal movimentation
        if (playerMovementLock == false)
        {
            moveInput = Input.GetAxis("Horizontal"); // Raw? (no aceleration)
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }


        // Flip control
        if (facingRight == false && moveInput > 0)
        {
            Flip();
        } else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }


        // There is ground under the player?
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }


    void Update()
    {

        // Jump control
        if (isGrounded == true && rb.velocity.y < 0.5)
        {
            extraJumps = defaultJumps;
            playerAnimator.SetBool("Grounded", isGrounded);
        } else
        {
            isGrounded = false;
            playerAnimator.SetBool("Grounded", isGrounded);
        }


        // For call jump anim while the player is just falling
        playerAnimator.SetFloat("AirSpeed", rb.velocity.y);

        if (Input.GetKeyDown("space") && extraJumps > 0 && playerMovementLock == false)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
            playerAnimator.SetTrigger("Jump");
            playerAnimator.SetBool("Grounded", isGrounded);
        }
        // Running animation
        else if (Mathf.Abs(moveInput) > Mathf.Epsilon && playerMovementLock == false)
        {
            playerAnimator.SetInteger("AnimState", 2);
        }
        // IDLE animation
        else
        {
            playerAnimator.SetInteger("AnimState", 0);
        }

        // Test block
        if (Input.GetKeyDown("p"))
        {
            LockThePlayer();
        }
    }


    // Flip the game object
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    // Lock the player momentation
    public void LockThePlayer()
    {
        playerMovementLock = !playerMovementLock;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
