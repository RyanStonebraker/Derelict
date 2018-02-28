using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {
	public GameObject ship;
	private Vector3 shipPosCopy;

	public GameObject explosion;
	public GameObject endExplosion;
	public GameObject smokeTrails;
	public GameObject missile;

	public int HitCount;

	public int ShipBlockWidth = 50;
	public int ShipBlockHeight = 50;

	private bool sunk = false;
	private string originalName = "";

	public Vector3 BoardSeparationAmount = new Vector3(300,0,600);

	private Vector3 SeaBoardLocation;

	static private int instanceCount = 0;

  public string SeaboardName = "SeaBoard";

	[System.Serializable]
	public struct ShipPiece { public bool isDead; public int Row; public int Col;}

	public ShipPiece [] LifeSpots;
	public string teamBoard = "Playerboard";

	public void Start() {
		LifeSpots = new ShipPiece[HitCount];

		for (int i = 0; i < HitCount; ++i) {
			LifeSpots[i].isDead = false;
			LifeSpots[i].Row = 0;
			LifeSpots[i].Col = 0;
		}

		GetComponent<Renderer>().enabled = false;
		SeaBoardLocation = GameObject.Find(SeaboardName).GetComponent<Renderer>().bounds.center;

		ship = Instantiate(ship, SeaBoardLocation + BoardSeparationAmount, transform.rotation) as GameObject;
		originalName = ship.gameObject.name;
		ship.gameObject.name = "Ship" + instanceCount.ToString();
		shipPosCopy = ship.gameObject.transform.position;

		++instanceCount;
	}

	public void SetShip (List<Vector3> pieceList) {
		HitCount = pieceList.Count;
		LifeSpots = new ShipPiece[HitCount];

		for (int i = 0; i < HitCount; ++i) {
			LifeSpots[i].isDead = (int)pieceList[i].x == 0 ? false : true;
			LifeSpots[i].Row = (int)pieceList[i].y;
			LifeSpots[i].Col = (int)pieceList[i].z;
		}

		GameObject.Find("AI").GetComponent<AI>().addPlayerShip(pieceList, originalName);
		Debug.Log("SetShip was called for: " + originalName + " on " + SeaboardName);
	}

	public bool checkHit(GameObject BoardObject, int shipPiece) {
		Debug.Log("Ship spot on " + originalName + " on " + SeaboardName + ": " + (char)('A' + LifeSpots[shipPiece].Row) + LifeSpots[shipPiece].Col + " was " + ((BoardObject.GetComponent<Board>().getNodeState((char)('A' + LifeSpots[shipPiece].Row), LifeSpots[shipPiece].Col)) ? "Hit" : "Not Hit"));
		return BoardObject.GetComponent<Board>().getNodeState((char)('A' + LifeSpots[shipPiece].Row), LifeSpots[shipPiece].Col);
	}

	public char getRow (int shipPiece) {
		return (char)('A' + LifeSpots[shipPiece].Row);
	}

	public int getCol (int shipPiece) {
		return LifeSpots[shipPiece].Col;
	}

	public void doExplosion () {
		Vector3 posSave = ship.transform.position;
		posSave = new Vector3(posSave.x + Random.Range(-25, 25), posSave.y + Random.Range(50,125), posSave.z + Random.Range(-100,50));

		Instantiate(explosion, posSave, transform.rotation);
	}

	public void doEndExplosion () {
		// shootMissile();
		Vector3 posSave = ship.transform.position;
		posSave = new Vector3(posSave.x + Random.Range(-25, 25), posSave.y + Random.Range(50,125), posSave.z + Random.Range(-100,50));

		ship.GetComponent<Rigidbody>().velocity = new Vector3(0, -10, 0);

		endExplosion = Instantiate(endExplosion, posSave, transform.rotation) as GameObject;
	}

	public void shootMissile () {
		Vector3 aboveShip = ship.transform.position;
		aboveShip = new Vector3(aboveShip.x, aboveShip.y + 100, aboveShip.z);
		missile = Instantiate(missile, aboveShip, transform.rotation) as GameObject;
		missile.GetComponent<Rigidbody>().velocity = new Vector3(0, -100, 0);
		// missile.GetComponent<Rigidbody>().velocity = new Vector3(missile.GetComponent<Rigidbody>().velocity.x, missile.GetComponent<Rigidbody>().velocity.y, missile.GetComponent<Rigidbody>().velocity.z + 10);
	}

	public void removeShip() {
		ship.gameObject.transform.localScale = new Vector3(0,0,0);
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
				if (LifeSpots[shipPiece].isDead == false && checkHit(BoardObject, shipPiece)) {

					// DEBUG
					Debug.Log(originalName + " on " + SeaboardName + " was HIT!");

					// BoardObject.GetComponent<Board>().toggleMiss(getRow(shipPiece), getCol(shipPiece));
					LifeSpots[shipPiece].isDead = true;

					BoardObject.GetComponent<Board>().setHit(getRow(shipPiece), getCol(shipPiece));

					doExplosion();
				}
			}
		}

		bool allDead = true;
		for (int i = 0; i < LifeSpots.Length; ++i) {
			if (!LifeSpots[i].isDead) {
				allDead = false;
			}
		}

		if (allDead) {
			doEndExplosion();
			sunk = true;

			if (teamBoard == "PlayerBoardAI")
				GameObject.Find("EndGame").GetComponent<HealthTracker>().removeEnemyShip();
			else
				GameObject.Find("EndGame").GetComponent<HealthTracker>().removePlayerShip();

			// removeShip();
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
		int widthAdjust = (isColumnAligned()) ? HitCount * ShipBlockWidth : 0;
		int heightAdjust = (isRowAligned()) ? HitCount * ShipBlockHeight : 0;

		ship.transform.position = shipPosCopy + new Vector3(LifeSpots[0].Row * -BoardSeparationAmount.x - widthAdjust, 0, LifeSpots[0].Col * -BoardSeparationAmount.z + heightAdjust);
	}

	public void hideShip () {
		ship.transform.position = new Vector3(-1000, -1000, -1000);
	}

	private bool isColumnAligned () {
		return LifeSpots[0].Col == LifeSpots[1].Col;
	}

	private bool isRowAligned () {
		return LifeSpots[0].Row == LifeSpots[1].Row;
	}

	private bool rotationSet () {
		if (isColumnAligned ())
			return ship.transform.eulerAngles.y == -90;
		else if (isRowAligned ())
			return ship.transform.eulerAngles.y == 0;

		return false;
		Debug.Log(gameObject.name + " PLACEMENT NOT SET CORRECTLY!");
	}

	public void setShipRotation () {
		if (!rotationSet() && isColumnAligned()) {
			ship.transform.eulerAngles = new Vector3(0,-90,0);
		}
		else if (!rotationSet() && isRowAligned()) {
			ship.transform.eulerAngles = new Vector3(0,0,0);
		}
	}

	public void Update () {
		if (!sunk) {
			UpdateShip();

			if (LifeSpots[0].Row == 0 && LifeSpots[0].Col == 0 && LifeSpots[1].Row == 0 && LifeSpots[1].Col == 0) {
				hideShip();
			}
			else {
				placeShipOnWaterGrid();
			}

			setShipRotation();
		}
	}
}
