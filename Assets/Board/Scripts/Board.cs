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

    public GameObject get(char row, int index)
    {
        return nodes[(row - 'A') * 10 + index];
    }

    public bool getNodeState(char row, int index)
    {
        return get(row, index).GetComponent<Node>().state;
    }

    public void setHit(char row, int index)
    {
        get(row, index).GetComponent<Node>().hit = true;
    }

    public void setSunk(char row, int index)
    {
        get(row, index).GetComponent<Node>().sunk = true;
    }

    public void toggleMiss(char row, int index)
    {
        get(row, index).GetComponent<Node>().miss = !get(row, index).GetComponent<Node>().miss;
    }

    public void setNodeState(char row, int index, bool state)
    {
        get(row, index).GetComponent<Node>().state = state;
    }

    public void setShipPart(char row, int index, GameObject shipPart)
    {
        get(row, index).GetComponent<Node>().shipPart = shipPart;
    }
    
    void OnTriggerEnter(Collider collision)
    {
        //row and index data are stored as part of object naming convention
        char row = collision.gameObject.tag[3];
        int index = collision.gameObject.name[6] - '0';

        replace(row, index, collision.gameObject);

        //debug print format: "Row <char> node <#>"
        Debug.Log(collision.gameObject.tag + " " + collision.gameObject);
    }

    void OnTriggerStay(Collider collision)
    {
        //Proto
    }

}
