using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    public bool isTouchingWall;
    public float wallCheckDistance;
    public LayerMask whatIsGround;
    private bool facingRight;
    private Rigidbody2D rb;

    private bool dontSlide = true;
    private bool aux = false;
    private int hairCode;

    private bool isGrounded;

    void Start()
    {
        rb = gameObject.GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = gameObject.GetComponentInParent<PlayerController>().isGrounded;
        hairCode = gameObject.GetComponentInParent<PlayerItems>().GetHairCode();

        if (hairCode == 4)
        {
            facingRight = gameObject.GetComponentInParent<PlayerController>().facingRight;
            wjump();
        }

        if (isGrounded)
        {
            dontSlide = true;
            aux = !facingRight;
        }
    }


    public void wjump()
    {
        float moveInput = gameObject.GetComponentInParent<PlayerController>().moveInput;

        // DON'T SLIDE
        if (aux == facingRight)
        {
            dontSlide = false;
        }
        else
        {
            dontSlide = true;
        }

        // Selecting side to check
        if (facingRight)
        {
            isTouchingWall = Physics2D.Raycast(transform.position, transform.right, wallCheckDistance, whatIsGround);
        }
        else
        {
            isTouchingWall = Physics2D.Raycast(transform.position, transform.right, -wallCheckDistance, whatIsGround);
        }
        
        // Wall jump
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0 && moveInput != 0 && dontSlide)
        {     
            aux = facingRight;
            gameObject.GetComponentInParent<PlayerController>().setExtraJumps(gameObject.GetComponentInParent<PlayerController>().defaultJumps);
        }
    }


    // Drawing raycast
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.white;
        if (facingRight == true)
        {
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance, transform.position.y, transform.position.z));
        }
        else
        {
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - wallCheckDistance, transform.position.y, transform.position.z));
        }
    }
}