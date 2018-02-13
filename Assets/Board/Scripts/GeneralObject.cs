using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralObject : MonoBehaviour {

    public GameObject collidingObject = null;
    public List<GameObject> currentCollisions = new List<GameObject>();
    public List<Vector3> shipConstructor = new List<Vector3>();
    public int shipSize = 3;
    public string shipName = "";
    public bool flag = true;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject);
        if (collision.gameObject.tag == "Controller")
            setCollidingObject(collision);

        /******REMOVE THIS FOR DEMO******/
    }

    private void populate()
    {
        foreach(GameObject node in currentCollisions)
        {
            shipConstructor.Add(new Vector3(0f, node.tag[3] - 'A', node.name[6] - '0'));
        }
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

        if(currentCollisions.Count == shipSize && shipConstructor.Count < shipSize)
        {
            populate();
        }

        if (shipConstructor.Count == shipSize && flag)
        {
            Debug.Log("Before SetShip");
            GameObject.Find(shipName + "Player").GetComponent<ShipController>().SetShip(shipConstructor);
            Debug.Log("Called SetShip");
            flag = !!flag;
        }
    }
}
