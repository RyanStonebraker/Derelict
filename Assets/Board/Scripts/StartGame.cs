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
        battleship = GameObject.Find("Battleship");
        radarShip = GameObject.Find("Landing Craft");
        cruiser = GameObject.Find("Warship");
        aircraftCarrier = GameObject.Find("Aircraft Carrier");
    }

    private void lockShipsInPlace()
    {
        submarine.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakForce = uint.MaxValue;
        submarine.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakTorque = uint.MaxValue;
        submarine.GetComponent<GeneralObject>().flag = false;

        battleship.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakForce = uint.MaxValue;
        battleship.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakTorque = uint.MaxValue;
        battleship.GetComponent<GeneralObject>().flag = false;


        radarShip.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakForce = uint.MaxValue;
        radarShip.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakTorque = uint.MaxValue;
        radarShip.GetComponent<GeneralObject>().flag = false;


        cruiser.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakForce = uint.MaxValue;
        cruiser.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakTorque = uint.MaxValue;
        cruiser.GetComponent<GeneralObject>().flag = false;


        aircraftCarrier.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakForce = uint.MaxValue;
        aircraftCarrier.GetComponent<GeneralObject>().currentCollisions[0].GetComponent<FixedJoint>().breakTorque = uint.MaxValue;
        aircraftCarrier.GetComponent<GeneralObject>().flag = false;

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
