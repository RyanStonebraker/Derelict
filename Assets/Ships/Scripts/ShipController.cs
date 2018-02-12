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

	// private int PieceToPosition (int pos) {
	// 	ShipPiece loc;
	// 	loc.Dead = false;
	//
	// 	loc.Row = Mathf.Floor(pos / 10);
	// 	loc.Col = pos % 10;
	//
	// 	return loc;
	// }
	//
	// private int PositionToPiece (int row, int col) {
	// 	return row * 10 + col;
	// }

	// pieceList: x -> Dead? 0 - Not Dead, 1 = Dead
	// pieceList: y -> Row
	// pieceList: z -> Col
	public void SetShip (List<Vector3> pieceList) {
		HitCount = pieceList.Count;
		LifeSpots = new ShipPiece[HitCount];

		for (int i = 0; i < HitCount; ++i) {
			LifeSpots[i].Dead = (int)pieceList[i].x == 0 ? false : true;
			LifeSpots[i].Row = (int)pieceList[i].y;
			LifeSpots[i].Col = (int)pieceList[i].z;
		}
	}

	public void UpdateShip () {
		GameObject BoardObject = null;
		try {
			BoardObject = GameObject.Find(teamBoard).gameObject;
		if (BoardObject.GetComponent<Board>()) {
			Debug.Log("GOT BOARD");
		}
		}
		catch {
			Debug.Log("Board does not exist!");
		}

		if (BoardObject != null) {
			for (int i = 0; i < LifeSpots.Length; ++i) {
				if (BoardObject.GetComponent<Board>().getNodeState((char)('A' + LifeSpots[i].Row), LifeSpots[i].Col)) {
					BoardObject.GetComponent<Board>().toggleMiss((char)('A' + LifeSpots[i].Row), LifeSpots[i].Col);
					LifeSpots[i].Dead = true;
					BoardObject.GetComponent<Board>().setHit((char)('A' + LifeSpots[i].Row), LifeSpots[i].Col);
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
					BoardObject.GetComponent<Board>().setSunk((char)('A' + LifeSpots[i].Row), LifeSpots[i].Col);
				}
			}
		}
		else if (!allDead && ship.gameObject.transform.localScale.x == 0) {
			ship.gameObject.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void Update () {
		UpdateShip();

		ship.transform.position = shipPosCopy + new Vector3(LifeSpots[0].Row * BoardSeparationAmount.x, 0, LifeSpots[0].Col * BoardSeparationAmount.z);

		if (ship.transform.eulerAngles.y != -90 && LifeSpots[0].Col == LifeSpots[1].Col) {
			ship.transform.eulerAngles = new Vector3(0,-90,0);
		}
		else if (ship.transform.eulerAngles.y != 0 && LifeSpots[0].Row == LifeSpots[1].Row) {
			ship.transform.eulerAngles = new Vector3(0,0,0);
		}
	}
}
