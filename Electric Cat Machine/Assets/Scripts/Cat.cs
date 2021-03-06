﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cat : MonoBehaviour
{

    protected Vector3 mouseWorld;
    protected Rigidbody2D rigidBody;
    protected Animator animator;
    public LayerMask ground;
    protected Mechanics playerAttributes;

    // Attributes
    public bool facingRight;
    public bool alive;

    // Use this for initialization
    protected virtual void Start()
    {
        playerAttributes = GameObject.FindGameObjectWithTag("Player").GetComponent<Mechanics>();
        alive = true;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("jump", true);
        Flip();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        mouseWorld = Input.mousePosition;
        mouseWorld.z = 10.0f;
        mouseWorld = Camera.main.ScreenToWorldPoint(mouseWorld);
        Move();

        if (!alive)
        {
            Kill();
        }
    }

    // Flip the cat sprite to face the mouse cursor
    protected virtual void Flip()
    {
        Vector3 scale = transform.localScale;
        if (mouseWorld.x > this.transform.position.x)
        {
            scale.x = 2f;
        }
        else
        {
            scale.x = -2f;
        }
        transform.localScale = scale;
    }

    // Return true if the cat is touching the ground
    public bool IsGrounded()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 1f, ground.value))
        {
            animator.SetBool("falling", false);
            animator.SetBool("jump", false);
            animator.SetBool("grounded", true);
            return true;
        }
        else
        {
            animator.SetBool("grounded", false);
            if (rigidBody.velocity.y < 0)
            {
                animator.SetBool("falling", true);
            }
            else
            {
                animator.SetBool("falling", false);
                animator.SetBool("jumping", true);
            }
            return false;
        }
    }

    // Immediately destroy the cat
    protected virtual void Kill()
    {
        if (alive)
        {
            alive = false;
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        Camera.main.GetComponent<CameraFollow>().target = GameObject.Find("Player").transform;
        playerAttributes.canThrowCat = true;
    }


    // Destroy cat when it goes off screen (no longer visible to Camera)
    protected virtual void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // Each cat type implements its own Move logic
    protected virtual void Move()
    {
        if (!alive)
        {
            rigidBody.AddForce(new Vector2(rigidBody.velocity.x * 10f, rigidBody.velocity.x));
        }
    }
}