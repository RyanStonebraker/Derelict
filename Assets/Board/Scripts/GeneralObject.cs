using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralObject : MonoBehaviour {

    public GameObject collidingObject = null;

    void OnCollisionEnter(Collision collision)
    {
        setCollidingObject(collision);
    }

    private void setCollidingObject(Collision col)
    {
        //if controller is already assigned an objectInHand or the game object
        //isn't a rigidbody then don't update the collidingObject
        if (collidingObject || !col.gameObject.GetComponent<Rigidbody>())
            return;

        //assign colliding object as a potential grab target
        collidingObject = col.gameObject;
    }

    void onCollisionStay(Collision collision)
    {
        setCollidingObject(collision);
    }

    void onCollisionExit(Collision collision)
    {
        if (!collidingObject)
            return;

        collidingObject = null;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
