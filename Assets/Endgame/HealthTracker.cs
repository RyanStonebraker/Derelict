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

	public void loseGame () {
		for (int i = 0; i < 10; ++i)
			Instantiate(explosion, new Vector3(transform.position.x + Random.Range(-nearMeRange, nearMeRange), transform.position.y + Random.Range(-nearMeRange, nearMeRange), transform.position.z + Random.Range(-nearMeRange, nearMeRange)), transform.rotation);
			loseText = Instantiate(loseText, transform.position, transform.rotation) as GameObject;
	}

	public void winGame () {
		for (int i = 0; i < 10; ++i)
			Instantiate(explosion, new Vector3(transform.position.x + Random.Range(-nearMeRange, nearMeRange), transform.position.y + Random.Range(-nearMeRange, nearMeRange), transform.position.z + Random.Range(-nearMeRange, nearMeRange)), transform.rotation);
			winText = Instantiate(winText, transform.position, transform.rotation) as GameObject;
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
		if (playerShips >= -50 && enemyShips >= -50)
			checkGameState();
	}
}
