using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour {

    public GameObject SHOTPREFAB = null;
    public GameObject PlayerBoard = null;
    public GameObject AIBoard = null;
    public GameObject currentShot = null;

    [System.Serializable]
    public struct ShipContainer {public List<Vector3> LifeSpots; public string ShipName;}

    public List<ShipContainer> playerShips;
    private List<ShipContainer> playerShipsThatCouldBeDead;
    public int currentPlayerShipIndex = -1;
    public double AIHitPercentange = 0.8;
    private int existingShipLocation = -1;

    public bool stopCheck = false;

    private bool ShipAlreadyExists(string shipName) {
      for (int i = 0; i < playerShips.Count; ++i) {
        if (playerShips[i].ShipName == shipName) {
          existingShipLocation = i;
          return true;
        }
      }
      existingShipLocation = -1;
      return false;
    }


    private int getExistingShipLocation () {
      return existingShipLocation;
    }

    public void addPlayerShip(List<Vector3> shipLifeSpots, string shipName) {
      if (ShipAlreadyExists(shipName)) {
        ShipContainer existShip = playerShips[getExistingShipLocation()];
        existShip.LifeSpots = shipLifeSpots;
        existShip.ShipName = shipName;
        playerShipsThatCouldBeDead = playerShips;
        return;
      }
      ShipContainer tempShip;
      tempShip.LifeSpots = shipLifeSpots;
      tempShip.ShipName = shipName;
      playerShips.Add(tempShip);
      playerShipsThatCouldBeDead = playerShips;
    }

    void chooseRandomIndex() {
      currentPlayerShipIndex = UnityEngine.Random.Range(0, playerShips.Count);
    }

    void removeCurrentShip() {
      if (currentPlayerShipIndex >= 0 && currentPlayerShipIndex < playerShips.Count) {
        playerShips.RemoveAt(currentPlayerShipIndex);
        currentPlayerShipIndex = -1;
      }
    }

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

    // Aircraft = 5
    // Battleship = 4
    // Cruiser = 3
    // Submarine = 3
    // RadarShip = 2
    private void setAIShips()
    {
        randomSpawnShip(5,"AircraftCarrierEnemy");
        randomSpawnShip(4,"BattleshipEnemy");
        randomSpawnShip(3, "SubmarineEnemy");
        randomSpawnShip(3, "WarshipEnemy");
        randomSpawnShip(2, "LandingCraftEnemy");
    }

    private void randomSpawnShip(int shipLength, string shipName)
    {
        int initPosition = UnityEngine.Random.Range(0, 100);
        int rotation = UnityEngine.Random.Range(0, 4);
        List<Vector3> shipNodes = new List<Vector3>();

        switch(rotation)
        {
            case 0: //left
                if (checkLeftPlacement(initPosition, shipLength))
                {
                    for (int shipPos = 0; shipPos < shipLength; shipPos++)
                    {
                        AIBoard.GetComponent<Board>().nodes[initPosition - shipPos].GetComponent<Node>().occupied = true;
                        GameObject currentNode = AIBoard.GetComponent<Board>().nodes[initPosition - shipPos];
                        shipNodes.Add(new Vector3(0f, currentNode.tag[3] - 'A', currentNode.name[6] - '0'));
                        //AIBoard.GetComponent<Board>().nodes[initPosition - shipPos].GetComponent<Node>().setNodeToHitState(); //REMOVE
                    }
                    if (shipName != "")
                    {
                        GameObject.Find(shipName).GetComponent<ShipController>().SetShip(shipNodes);
                        Debug.Log(shipNodes + " Called SetShip in AI.cs");
                        for (int i = 0; i < shipNodes.Count; i++)
                            Debug.Log("Alive: " + shipNodes[i].x + " Row " + (char)(shipNodes[i].y + 'A') + " Index " + shipNodes[i].z);
                    }
                }
                else
                    randomSpawnShip(shipLength, shipName);
            break;

            case 1: //right
                if (checkRightPlacement(initPosition, shipLength))
                {
                    for (int shipPos = 0; shipPos < shipLength; shipPos++)
                    {
                        AIBoard.GetComponent<Board>().nodes[initPosition + shipPos].GetComponent<Node>().occupied = true;
                        GameObject currentNode = AIBoard.GetComponent<Board>().nodes[initPosition + shipPos];
                        shipNodes.Add(new Vector3(0f, currentNode.tag[3] - 'A', currentNode.name[6] - '0'));
                        //AIBoard.GetComponent<Board>().nodes[initPosition + shipPos].GetComponent<Node>().setNodeToHitState(); //REMOVE
                    }
                    if (shipName != "")
                    {
                        GameObject.Find(shipName).GetComponent<ShipController>().SetShip(shipNodes);
                        Debug.Log(shipNodes + " Called SetShip in AI.cs");
                        for (int i = 0; i < shipNodes.Count; i++)
                            Debug.Log("Alive: " + shipNodes[i].x + " Row " + (char)(shipNodes[i].y + 'A') + " Index " + shipNodes[i].z);
                    }
                }
                else
                    randomSpawnShip(shipLength, shipName);
                break;

            case 2: //up
                if (checkUpPlacement(initPosition, shipLength))
                {
                    for (int shipPos = 0; shipPos < shipLength; shipPos++)
                    {
                        AIBoard.GetComponent<Board>().nodes[initPosition - 10 * shipPos].GetComponent<Node>().occupied = true;
                        GameObject currentNode = AIBoard.GetComponent<Board>().nodes[initPosition - 10 * shipPos];
                        shipNodes.Add(new Vector3(0f, currentNode.tag[3] - 'A', currentNode.name[6] - '0'));
                        //AIBoard.GetComponent<Board>().nodes[initPosition - 10 * shipPos].GetComponent<Node>().setNodeToHitState(); //REMOVE
                    }
                    if (shipName != "")
                    {
                        GameObject.Find(shipName).GetComponent<ShipController>().SetShip(shipNodes);
                        Debug.Log(shipNodes + " Called SetShip in AI.cs");
                        for (int i = 0; i < shipNodes.Count; i++)
                            Debug.Log("Alive: " + shipNodes[i].x + " Row " + (char)(shipNodes[i].y + 'A') + " Index " + shipNodes[i].z);
                    }
                }
                else
                    randomSpawnShip(shipLength, shipName);
                break;

            case 3: //down
                if (checkDownPlacement(initPosition, shipLength))
                {
                    for (int shipPos = 0; shipPos < shipLength; shipPos++)
                    {
                        AIBoard.GetComponent<Board>().nodes[initPosition + 10 * shipPos].GetComponent<Node>().occupied = true;
                        GameObject currentNode = AIBoard.GetComponent<Board>().nodes[initPosition + 10 * shipPos];
                        shipNodes.Add(new Vector3(0f, currentNode.tag[3] - 'A', currentNode.name[6] - '0'));
                        //AIBoard.GetComponent<Board>().nodes[initPosition + 10 * shipPos].GetComponent<Node>().setNodeToHitState(); //REMOVE
                    }
                    if (shipName != "")
                    {
                        GameObject.Find(shipName).GetComponent<ShipController>().SetShip(shipNodes);
                        Debug.Log(shipNodes + " Called SetShip in AI.cs");
                        for (int i = 0; i < shipNodes.Count; i++)
                            Debug.Log("Alive: " + shipNodes[i].x + " Row " + (char)(shipNodes[i].y + 'A') + " Index " + shipNodes[i].z);
                    }
                }
                else
                    randomSpawnShip(shipLength, shipName);
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
                AIBoard.GetComponent<Board>().nodes[initPos - posCheck].GetComponent<Node>().occupied ||
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
                AIBoard.GetComponent<Board>().nodes[initPos + posCheck].GetComponent<Node>().occupied ||
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
                AIBoard.GetComponent<Board>().nodes[initPos - 10*posCheck].GetComponent<Node>().occupied)
                return false;
        }
        return true;
    }

    private bool checkDownPlacement(int initPos, int shipLength)
    {
        for (int posCheck = 0; posCheck < shipLength; posCheck++)
        {
            if ((initPos + 10*posCheck) > 99 ||
                AIBoard.GetComponent<Board>().nodes[initPos + 10*posCheck].GetComponent<Node>().occupied)
                return false;
        }
        return true;
    }

    private void generateNextShot()
    {
        currentShot = Instantiate(SHOTPREFAB, new Vector3(1131.301f, 365.357f, 1461.777f), Quaternion.identity);
        stopCheck = false;
    }

    private int getCoord (int row, int col) {
      return row * 10 + col;
    }


    private void fireAtPlayer()
    {
        if (currentPlayerShipIndex == -1)
          chooseRandomIndex();

        if (currentPlayerShipIndex >= playerShips.Count) {
          Debug.Log("NO SPOTS FOR AI TO CHOOSE. RETURNING.");
          return;
        }

        int coord = -1;

        bool shouldHitAShip = (UnityEngine.Random.Range(0,100) <= 100 * AIHitPercentange) ? true : false;
        bool atLastIndexOfShip = false;
        if (shouldHitAShip) {
          for (int i = 0; i < playerShips[currentPlayerShipIndex].LifeSpots.Count; ++i) {
            // if spot not dead
            if (playerShips[currentPlayerShipIndex].LifeSpots[i].x == 0) {
              if (i == playerShips[currentPlayerShipIndex].LifeSpots.Count -1) {
                atLastIndexOfShip = true;
              }
              coord = getCoord((int)playerShips[currentPlayerShipIndex].LifeSpots[i].y, (int)playerShips[currentPlayerShipIndex].LifeSpots[i].z);
              playerShips[currentPlayerShipIndex].LifeSpots[i] = new Vector3(1, playerShips[currentPlayerShipIndex].LifeSpots[i].y, playerShips[currentPlayerShipIndex].LifeSpots[i].z);
              break;
            }
          }
        } else {
          int tempRow = UnityEngine.Random.Range(0, 10);
          int tempCol = UnityEngine.Random.Range(0, 10);
          bool foundEmpty = false;
          for (int j = 0; j < 500; ++j) {
            if (foundEmpty)
              break;
            for (int i = 0; i < playerShipsThatCouldBeDead.Count; ++i) {
              bool notEmpty = false;
              for (int place = 0; place < playerShipsThatCouldBeDead[i].LifeSpots.Count; ++place) {
                if (tempRow == playerShipsThatCouldBeDead[i].LifeSpots[place].y && tempCol == playerShipsThatCouldBeDead[i].LifeSpots[place].z) {
                  notEmpty = true;
                }
                if (notEmpty)
                  break;
              }
              if (!notEmpty) {
                foundEmpty = true;
              }
            }
            tempRow = UnityEngine.Random.Range(0, 10);
            tempCol = UnityEngine.Random.Range(0, 10);
          }

          coord = getCoord(tempRow, tempCol);
          Debug.Log("Found Empty Spot At: " + coord);
        }

        if (coord == -1 || coord >= 100) {
          Debug.Log("You should not have gotten here... Leaving..");
            fireAtPlayer();
          return;
        }

        List <GameObject> playerNodes = PlayerBoard.GetComponent<Board>().nodes;
        Debug.Log ("******************** FIRE AT TRISTAN. **************************");

        // bool hit = playerNodes[coord].GetComponent<Node>().hit;
        // bool miss = !playerNodes[coord].GetComponent<Node>().hit;
        // Debug.Log("Generated shot coords at " + coord + "hit status: " + hit + "miss status: " + miss);

        // if (hit)
        // {
        if (shouldHitAShip) {
            playerNodes[coord].GetComponent<Node>().state = true;
            playerNodes[coord].GetComponent<Node>().occupied = true;
            Debug.Log("AI Hit Coord: " + coord);
          }
          else {
            playerNodes[coord].GetComponent<Node>().state = false;
            playerNodes[coord].GetComponent<Node>().miss = true;
            playerNodes[coord].GetComponent<Node>().hit = false;
            Debug.Log("AI missed Coord: " + coord);
          }
            //playerNodes[coord].GetComponent<Node>().setNodeToHitState();
            playerNodes[coord].GetComponent<Node>().collidingObject = SHOTPREFAB;

            if (atLastIndexOfShip) {
              Debug.Log("AI Removing local copy of: " + playerShips[currentPlayerShipIndex].ShipName);
              removeCurrentShip();
              chooseRandomIndex();
            }
        // }
        // else if(miss)
        // {
        //     //playerNodes[coord].GetComponent<Node>().setNodeToMissState();
        //     playerNodes[coord].GetComponent<Node>().state = true;
        //     // playerNodes[coord].GetComponent<Node>().occupied = true;
        //     playerNodes[coord].GetComponent<Node>().collidingObject = SHOTPREFAB;
        //     Debug.Log("AI MISSED!");
        // }

    }

	// Update is called once per frame
	void Update () {
        if (PlayerBoard == null && !GameObject.Find("StartBlock").GetComponent<StartGame>().editMode)
        {
            Debug.Log("AI INITIATED");
            getBoardReferences();
            setAIShips();
            PlayerBoard.GetComponent<Board>().MoveToPosition(PlayerBoard.transform, PlayerBoard.transform.position + new Vector3(4f, 0f, 3f), 4);
            generateNextShot();
        }

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
