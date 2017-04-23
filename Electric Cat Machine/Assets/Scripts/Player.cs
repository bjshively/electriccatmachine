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
            if (Input.GetKeyDown(KeyCode.Space))
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
}
