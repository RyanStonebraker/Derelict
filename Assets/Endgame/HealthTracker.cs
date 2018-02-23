using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTracker : MonoBehaviour {
	public int playerShips = 5;
	public int enemyShips = 5;

	private int nearMeRange = 100;

	public GameObject explosion;
	public GameObject winText;
	public GameObject loseText;

	private int explosionsPerFrame = 10;

	private bool gameEnded = false;

	public void loseGame () {
		for (int i = 0; i < explosionsPerFrame; ++i) {
			GameObject tempExplosion = Instantiate(explosion, new Vector3(transform.position.x + Random.Range(-nearMeRange, nearMeRange), transform.position.y + Random.Range(-nearMeRange, nearMeRange), transform.position.z + Random.Range(-nearMeRange, nearMeRange)), transform.rotation) as GameObject;
			Destroy(tempExplosion, 2);
		}

		if (!gameEnded) {
			Vector3 shiftedTextPosition = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z + 3);
			loseText = Instantiate(loseText, shiftedTextPosition, transform.rotation) as GameObject;
			gameEnded = true;
		}
	}

	public void winGame () {
		for (int i = 0; i < explosionsPerFrame; ++i) {
			GameObject tempExplosion = Instantiate(explosion, new Vector3(transform.position.x + Random.Range(-nearMeRange, nearMeRange), transform.position.y + Random.Range(-nearMeRange, nearMeRange), transform.position.z + Random.Range(-nearMeRange, nearMeRange)), transform.rotation) as GameObject;
			Destroy(tempExplosion, 2);
		}

		if (!gameEnded) {
			Vector3 shiftedTextPosition = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z + 3);
			winText = Instantiate(winText, shiftedTextPosition, transform.rotation) as GameObject;
			gameEnded = true;
		}
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
		// if (playerShips >= 0 && enemyShips >= 0)
			checkGameState();
	}
}
