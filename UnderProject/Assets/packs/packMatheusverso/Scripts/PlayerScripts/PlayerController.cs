using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Tempo do jogador se libertar
    public float startFreeLockTime;

    // Movimentation variables
    public float speed;
    public float jumpForce;
    public float moveInput;
    private Rigidbody2D rb;
    public bool facingRight = true;
    public bool playerMovementLock;

    // Positioning Variables
    public bool withBuilding;
    public bool nextToDoor;
    public float gSb;

    // Variable of inside/outside check
    public bool inside;

    // Variables of ground check
    public bool isGrounded;
    public Transform groundCheck;
    public float checkWidth;
    public LayerMask whatIsGround;


    // Multiple jumps variables
    public int defaultJumps;
    private int extraJumps;


    // Animator variable
    private Animator playerAnimator;
    
    // isGrounded is used here too
    private bool combateIdle = false;
    private bool isDead = false;

    private Building building;

    private float rbgs;

    void Awake()
    {
        // Assigning variables
        rb = GetComponent<Rigidbody2D>();
        rbgs = rb.gravityScale;
        extraJumps = defaultJumps;
        playerAnimator = GetComponent<Animator>();
        if(withBuilding)
        {
            building = GameObject.FindGameObjectWithTag("Building").GetComponent<Building>();
        }
    }

    
    void FixedUpdate()
    {
        // Horizontal movimentation
        moveInput = Input.GetAxis("Horizontal"); // Raw? (no aceleration)

        if (playerMovementLock == false)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }

        if(moveInput == 0f || playerMovementLock)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkWidth, whatIsGround);
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(checkWidth, 0.1f), 0, whatIsGround);
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

        if (Input.GetButtonDown("Jump") && extraJumps > 0 && playerMovementLock == false)
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
        // if (Input.GetKeyDown("p"))
        // {
        //     LockThePlayer();
        // }


        //magia foda-se
        if (rb.velocity.y < 0 && PlayerItems.hairCode == 2)
        {
            rb.gravityScale = 1f;
        }
        else
        {
            rb.gravityScale = rbgs;
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
        playerMovementLock = true;
        StartCoroutine(FreeThePlayerDelay());
    }

    public void LockThePlayerAlternate()
    {
        playerMovementLock = !playerMovementLock;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public bool getPlayerLock() {
        return playerMovementLock;
    }

    // Drawing ground check box
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(checkWidth, 0.1f, 1));
    }

    // Warps player to some place's Transform position.
    public void WarpTo(Transform place,string withVelocity = "yes")
    {
        if(withVelocity == "no")
        {
            rb.velocity = new Vector2(0,0);   
        }
        transform.position = place.position;
    }

    public void SetSpeedY(float newSpeed)
    {
        rb.velocity = new Vector2(rb.velocity.x, newSpeed);
    }

    public void GetGravityControl(bool active)
    {
        if(rb.gravityScale != 0)
        {
            gSb = rb.gravityScale;
        }
        if(active)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gSb;
        }
    }

    // Collision check (hopefully for all cases)
    void OnTriggerStay2D(Collider2D any)
    {
        if(any.tag == "Door")
        {
            nextToDoor = true;

            if(Input.GetButtonDown("Interact"))
            {
                building.DoorWarpTo(transform,any.transform);
            }
        }
    }

    void OnTriggerExit2D(Collider2D any)
    {
        nextToDoor = false;
    }

    // Comando que libera o jogador do Lock do inimigo
    IEnumerator FreeThePlayerDelay()
    {
        yield return new WaitForSeconds(startFreeLockTime);
        playerMovementLock = false;
        //FreeThePlayer();
    }

    public void FreeThePlayer()
    {
        playerMovementLock = false;
    }

    public void DestroyPlayer()
    {
        Destroy(gameObject);
    }

    public void setExtraJumps(int aux)
    {
        extraJumps = aux;
    }
}
