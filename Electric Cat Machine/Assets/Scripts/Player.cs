using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;


    // Player Attributes
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float jumpForce;
    private bool airControl;

    // Player state
    private bool facingRight;
    private bool grounded;
    private bool attack;
    private bool jump;
    private bool jumpAttack;
    private bool slide;

    [SerializeField]
    private Transform[] groundPoints;

    // Define how close the player has to be to a platform to be considered on the ground
    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    // ##################################################
    // Setup and update functions
    // ##################################################
    private void Start()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
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

        //Flips the player in the correct direction
        Flip(horizontal);

        //Handles the player's movement
        HandleMovement(horizontal);

        //Handles the player's attacks
        HandleAttacks();

        //Handles the animator layers
        //HandleLayers();

        //Resets all actions
        ResetActions();
    }

    //
    // Player Actions
    //

    private void HandleMovement(float horizontal)
    {


        if (myRigidbody.velocity.y < 0) //We need to land if the player is falling
        {
            myAnimator.SetBool("Land", true); //Trigers the landing animation
        }
//        if (!myAnimator.GetBool("Slide") && grounded && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || airControl)  //chesk if we should move the player
//        {
//            myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y); //Moves the player
//        }
        if (grounded && jump) //if we should jump
        {
            // Add a vertical force to the player.
            grounded = false;

            //Makes the player jump
            myRigidbody.AddForce(new Vector2(0f, 400));
        }
        if (grounded && slide && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide")) //If we need to slide
        {
            myAnimator.SetBool("Slide", true); //Triggers the slide animation
        }
        else if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide")) //If we are sliding
        {
            myAnimator.SetBool("Slide", false); //Indicate that we are done sliding
        }

        //Keeps the speed in the animator up to date
        myAnimator.SetFloat("Speed", Mathf.Abs(horizontal));

//        if (transform.position.y <= -10) //If we fall down then respawn the player
//        {
//            transform.position = startPos;
//            myRigidbody.velocity = Vector2.zero;
//        }
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
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myRigidbody.velocity = Vector2.zero;
        }

        if (grounded && Input.GetKeyDown(KeyCode.W))
        {
            grounded = false;
            myRigidbody.AddForce((new Vector2(0, jumpForce)));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            attack = true;
            myRigidbody.velocity = Vector2.zero;
            HandleAttacks();
        }
        else
        {
            float horizontal = Input.GetAxis("Horizontal");
            myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);

            // Set speed inside animator for controlling run animation
            myAnimator.SetFloat("speed", Mathf.Abs(horizontal));

            Flip(horizontal);
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

    private void ResetActions()
    {
        attack = false;
        jump = false;
        jumpAttack = false;
        slide = false;

    }

    // Determine if the player is touching the ground
    private bool IsGrounded()
    {
        if (myRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    // If the groundPoint is colliding with anything except the player (GameObject)
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
