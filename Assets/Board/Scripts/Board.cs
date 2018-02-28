// Board.cs
// Derelict
// CS 372
// Tristan Van Cise
// Ryan Stonebraker
// Acts as a wrapper class around node.cs, board can interact directly with
// node states and track which objects are in each node. On gamestart, a List
// (nodes) is populated with all nodes in contact with the board. This list can
// be viewed within unity during runtime to see board state updates at runtime.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public List<GameObject> nodes = new List<GameObject>(100);

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }

    public int length()
    {
        return nodes.Count;
    }

    public List<GameObject> getList()
    {
        return nodes;
    }

    public void replace(char row, int index, GameObject item)
    {
        nodes[(row - 'A') * 10 + index] = item;
    }

    public GameObject retrieveNode(char row, int index)
    {
        return nodes[(row - 'A') * 10 + index];
    }

    public bool getNodeState(char row, int index)
    {
        // if (retrieveNode(row, index).GetComponent<Node>().collidingObject)
        //     return retrieveNode(row, index).GetComponent<Node>().state && retrieveNode(row, index).GetComponent<Node>().collidingObject.tag != "Ship";
        // else

        if(!retrieveNode(row, index).GetComponent<Node>().collidingObject)
          return false;
        
            if(retrieveNode(row, index).GetComponent<Node>().collidingObject.tag == "Shot")
                Debug.Log("IM A BANANA");

            Debug.Log("Node state for: " + row + index + " is " + retrieveNode(row, index).GetComponent<Node>().state);
            return retrieveNode(row, index).GetComponent<Node>().state;
    }

    public void setHit(char row, int index)
    {
        retrieveNode(row, index).GetComponent<Node>().hit = true;
    }

    public void setSunk(char row, int index)
    {
        retrieveNode(row, index).GetComponent<Node>().sunk = true;
    }

    public void toggleMiss(char row, int index)
    {
        retrieveNode(row, index).GetComponent<Node>().miss = !retrieveNode(row, index).GetComponent<Node>().miss;
    }

    public void setNodeState(char row, int index, bool state)
    {
        retrieveNode(row, index).GetComponent<Node>().state = state;
    }

    public void setShipPart(char row, int index, GameObject shipPart)
    {
        retrieveNode(row, index).GetComponent<Node>().shipPart = shipPart;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name.Contains("Node"))
        {
            //row and index data are stored as part of object naming convention
            try
            {
                char row = collision.gameObject.tag[3];
                int index = collision.gameObject.name[6] - '0';

                replace(row, index, collision.gameObject);
            }
            catch
            {
                Debug.Log("*****FAILED to bind " + collision.gameObject + " to board, board full? (most likely)");
            }
        }
        //debug print format: "Row <char> node <#>"
        Debug.Log(collision.gameObject.tag + " " + collision.gameObject);
    }


}
