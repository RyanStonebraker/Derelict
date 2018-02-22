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
        battleship = GameObject.Find("Warship");
        radarShip = GameObject.Find("");
        cruiser = GameObject.Find("");
        aircraftCarrier = GameObject.Find("");
    }

    private void lockShipsInPlace()
    {
        submarine.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakForce = uint.MaxValue;
        submarine.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakTorque = uint.MaxValue;

        battleship.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakForce = uint.MaxValue;
        battleship.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakTorque = uint.MaxValue;

        //radarShip.GetComponent<GeneralObject>().breakForce = uint.MaxValue;
        //radarShip.GetComponent<GeneralObject>().breakTorque = uint.MaxValue;

        //cruiser.GetComponent<GeneralObject>().breakForce = uint.MaxValue;
        //cruiser.GetComponent<GeneralObject>().breakTorque = uint.MaxValue;

        //aircraftCarrier.GetComponent<GeneralObject>().breakForce = uint.MaxValue;
        //aircraftCarrier.GetComponent<GeneralObject>().breakTorque = uint.MaxValue;
    }

    private void initGame()
    {
        findShips();
        lockShipsInPlace();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Registered on Enter");
        initGame();
        editMode = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
