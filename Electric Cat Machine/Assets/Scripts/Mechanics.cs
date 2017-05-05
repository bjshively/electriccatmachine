using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanics : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 laserPoint;
    private Vector3 mouseWorld;
    private int facing;
    private GameObject babyNinja;

    // Mechanics
    public bool canThrowCat;
    public bool isShiningLaser;

    private GameObject cat;

    void Start()
    {
        Cursor.visible = false;
        isShiningLaser = false;
        canThrowCat = true;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.sortingLayerName = "Laser";
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
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

        HandleControls();
    }

    public void HandleControls()
    {
        // Throw a regular cat
        if (Input.GetKeyDown(KeyCode.R))
        {
            ThrowCat("RunningCat");
        }

        // Shine laser. Throw a laser pointer chasing cat
        if (Input.GetMouseButtonDown(0) && !isShiningLaser)
        {
            // TODO: Freeze player movement when laser pointer is active
            ThrowCat("LaserFollowingCat");
        }

        // While button is held, shine laser
        if (Input.GetMouseButton(0))
        {
            if (isShiningLaser)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPositions(new Vector3[]{ this.transform.position, mouseWorld });
            }
        }

        // Disable laser when mouse button isn't pressed
        else
        {
            lineRenderer.enabled = false;
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
            babyNinja.GetComponent <Rigidbody2D>().AddForce(new Vector2(200 * facing, 200));

            // Facing is actually throwing direction, which is towards the cursor
            // This tells which way to throw the cat, and the cat to face
            babyNinja.GetComponent <Cat>().facing = facing;
        }
    }
}