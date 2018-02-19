// Node.cs
// Derelict
// CS 372
// Tristan Van Cise
// Ryan Stonebraker
// Creates joints with valids objects, interacts with placeable
// battleship piece collision list, and changes/updates node states
// depending on hit, sunk, and miss booleans.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public bool state = false;
    public GameObject shipPart = null;
    public GameObject collidingObject;
    public int jointBreakForce = 1000;
    public int jointTorqueBreakForce = 1000;
    public bool hit = false;
    public bool sunk = false;
    public bool miss = false;
    public string boardType = "";
	
    public void joinObject(Collider collision)
    {
        collision.gameObject.transform.parent = gameObject.transform;
        var joint = addFixedJoint();
        joint.connectedBody = collidingObject.GetComponent<Rigidbody>();
        Debug.Log("Joint Created with " + collidingObject.gameObject);
    }

    private FixedJoint addFixedJoint()
    {
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.breakForce = jointBreakForce;
        joint.breakTorque = jointTorqueBreakForce;
        return joint;
    }

    private void setCollidingObject(Collider col)
    {
        if (collidingObject || !col.gameObject.GetComponent<Rigidbody>())
            return;

        collidingObject = col.gameObject;
    }

    private void snap()
    {
        collidingObject.transform.position = gameObject.transform.position + new Vector3 (0,0,0.5f);
        collidingObject.transform.rotation = gameObject.transform.rotation;
    }

    private bool collisionIsWithBattleshipPiece(Collider collision)
    {
        return collision.gameObject.tag != "Board"
           && collision.gameObject.tag != "RowA"
           && collision.gameObject.tag != "RowB"
           && collision.gameObject.tag != "RowC"
           && collision.gameObject.tag != "RowD"
           && collision.gameObject.tag != "RowE"
           && collision.gameObject.tag != "RowF"
           && collision.gameObject.tag != "RowG"
           && collision.gameObject.tag != "RowH"
           && collision.gameObject.tag != "RowI"
           && collision.gameObject.tag != "RowJ"
           && collision.gameObject.tag != "Controller";
    }

    private void addNodeToBattleshipCollisionList(Collider collision)
    {
        collision.gameObject.GetComponent<GeneralObject>().currentCollisions.Add(gameObject);
    }

    private bool nodeNotJoinedWithBattleshipPiece()
    {
        return gameObject.GetComponent<FixedJoint>() == null;
    }

    void OnTriggerEnter (Collider collision)
    {
        Debug.Log("Impact with " + collision.gameObject.tag);

        if (boardType == "" && collision.gameObject.tag == "Board")
            boardType = collision.gameObject.name;

        addNodeToBattleshipCollisionList(collision);
        
        if (collisionIsWithBattleshipPiece(collision))
        {
            Debug.Log("Impact with " + collision.gameObject);
            try
            {
                setCollidingObject(collision);

                if (nodeNotJoinedWithBattleshipPiece())
                    joinObject(collision);
                //snap(); //Work in progress
            }
            catch
            {
                Debug.Log("Error in joint connection when object entered collider");
            }
            
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collisionIsWithBattleshipPiece(collision))
        {
            try
            {
                if (nodeNotJoinedWithBattleshipPiece())
                {
                    setCollidingObject(collision);
                    joinObject(collision);
                    miss = true;
                    //snap(); //Work in progress
                }
            }
            catch
            {
                Debug.Log("Error in joint connection while object is in collider");
            }
        }
    }

    private void killJoint()
    {
        GetComponent<FixedJoint>().connectedBody = null;
        Destroy(GetComponent<FixedJoint>());
    }

    private void resetColoring()
    {
        state = false;
        hit = false;
        sunk = false;
        miss = false;
        colorNodes();
    }

    private void resetBattleshipCollisionsList(Collider collision)
    {
        collision.gameObject.GetComponent<GeneralObject>().currentCollisions.Remove(gameObject);
        collision.gameObject.GetComponent<GeneralObject>().shipConstructor.Clear();
    }

    private void removeReferenceToCollidedObject(Collider collision)
    {
        Debug.Log("Killed " + collision.gameObject);
        collidingObject = null;
    }

    void OnTriggerExit(Collider collision)
    {
        removeReferenceToCollidedObject(collision);
        resetBattleshipCollisionsList(collision);
        killJoint();

        // make the node turn green when the battleship piece exits

        miss = false; 
        resetColoring();
    }

    //color the node blue and set the node as occupied with battleship piece
    private void setNodeToOccupiedState()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

    private void setNodeToMissState()
    {
        miss = true;
        if (miss)
            gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    private void setNodeToDefaultState()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
        state = false;
    }

    private void setNodeToHitState()
    {
        float duration = 1.0F;
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, lerp);
    }

    private void setNodeToSunkState()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    private bool theShotWasAMiss()
    {
        return (!hit && collidingObject.tag == "Finish");
    }

    public void colorNodes()
    {
        if (shipPart != null) //test if this is doing anything
        {
            GetComponent<Renderer>().enabled = false;
            Instantiate(shipPart, transform.position, transform.rotation);
        }

        if(collidingObject != null)
            state = true;

        if (boardType == "PlacementBoard")
        {
            if (collidingObject != null)
                setNodeToOccupiedState();
            else
                setNodeToDefaultState();
        }
        else if (boardType == "PlayerBoard")
        {
            if (theShotWasAMiss())
                setNodeToMissState();
            else if (sunk)
                setNodeToSunkState();
            else if (hit)
                setNodeToHitState();
            else
                setNodeToDefaultState();
        }
    }

	// Update is called once per frame
	void Update () {
        colorNodes();
	}
}
