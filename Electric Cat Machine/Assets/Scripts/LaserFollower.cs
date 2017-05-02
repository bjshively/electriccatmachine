using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFollower : MonoBehaviour
{
    private Vector3 mouseWorld;
    private Rigidbody2D rigidBody;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
        mouseWorld = Input.mousePosition;
        mouseWorld.z = 10.0f;
        mouseWorld = Camera.main.ScreenToWorldPoint(mouseWorld);
        float xpos = this.transform.position.x;

        if (Input.GetMouseButton(0))
        {
            if (mouseWorld.x > xpos)
            {
                Vector3 scale = transform.localScale;
                scale.x = .3f;
                transform.localScale = scale;
                rigidBody.velocity = new Vector2(5, 0);
            }
            else if (mouseWorld.x < xpos)
            {
                Vector3 scale = transform.localScale;
                scale.x = -.3f;
                transform.localScale = scale;
                rigidBody.velocity = new Vector2(-5, 0);
            }
        }
        else
        {
            rigidBody.velocity = Vector2.down;
        }
    }
}
