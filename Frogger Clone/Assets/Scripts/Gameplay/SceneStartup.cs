using UnityEngine;
using System.Collections;

public class SceneStartup : MonoBehaviour 
{
	private bool hasGameBegun = false;
	private float timeOnSetupFinished;

	private DisplayScript displayScript;
	private GameObject playerPrefab;
	private PlayerManager playerMan;

	private int NUM_OF_PLAYERS;

	public float startTimer = 3.0f;

	// Use this for initialization
	void Start () 
	{
		//Get number of players and tell player manager to to setup the players
		NUM_OF_PLAYERS = GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().numOfPlayers;
		playerMan = GetComponent<PlayerManager> ();
		playerMan.SetupPlayers (NUM_OF_PLAYERS);

		//Get time when switch to game scene
		timeOnSetupFinished = Time.realtimeSinceStartup;

		//Setup the starting timer on the display script
		displayScript = GameObject.Find ("DisplayCanvas").GetComponent<DisplayScript> ();
		displayScript.SetStartTime (startTimer);
		displayScript.UpdateTimer (Time.realtimeSinceStartup - timeOnSetupFinished);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the playable game has not yet started
		if(hasGameBegun == false)
		{
			displayScript.UpdateTimer (Time.realtimeSinceStartup - timeOnSetupFinished);
			//Determine when the starting timer has finished so players can be set to start moving
			if(Time.realtimeSinceStartup - timeOnSetupFinished > startTimer)
			{
				displayScript.UpdateTimer (startTimer);
				playerMan.PlayersStart();
				hasGameBegun = true;
			}
		}
	}	
	

}
