using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFleet : MonoBehaviour {
	[System.Serializable]
	public struct ShipContainer {public Vector3 [] LifeSpots; public string ShipName;}

	public List<ShipContainer> playerShips;

	private bool ShipAlreadyExists(string shipName) {
		for (int i = 0; i < playerShips.Count; ++i) {
			if (playerShips[i].ShipName == shipName)
				return true;
		}
		return false;
	}

	public void addPlayerShip(Vector3 [] shipLifeSpots, string shipName) {
		if (ShipAlreadyExists(shipName))
			return;
		ShipContainer tempShip;
		tempShip.LifeSpots = shipLifeSpots;
		tempShip.ShipName = shipName;
		playerShips.Add(tempShip);
	}
}
