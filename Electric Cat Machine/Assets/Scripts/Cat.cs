using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cat : MonoBehaviour
{

    protected Vector3 mouseWorld;
    protected Rigidbody2D rigidBody;
    public LayerMask ground;
    public int facing;
    public bool alive;

    // Use this for initialization
    protected virtual void Start()
    {
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

    protected virtual void Kill()
    {
        Destroy(gameObject);
    }

    protected abstract void Move();

}