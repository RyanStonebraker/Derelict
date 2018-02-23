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

public class Node : MonoBehaviour
{

    public bool state = false;
    public GameObject shipPart = null;
    public GameObject collidingObject = null;
    public int jointBreakForce = 1000;
    public int jointTorqueBreakForce = 1000;
    public bool hit = false;
    public bool sunk = false;
    public bool miss = false;
    public bool occupied = false;
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

        if (collidingObject.tag == "Shot")
        {
            joint.breakForce = uint.MaxValue;
            joint.breakTorque = uint.MaxValue;
        }

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
        collidingObject.transform.position = gameObject.transform.position + new Vector3(0, 0, 0.5f);
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
           && collision.gameObject.tag != "Controller"
           && collision.gameObject.tag != "Fragment";
    }

    private void addNodeToBattleshipCollisionList(Collider collision)
    {
        if (collision.gameObject.tag == "Ship")
            collision.gameObject.GetComponent<GeneralObject>().currentCollisions.Add(gameObject);
    }

    private bool nodeNotJoinedWithBattleshipPiece()
    {
        return gameObject.GetComponent<FixedJoint>() == null;
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Impact with " + collision.gameObject.tag);

        if (collision.gameObject.tag == "Shot")
            collision.gameObject.GetComponent<ShotPiece>().attachedToNode = true;

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
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
        }
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
        try
        {
            resetBattleshipCollisionsList(collision);
        }
        catch
        {
            Debug.Log(collision.gameObject + "was not a battleship piece");
        }
        killJoint();

        // make the node turn green when the battleship piece exits
        if (collision.gameObject.tag != "Controller")
        {
            miss = false;

            if (boardType != "PlayerBoardAI")
            {
                collidingObject = null;
                resetColoring();
            }
        }
    }

    //color the node blue and set the node as occupied with battleship piece
    private void setNodeToOccupiedState()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

    public void setNodeToMissState()
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

    public void setNodeToHitState()
    {
        float duration = 1.0F;
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, lerp);
    }

    public void setNodeToSunkState()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    private bool theShotWasAMiss()
    {
        //try
        //{
        return !hit && (collidingObject.tag == "Shot");
          
        //}
        //catch
        //{
        //    return false;
        //}
    }

    public void colorNodes()
    {
        if (shipPart) //test if this is doing anything
        {
            GetComponent<Renderer>().enabled = false;
            Instantiate(shipPart, transform.position, transform.rotation);
        }

        if (collidingObject)
            state = true;

        if (boardType == "PlacementBoard")
        {
            if (collidingObject)
                setNodeToOccupiedState();
            else
                setNodeToDefaultState();
        }
        else if (boardType == "PlayerBoardAI" && collidingObject)
        {
            if (hit)
                setNodeToHitState();
            else if (theShotWasAMiss())
                setNodeToMissState();
            else if (sunk)
                setNodeToSunkState();
            else
                setNodeToDefaultState();
        }
        
    }

    private void colorShot()
    {
        Debug.Log("Inside colorShot");
        state = true;
        if (occupied)
        {
            hit = true;
            setNodeToHitState();
        }
        else
        {
            setNodeToMissState();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (collidingObject && collidingObject.tag == "Shot")
            colorShot();
        else
            colorNodes();
    }
}
