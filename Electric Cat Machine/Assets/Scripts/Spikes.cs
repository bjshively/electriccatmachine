﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Mechanics>().Kill();
        }
        else if (other.gameObject.layer == 10)
        {
            Destroy(other.gameObject);
        }
    }
}