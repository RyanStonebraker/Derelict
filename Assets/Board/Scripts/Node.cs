using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public bool state = false;
    public GameObject shipPart = null;
    public GameObject collidingObject;
    public int jointBreakForce = 1000;
    public int jointTorqueBreakForce = 1000;
	
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

    void OnTriggerEnter (Collider collision)
    {
        Debug.Log("Impact with " + collision.gameObject.tag);
        if (collision.gameObject.tag != "Board" 
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
           /*&& collision.gameObject.GetComponent<GeneralObject>().collidingObject.tag == "Controller"*/)
        {
            Debug.Log("Impact with " + collision.gameObject);
            try
            {
                //collision.gameObject.GetComponent<GeneralObject>().collidingObject.GetComponent<Grab>().releaseObject();
                setCollidingObject(collision);

                if (!gameObject.GetComponent<FixedJoint>().connectedBody)
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
        Debug.Log("Impact with " + collision.gameObject.tag);
        if (collision.gameObject.tag != "Board"
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
           /*&& collision.gameObject.GetComponent<GeneralObject>().collidingObject.tag == "Controller"*/)
        {
            Debug.Log("Impact with " + collision.gameObject);
            try
            {
                //collision.gameObject.GetComponent<GeneralObject>().collidingObject.GetComponent<Grab>().releaseObject();
                setCollidingObject(collision);
                collision.gameObject.transform.parent = gameObject.transform;
                joinObject();
                //snap();
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

    void OnTriggerExit(Collider collision)
    {
        Debug.Log("Killed " + collision.gameObject);
        collidingObject = null;
        killJoint();
    }

	// Update is called once per frame
	void Update () {
		if(shipPart != null)
        {
            GetComponent <Renderer>().enabled = false;
            Instantiate(shipPart, transform.position, transform.rotation);
        }
	}
}
