using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    public bool editMode = true;

    public GameObject submarine = null;
    public GameObject battleship = null;
    public GameObject radarShip = null;
    public GameObject cruiser = null;
    public GameObject aircraftCarrier = null;

    private void findShips()
    {
        submarine = GameObject.Find("Submarine");
        battleship = GameObject.Find("");
        radarShip = GameObject.Find("");
        cruiser = GameObject.Find("");
        aircraftCarrier = GameObject.Find("");
    }

    private void lockShipsInPlace()
    {
        submarine.GetComponent<FixedJoint>().breakForce = uint.MaxValue;
        submarine.GetComponent<FixedJoint>().breakTorque = uint.MaxValue;

        battleship.GetComponent<FixedJoint>().breakForce = uint.MaxValue;
        battleship.GetComponent<FixedJoint>().breakTorque = uint.MaxValue;

        radarShip.GetComponent<FixedJoint>().breakForce = uint.MaxValue;
        radarShip.GetComponent<FixedJoint>().breakTorque = uint.MaxValue;

        cruiser.GetComponent<FixedJoint>().breakForce = uint.MaxValue;
        cruiser.GetComponent<FixedJoint>().breakTorque = uint.MaxValue;

        aircraftCarrier.GetComponent<FixedJoint>().breakForce = uint.MaxValue;
        aircraftCarrier.GetComponent<FixedJoint>().breakTorque = uint.MaxValue;
    }

    private void initGame()
    {
        findShips();
        lockShipsInPlace();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Registered on Enter");
        editMode = false;
        initGame();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
