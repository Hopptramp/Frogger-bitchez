﻿using UnityEngine;
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
		if (Input.GetKey(KeyCode.A))
		{
			players[0].SetActive(true);
			
			players[0].transform.position = new Vector3 (gameObject.transform.position.x, -37, 0);
		}
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
			Vector3 spawnPoint = new Vector3((((-NUM_OF_PLAYERS/2)+ i + 0.5f)*multiplier),-HEIGHT/2,0); 
			//Instantiate and set starting parameters of player
			players[i] = Instantiate(playerPrefab, spawnPoint, Quaternion.identity) as GameObject;
			players[i].GetComponent<PlayerMain>().SetupPlayer(i + 1, GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().inputControllers[i]);
			players[i].GetComponent<PlayerMovement>().SetMovementIsPaused(true);
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
