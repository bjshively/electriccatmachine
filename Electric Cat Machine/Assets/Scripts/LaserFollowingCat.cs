using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFollowingCat : Cat
{
    protected override void Start()
    {
        base.Start();
        playerAttributes.isShiningLaser = true;
    }

    protected override void Move()
    {
        // As long as the cat is active, it should look at the mouse pointer
        if (alive)
        {
            Flip();
        }

        // When mouse button is let off, deactive the cat
        // Cat is killed immediately if it is airborn
        // This cat begins ignoring the mouse and runs off in the direction it is currently headed
        if (Input.GetMouseButtonUp(0))
        {
            if (!IsGrounded())
            {
                base.Kill();
            }
            else
            {
                Kill();
            }
        }

        // Cat follows position of cursor/laser
        else if (IsGrounded() && alive)
        {
            float xpos = this.transform.position.x;
            if (mouseWorld.x > xpos)
            {
                rigidBody.velocity = new Vector2(5, rigidBody.velocity.y);
            }
            else if (mouseWorld.x < xpos)
            {
                rigidBody.velocity = new Vector2(-5, rigidBody.velocity.y);
            }
        }
    }

    protected override void Kill()
    {
        if (alive)
        {
            alive = false;
            playerAttributes.isShiningLaser = false;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x * 3f, rigidBody.velocity.y);
            Destroy(gameObject, 5);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        playerAttributes.isShiningLaser = false;
    }
}