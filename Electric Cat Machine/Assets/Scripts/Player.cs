using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private Rigidbody2D myRigidBody;
	private Animator myAnimator;

	[SerializeField] // Shows the movement speed in the inspector
	private float movementSpeed;

	private bool facingRight;

	// Use this for initialization
	void Start()
	{
		facingRight = true;
		myRigidBody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
	}

	void FixedUpdate()
	{
		float horizontal = Input.GetAxis("Horizontal");

		HandleMovement(horizontal);
		Flip(horizontal);
	}

	private void HandleMovement(float horizontal)
	{
		myRigidBody.velocity = new Vector2(horizontal * movementSpeed, myRigidBody.velocity.y);

		// Set speed inside animator for controlling run animation
		myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
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
}
