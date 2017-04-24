﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float xMax;

    [SerializeField]
    private float xMin;

    [SerializeField]
    private float yMax;


    [SerializeField]
    private float yMin;

    private Transform target;

    // Use this for initialization
    void Start()
    {
        //Sets a reference to the player
        target = GameObject.Find("Robot").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Follows the player
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), -10);
    }
}
