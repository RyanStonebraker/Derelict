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
	
    public void joinObject()
    {
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

    void OnTriggerEnter (Collider collision)
    {
        Debug.Log("Impact with " + collision.gameObject.tag);

        collision.gameObject.GetComponent<GeneralObject>().currentCollisions.Add(gameObject);
        
        if (collisionIsWithBattleshipPiece(collision))
        {
            Debug.Log("Impact with " + collision.gameObject);
            try
            {
                setCollidingObject(collision);

                if (gameObject.GetComponent<FixedJoint>() == null)
                {
                    collision.gameObject.transform.parent = gameObject.transform;
                    joinObject();
                }
                //snap();
            }
            catch
            {
                Debug.Log("Error in joint");
            }
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collisionIsWithBattleshipPiece(collision))
        {
            try
            {
                if (gameObject.GetComponent<FixedJoint>() == null)
                {
                    setCollidingObject(collision);
                    collision.gameObject.transform.parent = gameObject.transform;
                    joinObject();
                    miss = true;
                    //snap();
                }
            }
            catch
            {
                Debug.Log("Error in joint");
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

    public void colorNodes()
    {
        if (shipPart != null)
        {
            GetComponent<Renderer>().enabled = false;
            Instantiate(shipPart, transform.position, transform.rotation);
        }

        if (collidingObject != null)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
            state = true;

            if (!hit && collidingObject.tag == "Finish")
            {
                miss = true;
                if (miss)
                    gameObject.GetComponent<Renderer>().material.color = Color.white;
            }
        }
        else if (!miss)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
            state = false;
        }

        if (sunk)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (hit)
        {
            float duration = 1.0F;
            float lerp = Mathf.PingPong(Time.time, duration) / duration;
            gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, lerp);
        }
    }

	// Update is called once per frame
	void Update () {
        colorNodes();
	}
}
