using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFollowingCat : MonoBehaviour
{
    private Vector3 mouseWorld;
    private Rigidbody2D rigidBody;
    public LayerMask ground;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
        Flip();
        mouseWorld = Input.mousePosition;
        mouseWorld.z = 10.0f;
        mouseWorld = Camera.main.ScreenToWorldPoint(mouseWorld);
        float xpos = this.transform.position.x;

        if (Input.GetMouseButton(0))
        {
            if (IsGrounded())
            {
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

    private bool IsGrounded()
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

    private void Flip()
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
}
