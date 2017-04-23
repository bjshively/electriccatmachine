using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;

    [SerializeField] // Shows the movement speed in the inspector
	private float movementSpeed;

    private bool attack;
    private bool facingRight;

    [SerializeField]
    private Transform[] groundPoints;


    // Define how close the player has to be to a platform to be considered on the ground
    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;
    private bool isGrounded;

    private bool jump;

    [SerializeField]
    private float jumpForce;

    // Use this to "freeze" the player, i.e. can't move when attacking
    private bool canMove;


    // Use this for initialization
    void Start()
    {
        canMove = true;
        facingRight = true;
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        isGrounded = IsGrounded();
        HandleInput();
    }

    private void HandleAttacks()
    {
        canMove = false;
        myRigidBody.velocity = Vector2.zero;
        myAnimator.SetTrigger("attack");
    }

    private void HandleInput()
    {
        // Don't process any input if the player is attacking
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myRigidBody.velocity = Vector2.zero;
        }

        // Proceed with normal input processing/movement
        if (canMove)
        {
            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                myRigidBody.AddForce((new Vector2(0, jumpForce)));
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                attack = true;
                canMove = false;
                myRigidBody.velocity = Vector2.zero;
                HandleAttacks();
            }
            else
            {
                float horizontal = Input.GetAxis("Horizontal");
                myRigidBody.velocity = new Vector2(horizontal * movementSpeed, myRigidBody.velocity.y);

                // Set speed inside animator for controlling run animation
                myAnimator.SetFloat("speed", Mathf.Abs(horizontal));

                Flip(horizontal);
            }

        }
    }

    // Flip player sprite to face the correct direction based on movement
    private void Flip(float horizontal)
    {
        if (canMove)
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
    }

    private void ResetValues()
    {
        attack = false;
        canMove = true;
    }

    // Determine if the player is touching the ground
    private bool IsGrounded()
    {
        if (myRigidBody.velocity.y <= 0)
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
