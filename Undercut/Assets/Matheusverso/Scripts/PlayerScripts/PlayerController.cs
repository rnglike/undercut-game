using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Tempo do jogador Travado
    public float timeLocked;
    public float startFreeLockTime;

    // Movimentation variables
    public float speed;
    public float jumpForce;
    private float moveInput;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool playerMovementLock = false;

    // Variable of inside/outside check
    public bool inside;

    // Variables of ground check
    private bool isGrounded;
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


    void Awake()
    {
        // Assigning variables
        rb = GetComponent<Rigidbody2D>();
        extraJumps = defaultJumps;
        playerAnimator = GetComponent<Animator>();
        building = GameObject.FindGameObjectWithTag("Building").GetComponent<Building>();
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
    public void WarpTo(Transform place)
    {
        transform.position = place.position;
    }

    // Collision check (hopefully for all cases)
    void OnTriggerStay2D(Collider2D any)
    {
        if(Input.GetButtonDown("Interact"))
        {
            if((any.transform.parent.tag == "Doors"))
            {
                building.DoorWarpTo(transform,any.transform);
            }
        }
    }

    // Comando que libera o jogador do Lock do inimigo, o tempo para se soltar é fixo pro jogador no momento, tendo que achar uma forma de executar esse comando no script do inimigo para não ser fixo.
    IEnumerator FreeThePlayerDelay()
    {
        yield return new WaitForSeconds(startFreeLockTime);
        FreeThePlayer();
    }

    public void FreeThePlayer()
    {
        playerMovementLock = false;
    }
}
