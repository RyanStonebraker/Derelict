﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour {

    public float hoverStrength = 14f;
    public bool checkShips = true;
    public bool checkShots = true;
    public bool hoverAll = false;

    private bool aShotCollided(Collider collision)
    {
        return collision.gameObject.tag == "Shot" && checkShots;
    }

    private bool aShipPieceCollided(Collider collision)
    {
        return collision.gameObject.tag == "Ship" && checkShips;
    }

    void OnTriggerEnter(Collider collision)
    {
        if(aShotCollided(collision) || aShipPieceCollided(collision) || hoverAll)
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * hoverStrength, ForceMode.Force); //.up is short for (0,1,0)
    }

    void OnTriggerStay(Collider collision)
    {
        if (aShotCollided(collision) || aShipPieceCollided(collision) || hoverAll)
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * hoverStrength, ForceMode.Force);
    }

    void OnTriggerExit(Collider collision)
    {
        //might use this for some effects later
        //if (aShotCollided(collision) || aShipPieceCollided(collision))
        //    collision.gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.up * 10f, ForceMode.Force);
    }
}
