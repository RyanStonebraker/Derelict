using System.Collections;
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
        randomSpawnShip(5);
        randomSpawnShip(4);
        randomSpawnShip(3);
        randomSpawnShip(3);
        randomSpawnShip(2);
    }

    private void randomSpawnShip(int shipLength)
    {
        int initPosition = rng.Next(100);
        int rotation = rng.Next(4);

        switch(rotation)
        {
            case 0: //left
                if (checkLeftPlacement(initPosition, shipLength))
                    for (int shipPos = 0; shipPos < shipLength; shipPos++)
                    {
                        AIBoard.GetComponent<Board>().nodes[initPosition - shipPos].GetComponent<Node>().state = true;
                        AIBoard.GetComponent<Board>().nodes[initPosition - shipPos].GetComponent<Node>().setNodeToHitState(); //REMOVE
                    }
                else
                    randomSpawnShip(shipLength);
            break;

            case 1: //right
                if (checkRightPlacement(initPosition, shipLength))
                    for (int shipPos = 0; shipPos < shipLength; shipPos++)
                    {
                        AIBoard.GetComponent<Board>().nodes[initPosition + shipPos].GetComponent<Node>().state = true;
                        AIBoard.GetComponent<Board>().nodes[initPosition + shipPos].GetComponent<Node>().setNodeToHitState(); //REMOVE
                    }
                else
                    randomSpawnShip(shipLength);
                break;

            case 2: //up
                if (checkUpPlacement(initPosition, shipLength))
                    for (int shipPos = 0; shipPos < shipLength; shipPos++)
                    {
                        AIBoard.GetComponent<Board>().nodes[initPosition - 10 * shipPos].GetComponent<Node>().state = true;
                        AIBoard.GetComponent<Board>().nodes[initPosition - 10 * shipPos].GetComponent<Node>().setNodeToHitState(); //REMOVE
                    }
                else
                    randomSpawnShip(shipLength);
                break;

            case 3: //down
                if (checkDownPlacement(initPosition, shipLength))
                    for (int shipPos = 0; shipPos < shipLength; shipPos++)
                    {
                        AIBoard.GetComponent<Board>().nodes[initPosition + 10 * shipPos].GetComponent<Node>().state = true;
                        AIBoard.GetComponent<Board>().nodes[initPosition + 10 * shipPos].GetComponent<Node>().setNodeToHitState(); //REMOVE
                    }
                else
                    randomSpawnShip(shipLength);
                break;

            default:
                Debug.Log("***Bad rotation random number generated***");
            break;
        }

    }

    private bool edgeCaseWrap(int piecesRemaining, int currentPos, int modVal)
    {
        return (piecesRemaining != 1) && (currentPos % 10 == modVal);
    }

    private bool checkLeftPlacement(int initPos, int shipLength)
    {
        for(int posCheck = 0; posCheck < shipLength; posCheck++)
        {
            if ((initPos - posCheck) < 0 || 
                AIBoard.GetComponent<Board>().nodes[initPos - posCheck].GetComponent<Node>().state || 
                edgeCaseWrap((shipLength-posCheck),(initPos-posCheck),0))
                return false;
        }
        return true;
    }

    private bool checkRightPlacement(int initPos, int shipLength)
    {
        for (int posCheck = 0; posCheck < shipLength; posCheck++)
        {
            if ((initPos + posCheck) > 99 || 
                AIBoard.GetComponent<Board>().nodes[initPos + posCheck].GetComponent<Node>().state ||
                edgeCaseWrap((shipLength - posCheck), (initPos + posCheck), 9))
                return false;
        }
        return true;
    }

    private bool checkUpPlacement(int initPos, int shipLength)
    {
        for (int posCheck = 0; posCheck < shipLength; posCheck++)
        {
            if ((initPos - 10*posCheck) < 0 || 
                AIBoard.GetComponent<Board>().nodes[initPos - 10*posCheck].GetComponent<Node>().state)
                return false;
        }
        return true;
    }

    private bool checkDownPlacement(int initPos, int shipLength)
    {
        for (int posCheck = 0; posCheck < shipLength; posCheck++)
        {
            if ((initPos + 10*posCheck) > 99 || 
                AIBoard.GetComponent<Board>().nodes[initPos + 10*posCheck].GetComponent<Node>().state)
                return false;
        }
        return true;
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

        if (currentShot)
        {
            if (currentShot.GetComponent<ShotPiece>().attachedToNode && !stopCheck)
            {
                stopCheck = true;
                fireAtPlayer();
                Debug.Log("Fired at player");
                Wait(1, () => { generateNextShot(); });
                Debug.Log("Spawned next shot");
            }
        }
    }
}
