using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public List<GameObject> nodes = new List<GameObject>(100);
    private int boardInitCount = 0;
	
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
        replace(collision.gameObject.tag[3], boardInitCount / 10, collision.gameObject);
        boardInitCount++;
        Debug.Log(collision.gameObject.tag + " " + collision.gameObject);
    }

    void OnTriggerStay(Collider collision)
    {
        
    }

    // Update is called once per frame
    void Update () {
        
	}
}
