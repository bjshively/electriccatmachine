using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFollowingCat : Cat
{
    protected override void Move()
    {
        Flip();
        if (!Input.GetMouseButton(0))
        {
            alive = false;
        }

        if (IsGrounded())
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
}