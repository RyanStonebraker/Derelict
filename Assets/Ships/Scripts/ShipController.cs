using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {
	public GameObject ship;
	private Vector3 shipPosCopy;

	public int HitCount;

	public Vector3 BoardSeparationAmount = new Vector3(300,0,600);

	private Vector3 SeaBoardLocation;

	static private int instanceCount = 0;

	[System.Serializable]
	public struct ShipPiece { public bool Dead; public int Row; public int Col;}

	public ShipPiece [] LifeSpots;
	public string teamBoard = "GameBoard-Hit";

	public void Start() {
		LifeSpots = new ShipPiece[HitCount];

		for (int i = 0; i < HitCount; ++i) {
			LifeSpots[i].Dead = false;
			LifeSpots[i].Row = 0;
			LifeSpots[i].Col = 0;
		}

		GetComponent<Renderer>().enabled = false;
		SeaBoardLocation = GameObject.Find("SeaBoard").GetComponent<Renderer>().bounds.center;
		ship = Instantiate(ship, SeaBoardLocation + BoardSeparationAmount, transform.rotation) as GameObject;
		ship.gameObject.name = "Ship" + instanceCount.ToString();
		shipPosCopy = ship.gameObject.transform.position;

		++instanceCount;
	}

	public void SetShip (List<Vector3> pieceList) {
		HitCount = pieceList.Count;
		LifeSpots = new ShipPiece[HitCount];

		for (int i = 0; i < HitCount; ++i) {
			LifeSpots[i].Dead = (int)pieceList[i].x == 0 ? false : true;
			LifeSpots[i].Row = (int)pieceList[i].y;
			LifeSpots[i].Col = (int)pieceList[i].z;
		}
	}

	public bool checkHit(GameObject & BoardObject, int shipPiece) {
		return BoardObject.GetComponent<Board>().getNodeState((char)('A' + LifeSpots[shipPiece].Row), LifeSpots[shipPiece].Col);
	}

	public char getRow (int shipPiece) {
		return (char)('A' + LifeSpots[shipPiece].Row);
	}

	public int getCol (int shipPiece) {
		return LifeSpots[shipPiece].Col;
	}

	public void UpdateShip () {
		GameObject BoardObject = null;
		try {
			BoardObject = GameObject.Find(teamBoard).gameObject;
		}
		catch {
			Debug.Log("----- Ship Error ----- Board does not exist!");
		}

		if (BoardObject != null) {
			for (int shipPiece = 0; shipPiece < LifeSpots.Length; ++shipPiece) {
				if (checkHit(BoardObject, shipPiece)) {
					BoardObject.GetComponent<Board>().toggleMiss(getRow(shipPiece), getCol(shipPiece));
					LifeSpots[shipPiece].Dead = true;
					BoardObject.GetComponent<Board>().setHit(getRow(shipPiece), getCol(shipPiece));
				}
			}
		}

		bool allDead = true;
		for (int i = 0; i < LifeSpots.Length; ++i) {
			if (!LifeSpots[i].Dead) {
				allDead = false;
			}
		}

		if (allDead) {
			// TODO: Explosions? Boom
			ship.gameObject.transform.localScale = new Vector3(0,0,0);
			if (BoardObject != null) {
				// Sets all nodes to sunk
				for (int i = 0; i < LifeSpots.Length; ++i) {
					BoardObject.GetComponent<Board>().setSunk(getRow(i), getCol(i));
				}
			}
		}
		else if (!allDead && ship.gameObject.transform.localScale.x == 0) {
			ship.gameObject.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void placeShipOnWaterGrid () {
		ship.transform.position = shipPosCopy + new Vector3(LifeSpots[0].Row * BoardSeparationAmount.x, 0, LifeSpots[0].Col * BoardSeparationAmount.z);
	}

	public void hideShip () {
		ship.transform.position = new Vector3(0, 0, 0);
	}

	public void setShipRotation () {
		if (ship.transform.eulerAngles.y != -90 && LifeSpots[0].Col == LifeSpots[1].Col) {
			ship.transform.eulerAngles = new Vector3(0,-90,0);
		}
		else if (ship.transform.eulerAngles.y != 0 && LifeSpots[0].Row == LifeSpots[1].Row) {
			ship.transform.eulerAngles = new Vector3(0,0,0);
		}
	}

	public void Update () {
		UpdateShip();

		if (LifeSpots[0].Row == 0 && LifeSpots[0].Col == 0 && LifeSpots[1].Row == 0 && LifeSpots[1].Col == 0) {
			hideShip();
		}
		else
			placeShipOnWaterGrid();

		setShipRotation();
	}
}
