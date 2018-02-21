﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour {

    public GameObject SHOTPREFAB = null;
    public GameObject PlayerBoard = null;
    public GameObject AIBoard = null;
    public GameObject currentShot = null;

    public bool stopCheck = false;

    System.Random rng = new System.Random();

    public void Wait(float seconds, Action action)
    {
        StartCoroutine(_wait(seconds, action));
    }

    IEnumerator _wait(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    private void getBoardReferences()
    {
        PlayerBoard = GameObject.Find("PlacementBoard");
        AIBoard = GameObject.Find("PlayerBoardAI");
    }

    //Aircraft = 5
    //Battleship = 4
    //Cruiser = 3
    //Submarine = 3
    //RadarShip = 2
    private void setAIShips()
    {
        for(int i = 0; i < 5; i++)
            AIBoard.GetComponent<Board>().nodes[i].GetComponent<Node>().state = true;

        for(int i = 0; i < 4; i++)
            AIBoard.GetComponent<Board>().nodes[10 + i].GetComponent<Node>().state = true;

        for(int i = 0; i < 3; i++)
            AIBoard.GetComponent<Board>().nodes[20 + i].GetComponent<Node>().state = true;

        for (int i = 0; i < 3; i++)
            AIBoard.GetComponent<Board>().nodes[30 + i].GetComponent<Node>().state = true;

        for (int i = 0; i < 2; i++)
            AIBoard.GetComponent<Board>().nodes[40 + i].GetComponent<Node>().state = true;
    }

    private void generateNextShot()
    {
        currentShot = Instantiate(SHOTPREFAB, new Vector3(1131.301f, 365.357f, 1461.777f), Quaternion.identity);
        stopCheck = false;
    }

    private void fireAtPlayer()
    {
        List <GameObject> playerNodes = PlayerBoard.GetComponent<Board>().nodes;

        while (true)
        {
            int coord = rng.Next(100);
            bool hit = playerNodes[coord].GetComponent<Node>().state && !playerNodes[coord].GetComponent<Node>().miss;
            bool miss = !playerNodes[coord].GetComponent<Node>().state && !playerNodes[coord].GetComponent<Node>().miss;
            Debug.Log("Generated shot coords at " + coord + "hit status: " + hit + "miss status: " + miss);

            if (hit)
            {
                playerNodes[coord].GetComponent<Node>().hit = true;
                playerNodes[coord].GetComponent<Node>().setNodeToHitState();
                playerNodes[coord].GetComponent<Node>().collidingObject = SHOTPREFAB;
                Debug.Log("AI LANDED A HIT!");
                break;
            }
            else if(miss)
            {
                playerNodes[coord].GetComponent<Node>().setNodeToMissState();
                playerNodes[coord].GetComponent<Node>().collidingObject = SHOTPREFAB;
                Debug.Log("AI MISSED!");
                break;
            }
        }

    }

	// Update is called once per frame
	void Update () {
        if (PlayerBoard == null && !GameObject.Find("StartBlock").GetComponent<StartGame>().editMode)
        {
            Debug.Log("AI INITIATED");
            getBoardReferences();
            setAIShips();
            generateNextShot();
        }

        if(currentShot.GetComponent<ShotPiece>().attachedToNode && !stopCheck)
        {
            stopCheck = true;
            fireAtPlayer();
            Debug.Log("Fired at player");
            Wait(1, () => { generateNextShot(); });
            Debug.Log("Spawned next shot");
        }
    }
}
