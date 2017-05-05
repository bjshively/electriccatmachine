using System.Collections;
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
            rigidBody.velocity = new Vector2(10 * facing, rigidBody.velocity.y);
        }
    }

    protected override void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= facing;
        transform.localScale = scale;
    }

    protected override void Kill()
    {
        Destroy(gameObject, 3);
    }
}
