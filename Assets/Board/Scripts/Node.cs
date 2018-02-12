using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public bool state = false;
    public GameObject shipPart = null;
	
	// Update is called once per frame
	void Update () {
		if(shipPart != null)
        {
            GetComponent <Renderer>().enabled = false;
            Instantiate(shipPart, transform.position, transform.rotation);
        }
	}
}
