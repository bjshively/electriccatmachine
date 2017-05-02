using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 laserPoint;
    private Vector3 mouseWorld;
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
        if (Input.GetMouseButton(0))
        {
            lineRenderer.enabled = true;
            // Translate mouse position to world position
            mouseWorld = Input.mousePosition;
            mouseWorld.z = 10.0f;
            mouseWorld = Camera.main.ScreenToWorldPoint(mouseWorld);
//        Vector3[] lineCoordinates = [this.transform, Input.mousePosition];
            if (Input.GetMouseButton(0))
            {
                lineRenderer.SetPositions(new Vector3[]{ this.transform.position, mouseWorld });
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

}