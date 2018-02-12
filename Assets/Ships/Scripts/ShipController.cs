using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {
	public GameObject ship;

	public int HitCount;

	static private int instanceCount = 0;

	[System.Serializable]
	public struct ShipPiece { public bool Dead; public int Row; public int Col;}

	public ShipPiece [] LifeSpots;
	public string teamBoard = "AIBoard";

	public void Awake() {
		GetComponent<Renderer>().enabled = false;
		ship = Instantiate(ship, gameObject.GetComponent<Renderer>().bounds.center + Vector3.forward, transform.rotation) as GameObject;
		ship.gameObject.name = "Ship" + instanceCount.ToString();

		LifeSpots = new ShipPiece[HitCount];

		for (int i = 0; i < HitCount; ++i) {
			LifeSpots[i].Dead = false;
			LifeSpots[i].Row = -1;
			LifeSpots[i].Col = -1;
		}
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
		BoardObject = GameObject.FindGameObjectsWithTag(teamBoard)[0];
		}
		catch {
			Debug.Log("Board does not exist!");
		}

		if (BoardObject != null) {
			for (int i = 0; i < LifeSpots.Length; ++i) {
				if (BoardObject.GetComponent<Board>().getNodeState((char)('A' + LifeSpots[i].Row), LifeSpots[i].Col))
					LifeSpots[i].Dead = true;
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
		}
		else if (!allDead && ship.gameObject.transform.localScale.x == 0) {
			ship.gameObject.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void Update () {
		UpdateShip();
	}
}
