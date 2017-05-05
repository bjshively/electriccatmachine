using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 laserPoint;
    private Vector3 mouseWorld;
    private int facing;
    private GameObject babyNinja;
    //    private
    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
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

        // TODO: Freeze player movement when laser pointer is active
        if (Input.GetMouseButtonDown(0) && !babyNinja)
        {
            ThrowCat("LaserFollowingCat");
        }
        if (Input.GetMouseButton(0))
        {
            lineRenderer.enabled = true;
            // Translate mouse position to world position

            if (Input.GetMouseButton(0))
            {
                lineRenderer.SetPositions(new Vector3[]{ this.transform.position, mouseWorld });
            }
        }
        else
        {
            lineRenderer.enabled = false;
            Destroy(babyNinja);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ThrowCat("RunningCat");
        }
    }

    public void ThrowCat(string catType)
    {
        babyNinja = (GameObject)Instantiate(Resources.Load(catType));
        babyNinja.transform.position = this.transform.position;
        babyNinja.GetComponent <Rigidbody2D>().AddForce(new Vector2(200 * facing, 200));
    }

}