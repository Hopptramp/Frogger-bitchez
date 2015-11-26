using UnityEngine;
using System.Collections;

public class SceneStartup : MonoBehaviour 
{
	private bool hasGameBegun = false;
	private float timeOnSetupFinished;
	public float startTimer = 3.0f;
	public GameObject playerPrefab;

	private int NUM_OF_PLAYERS;
	// Use this for initialization
	void Start () 
	{
		NUM_OF_PLAYERS = GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().numOfPlayers;
		SetupPlayers ();
		timeOnSetupFinished = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the playable game has not yet started
		if(hasGameBegun == false)
		{
			//Determine when the starting timer has finished
			if(Time.realtimeSinceStartup - timeOnSetupFinished > startTimer)
			{
				GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
				for(int i = 0; i < NUM_OF_PLAYERS; ++i)
				{
					players[i].GetComponent<PlayerMovement> ().SetMovementIsPaused(false);
					players[i].GetComponent<PlayerMovement> ().SetStartDirection();
				}

				hasGameBegun = true;
			}
		}
	}	
	
	void SetupPlayers()
	{
		for (int i = 0; i < NUM_OF_PLAYERS; ++i) 
		{
			int WIDTH = 10; //should be taken from level somehow
			int multiplier;
			if(NUM_OF_PLAYERS < 2)
			{
				multiplier = 0;
			}
			else
			{
				multiplier = (WIDTH - (WIDTH/10)) / (NUM_OF_PLAYERS/2);
			}
			Vector3 spawnPoint = new Vector3((((-NUM_OF_PLAYERS/2)+ i + 0.5f)*multiplier),0,0); //Algorithm to determine location of player
			GameObject player = Instantiate(playerPrefab, spawnPoint, Quaternion.identity) as GameObject;
			player.GetComponent<PlayerMain>().SetupPlayer(i + 1, GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().inputControllers[i]);
			player.GetComponent<PlayerMovement>().SetMovementIsPaused(true);
		}
	}
}
