using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralObject : MonoBehaviour {

    public GameObject collidingObject = null;
    public List<GameObject> currentCollisions = new List<GameObject>();

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject);
        if (collision.gameObject.tag == "Controller")
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

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exited with " + collision.gameObject);
        if (!collidingObject)
            return;

        collidingObject = null;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
