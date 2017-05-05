﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cat : MonoBehaviour
{

    protected Vector3 mouseWorld;
    protected Rigidbody2D rigidBody;
    public LayerMask ground;
    protected Mechanics playerAttributes;

    // Attributes
    public int facing;
    public bool alive;

    // Use this for initialization
    protected virtual void Start()
    {
        playerAttributes = GameObject.FindGameObjectWithTag("Player").GetComponent<Mechanics>();
        alive = true;
        rigidBody = GetComponent<Rigidbody2D>();
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
            scale.x = .3f;
        }
        else
        {
            scale.x = -.3f;
        }
        transform.localScale = scale;
    }

    // Return true if the cat is touching the ground
    public bool IsGrounded()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 1f, ground.value))
        {
            return true;
        }
        else
        {
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
        playerAttributes.canThrowCat = true;
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