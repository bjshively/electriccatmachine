using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanics : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Rigidbody2D rigidBody;
    private Animator animator;

    private Vector3 laserPoint;
    private Vector3 mouseWorld;
    private int facing;

    private GameObject babyNinja;
    private GameObject laserOrigin;
    public float maxSpeed = 15f;
    public float jumpForce = 70f;



    // Mechanics
    public bool canThrowCat;
    public bool isShiningLaser;
    private bool canMove;
    private bool canJump;

    [SerializeField]
    public LayerMask ground;

    private GameObject cat;

    void Start()
    {
//        Cursor.visible = false;
        isShiningLaser = false;
        canThrowCat = true;
        canJump = true;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.sortingLayerName = "Laser";
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();


        laserOrigin = GameObject.Find("LaserOrigin");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mouseWorld = Input.mousePosition;
        mouseWorld.z = 10.0f;
        mouseWorld = Camera.main.ScreenToWorldPoint(mouseWorld);

        facing = 1;
        if (this.transform.position.x > mouseWorld.x)
        {
            facing = -1;
        }

        float horizontal = Input.GetAxis("Horizontal");

        HandleControls(horizontal);
    }

    public void HandleControls(float horizontal)
    {
        Debug.Log(IsGrounded());
        if (IsGrounded())
        {
            canJump = true;
        }

        if (canMove)
        {
            rigidBody.velocity = new Vector2(maxSpeed * horizontal, rigidBody.velocity.y);
            if (horizontal > 0)
            {
                facing = 1;
            }
            else if (horizontal < 0)
            {
                facing = -1;
            }

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
        if (Input.GetMouseButtonDown(0) && !isShiningLaser && IsGrounded())
        {
            // TODO: Freeze player movement when laser pointer is active
            ThrowCat("LaserFollowingCat");
        }

        // While button is held, shine laser
        if (Input.GetMouseButton(0))
        {
            if (isShiningLaser)
            {
                canMove = false;
                rigidBody.velocity = Vector2.zero;
                lineRenderer.enabled = true;
                lineRenderer.SetPositions(new Vector3[]{ laserOrigin.transform.position, mouseWorld });
            }
        }

        // Disable laser when mouse button isn't pressed
        else
        {
            lineRenderer.enabled = false;
            canMove = true;
        }

    }

    public bool IsGrounded()
    {
        if (Physics2D.Raycast(transform.FindChild("groundPoint").position, Vector2.down, 0.4f, ground.value))
        {
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
            babyNinja.transform.position = this.transform.position;
            //TODO: Make cats throw at about the same speed the player is currently moving (i.e. running jump and throw)
            babyNinja.GetComponent <Rigidbody2D>().AddForce(new Vector2(200 * facing, 200));

            // Facing is actually throwing direction, which is towards the cursor
            // This tells which way to throw the cat, and the cat to face
            babyNinja.GetComponent <Cat>().facing = facing;
        }
    }
}