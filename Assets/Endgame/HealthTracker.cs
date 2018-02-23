using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTracker : MonoBehaviour {
	public int playerShips = 5;
	public int enemyShips = 5;

	private int nearMeRange = 100;

	public GameObject explosion;

	public void loseGame () {

	}

	public void winGame () {
		// Instantiate(explosion, new Vector3(transform.position.x + Random.Range(-nearMeRange, nearMeRange), transform.position.y + Random.Range(-nearMeRange, nearMeRange), transform.position.z + Random.Range(-nearMeRange, nearMeRange)), transform.rotation) as GameObject;
	}

	public void checkGameState() {
		if (playerShips == 0)
			loseGame();
		else if (enemyShips == 0)
			winGame();
	}

	public void removePlayerShip() {
		--playerShips;
	}

	public void removeEnemyShip() {
		--enemyShips;
	}

	void Update () {
		checkGameState();
	}
}
