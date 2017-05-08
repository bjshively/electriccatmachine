using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mechanics : MonoBehaviour
{
    // Components
    private LineRenderer lineRenderer;
    private Rigidbody2D rigidBody;
    private Animator animator;

    // Values
    private Vector3 startingPoint;
    private Vector3 laserPoint;
    private Vector3 mouseWorld;
    private bool facingRight;
    private bool grounded;


    private GameObject babyNinja;
    private GameObject laserOrigin;
    private float maxSpeed = 10f;
    private float jumpForce = 14f;



    // Mechanics
    public bool canThrowCat;
    public bool isShiningLaser;
    public bool canMove;
    private bool canJump;

    [SerializeField]
    public LayerMask ground;

    private GameObject cat;

    void Start()
    {
        // Invisible cursor applies to all of Unity and makes debugging difficult.
        // May want to only set this when shining laser.
        // Cursor.visible = false;
        isShiningLaser = false;
        canThrowCat = true;
        canJump = true;
        canMove = true;
        facingRight = true;

        startingPoint = transform.position;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.sortingLayerName = "Laser";

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        laserOrigin = GameObject.Find("LaserOrigin");

    }

    void Update()
    {
        // Converts mouse coords to world coords for drawing vectors
        mouseWorld = Input.mousePosition;
        mouseWorld.z = 10.0f;
        mouseWorld = Camera.main.ScreenToWorldPoint(mouseWorld);

        // Maintains the relative position for the laser origin point
        laserOrigin.transform.parent = gameObject.transform;
        laserOrigin.transform.localPosition = new Vector2(0.09f, 0.33f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // This is needed for the laser pointer look behavior
        // but it causes issues with the Flip() function
        // Need to move it so its only called when shining laser

//        facingRight = true;
//        if (this.transform.position.x > mouseWorld.x)
//        {
//            facingRight = false;
//        }
        InBounds();
        grounded = IsGrounded();
        float horizontal = Input.GetAxis("Horizontal");
        Flip(horizontal);
        HandleControls(horizontal);
    }

    public void HandleControls(float horizontal)
    {
        if (grounded)
        {
            canJump = true;
        }

        if (canMove)
        {
            animator.SetFloat("speed", Mathf.Abs(horizontal * maxSpeed));
            rigidBody.velocity = new Vector2(maxSpeed * horizontal, rigidBody.velocity.y);
//            if (horizontal > 0)
//            {
//                facing = 1;
//            }
//            else if (horizontal < 0)
//            {
//                facing = -1;
//            }

            if (canJump && Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }
        }

        // Throw a regular cat
        if (Input.GetKeyDown(KeyCode.R))
        {
            ThrowCat("RunningCat");
        }

        // Shine laser. Throw a laser pointer chasing cat
        if (Input.GetMouseButtonDown(0) && !isShiningLaser && grounded)
        {
            ThrowCat("LaserFollowingCat");
        }

        // While button is held, shine laser
        if (Input.GetMouseButton(0))
        {
            if (isShiningLaser)
            {
                canMove = false;
                animator.Play("Idle");
                rigidBody.velocity = Vector2.zero;
                lineRenderer.enabled = true;
                lineRenderer.SetPositions(new Vector3[]{ laserOrigin.transform.position, mouseWorld });
            }
        }

        // Disable laser when mouse button isn't pressed
        else
        {
            lineRenderer.enabled = false;
        }

    }

    // Flip player sprite to face the correct direction based on movement
    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            // Flip x scale, update Player's localScale
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public bool IsGrounded()
    {
        if (Physics2D.Raycast(transform.FindChild("groundPoint").position, Vector2.down, 0.4f, ground.value))
        {
            animator.SetBool("jump", false);
            animator.SetBool("falling", false);
            animator.SetBool("grounded", true);
            return true;
        }
        else
        {
            if (rigidBody.velocity.y < 0)
            {
                animator.SetBool("falling", true);
            }
            animator.SetBool("grounded", false);
            return false;
        }
    }

    private void Jump()
    {
        if (canJump)
        {
            animator.SetBool("jump", true);
            canJump = false;
            rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void ThrowCat(string catType)
    {
        if (canThrowCat)
        {
            canThrowCat = false;
            babyNinja = (GameObject)Instantiate(Resources.Load(catType));
            babyNinja.tag = "Cat";
            babyNinja.transform.position = transform.position;
           
            //TODO: Make cats throw at about the same speed the player is currently moving (i.e. running jump and throw)
            Vector2 throwForce;
            if (facingRight)
            {
                throwForce = new Vector2(rigidBody.velocity.x + 200, 200);
            }
            else
            {
                throwForce = new Vector2(rigidBody.velocity.x + -200, 200);
            }

            babyNinja.GetComponent <Rigidbody2D>().AddForce(throwForce);

            // Facing is actually throwing direction, which is towards the cursor
            // This tells which way to throw the cat, and the cat to face
            babyNinja.GetComponent <Cat>().facingRight = facingRight;
        }
    }

    public void Kill()
    {
        rigidBody.velocity = Vector3.zero;
        transform.position = startingPoint;
    }


    // A very simple bounds check to respawn if you fall off the platforms
    public void InBounds()
    {
        if (transform.position.y < -100)
        {
            Kill();
        }
    }
}