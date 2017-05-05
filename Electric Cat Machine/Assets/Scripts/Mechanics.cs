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
    public bool canThrow;
    public bool isShiningLaser;

    private GameObject cat;

    void Start()
    {
        Cursor.visible = false;
       
        isShiningLaser = false;
        canThrow = true;

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


        // Throw a regular cat
        if (Input.GetKeyDown(KeyCode.R) && canThrow)
        {
            canThrow = false;
            cat = ThrowCat("RunningCat");
        }

        // Shine laser. Throw a laser pointer chasing cat
        if (Input.GetMouseButtonDown(0) && !isShiningLaser)
        {
            // TODO: Freeze player movement when laser pointer is active
            isShiningLaser = true;
            cat = ThrowCat("LaserFollowingCat");

        }
        if (Input.GetMouseButton(0))
        {
            lineRenderer.enabled = true;

            if (Input.GetMouseButton(0))
            {
                lineRenderer.SetPositions(new Vector3[]{ this.transform.position, mouseWorld });
            }
        }
        else
        {
            lineRenderer.enabled = false;
            isShiningLaser = false;
        }
    }

    public GameObject ThrowCat(string catType)
    {
        babyNinja = (GameObject)Instantiate(Resources.Load(catType));
        babyNinja.tag = "Cat";
        babyNinja.transform.position = this.transform.position;
        babyNinja.GetComponent <Rigidbody2D>().AddForce(new Vector2(200 * facing, 200));

        // Facing is actually throwing direction, which is towards the cursor
        // This tells which way to throw the cat, and the cat to face
        babyNinja.GetComponent <Cat>().facing = facing;

        return babyNinja;
    }

    public void ResetValues()
    {
        if (!cat)
        {
            canThrow = true;
        }
    }
}