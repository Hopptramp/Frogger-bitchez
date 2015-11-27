using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{
	private int NUM_OF_PLAYERS;
	private GameObject[] players;
	public GameObject playerPrefab;

	// Use this for initialization
	void Start () 
	{
	
	}

	// Update is called once per frame
	void Update () 
	{
	
	}

	public void SetupPlayers(int _numPlayers)
	{
		NUM_OF_PLAYERS = _numPlayers;
		players = new GameObject[NUM_OF_PLAYERS];

		int WIDTH = GetComponent<LevelTileManager>().columns / 2;
		int HEIGHT = GetComponent<LevelTileManager>().rows;
		
		int multiplier;
		if(NUM_OF_PLAYERS < 2)
		{
			multiplier = 0;
		}
		else
		{
			multiplier = (WIDTH - (WIDTH/10)) / (int)Mathf.Ceil((float)NUM_OF_PLAYERS/2);
		}

		for (int i = 0; i < NUM_OF_PLAYERS; ++i) 
		{
			Vector3 spawnPoint = new Vector3((((-NUM_OF_PLAYERS/2)+ i + 0.5f)*multiplier),-HEIGHT/2,0); //Algorithm to determine location of player
			players[i] = Instantiate(playerPrefab, spawnPoint, Quaternion.identity) as GameObject;
			players[i].GetComponent<PlayerMain>().SetupPlayer(i + 1, GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().inputControllers[i]);
			players[i].GetComponent<PlayerMovement>().SetMovementIsPaused(true);
		}
	}

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
}
