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

        //Flips the player in the correct direction
        Flip(horizontal);

        //Handles the player's movement
        HandleMovement(horizontal);

        //Handles the player's attacks
        HandleAttacks();

        //Handles the animator layers
        //HandleLayers();

        Debug.Log("grounded: " + grounded);
        Debug.Log("jump: " + jump);


        //Resets all actions
        ResetActions();
    }

    // ##################################################
    // Player Actions
    // ##################################################
    private void HandleMovement(float horizontal)
    {
        if (grounded && jump) //if we should jump
        {
            myRigidbody.AddForce(new Vector2(0f, 400));
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

    private void HandleInput()
    {
        // Don't process any input if the player is attacking
//        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
//        {
//            myRigidbody.velocity = Vector2.zero;
//        }


        // This is a quick hack to test loading another scene
//        if (Input.GetKeyDown(KeyCode.Z))
//        {
//            Application.LoadLevel("MainScene");
//        }
//
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            grounded = false;
            jump = true;
            myAnimator.SetBool("jump", true);
        }

//        if (Input.GetKeyDown(KeyCode.LeftShift))
//        {
//            attack = true;
//            myRigidbody.velocity = Vector2.zero;
//            HandleAttacks();
//        }
//        else
//        {
        if (grounded || airControl)
        {
//            myAnimator.SetBool("jump", false);
            float horizontal = Input.GetAxis("Horizontal");
            myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);

            // Set speed inside animator for controlling run animation
            myAnimator.SetFloat("speed", Mathf.Abs(horizontal * movementSpeed));
            Flip(horizontal);
        }
//        }
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

    private void ResetActions()
    {
        attack = false;
        if (IsGrounded())
        {
            jump = false;   
        }
        slide = false;

    }

    // Determine if the player is touching the ground
    private bool IsGrounded()
    {
//        return jumpCollider.IsTouchingLayers(whatIsGround);
        if (Physics2D.Raycast(jumpCollider.transform.position, Vector2.down, 0.2f, whatIsGround.value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
