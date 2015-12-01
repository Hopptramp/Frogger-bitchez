using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{
	private int NUM_OF_PLAYERS;
	private GameObject[] players;
	public GameObject playerPrefab;
	public GameObject winZonePrefab;
    public bool instantReset = true;
	private int scorePosition = 1;

    // Use this for initialization
    void Start () 
	{
	
	}

	// Update is called once per frame
	void Update () 
	{

	}

    public void tryEndLevel(GameObject _player)
    {
        // set the player finished to be true
        _player.GetComponent<PlayerMain>().isFinished = true;
		// tell the player where they placed + time
		GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().scores[_player.GetComponent<PlayerMain>().playerNumber - 1] = scorePosition;
		GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().timeTaken[_player.GetComponent<PlayerMain>().playerNumber - 1] = _player.GetComponent<PlayerMovement>().timeTaken;
		GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().timesDied[_player.GetComponent<PlayerMain>().playerNumber - 1] = _player.GetComponent<PlayerMovement>().timesDied;
		GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().instantRespawn = instantReset;
		//increment the score position ready for the next player
		++scorePosition;


        for (int i = 0; i < NUM_OF_PLAYERS; ++i)
        {
            if (players[i].GetComponent<PlayerMain>().isFinished == false)
            {
                return;
            }
        }
        updateConstantData();
        Application.LoadLevel("EndScene");
    }

	public void updateConstantData()
	{
		GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().players = players;
	}

	public void SetupPlayers(int _numPlayers)
	{
		NUM_OF_PLAYERS = _numPlayers;
		players = new GameObject[NUM_OF_PLAYERS];

		//Get assist values that will spawn the players in the correct position
		int WIDTH = GetComponent<LevelTileManager>().columns / 2;
		int HEIGHT = GetComponent<LevelTileManager>().rows;

		//Multiplier to determine how far left or right along the screen to spawn the player
		int multiplier;
		if(NUM_OF_PLAYERS < 2)
		{
			multiplier = 0;
		}
		else
		{
			multiplier = (WIDTH - (WIDTH/10)) / (int)Mathf.Ceil((float)NUM_OF_PLAYERS/2);
		}

		//Setup each player
		for (int i = 0; i < NUM_OF_PLAYERS; ++i) 
		{
			//Algorithm to determine location of player
			Vector3 spawnPoint = new Vector3 ((((-NUM_OF_PLAYERS / 2) + i + 0.5f) * multiplier), -HEIGHT / 2, 0); 

			//Setup players from already existing 
			if(GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().setupPlayersFromData)
			{
				players [i] = Instantiate (GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().players[i], spawnPoint, Quaternion.identity) as GameObject;
			}
			//Instantiate and set starting parameters of player from scratch
			else
			{
				players [i] = Instantiate (playerPrefab, spawnPoint, Quaternion.identity) as GameObject;
				players [i].GetComponent<PlayerMain> ().SetupPlayer (i + 1, GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().inputControllers [i]);
			}
			players [i].GetComponent<PlayerMovement> ().SetMovementIsPaused (true);

			//Set up winzones to be in line with players
			LevelTileManager tileMan = GameObject.Find ("Managers").GetComponent<LevelTileManager> ();
			float y = ((tileMan.rows/2) + 0.5f - (float)tileMan.topBound) + 1.9f;
			Vector3 winLocation = new Vector3 (players [i].transform.position.x, y, 0);
			GameObject winZone = Instantiate (winZonePrefab, winLocation, Quaternion.identity) as GameObject;
			winZone.transform.parent = tileMan.mapHolder;
		}

		for (int i = 0; i < NUM_OF_PLAYERS - 1; ++i) 
		{
			for(int j = i + 1; j < NUM_OF_PLAYERS; ++j)
			{
				Physics2D.IgnoreCollision(players[i].GetComponent<BoxCollider2D>(), players[j].GetComponent<BoxCollider2D>());
			}
		}
	}

	//Enables and players and sets initial direction movement
	public void PlayersStart()
	{
		for(int i = 0; i < NUM_OF_PLAYERS; ++i)
		{
			players[i].GetComponent<PlayerMovement> ().SetMovementIsPaused(false);
			players[i].GetComponent<PlayerMovement> ().SetStartDirection();
		}
	}

	public GameObject[] GetAllPlayers()
	{
		return players;
	}	

	public void KillPlayer(int _player)
	{
		players [_player].GetComponent<PlayerMain> ().OnDeath ();
	}
}
