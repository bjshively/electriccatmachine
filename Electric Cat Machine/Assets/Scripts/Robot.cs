using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;


    // Player Attributes
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float jumpForce;
    private bool airControl = true;
    private float speed;

    // Player state
    private bool facingRight;
    private bool grounded;
    private bool attack;
    private bool jump;
    private bool slide;

    [SerializeField]
    private LayerMask whatIsGround;

    private BoxCollider2D jumpCollider;

    // ##################################################
    // Setup and update functions
    // ##################################################
    private void Start()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        jumpCollider = this.transform.Find("groundPoint").GetComponent<BoxCollider2D>();
        grounded = true;
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        //Gets the horizontal input
        float horizontal = Input.GetAxis("Horizontal");

        //Checks if the player is grounded
        grounded = IsGrounded();

        if (!grounded)
        {
            myAnimator.SetBool("jump", true);
        }
        else
        {
            myAnimator.SetBool("jump", false);
        }
            
        Flip(horizontal);
        HandleMovement(horizontal);
        HandleAttacks();
//        ResetActions();
    }

    // ##################################################
    // Player Actions
    // ##################################################


    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            jump = true;
            myAnimator.SetBool("jump", true);
        }

        if (grounded || airControl)
        {
            //            myAnimator.SetBool("jump", false);
            float horizontal = Input.GetAxis("Horizontal");
            myRigidbody.AddForce(new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y));

            // Set speed inside animator for controlling run animation
            myAnimator.SetFloat("speed", Mathf.Abs(horizontal * movementSpeed));
            Flip(horizontal);
        }
        //        }
    }

    private void HandleMovement(float horizontal)
    {
        if (grounded && jump) //if we should jump
        {
            jump = false;
            myRigidbody.AddForce(new Vector2(myRigidbody.velocity.x, 5), ForceMode2D.Impulse);
        }
    }


    private void HandleAttacks()
    {
        if (attack && grounded && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myRigidbody.velocity = Vector2.zero;
            myAnimator.SetTrigger("attack");
        }
    }

    // Flip player sprite to face the correct direction based on movement
    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            // Flip x scale, update Player's localScale
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    // Determine if the player is touching the ground
    private bool IsGrounded()
    {
        if (Physics2D.Raycast(jumpCollider.transform.position, Vector2.down, 0.4f, whatIsGround.value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
