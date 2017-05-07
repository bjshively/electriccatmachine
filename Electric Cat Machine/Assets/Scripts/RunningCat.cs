﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningCat : Cat
{
    protected override void Start()
    {
        // Run regular setup
        base.Start();

        //Start timer to destroy running cat
        Kill();
    }

    protected override void Move()
    {
        if (IsGrounded())
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x * 1.2f, rigidBody.velocity.y);
        }
    }

    protected override void Flip()
    {
        if (facingRight && (mouseWorld.x < gameObject.transform.position.x))
        {
            facingRight = false;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        else if (!facingRight && (mouseWorld.x > gameObject.transform.position.x))
        {
            facingRight = true;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    protected override void Kill()
    {
        Destroy(gameObject, 3);
    }
}
