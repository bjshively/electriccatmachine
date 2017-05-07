using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningCat : Cat
{
    protected override void Start()
    {
        // Run regular setup
        base.Start();
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
        if (!facingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }


    // Running cats self destruct 3 seconds after spawning
    protected override void Kill()
    {
        Destroy(gameObject, 3);
    }
}
