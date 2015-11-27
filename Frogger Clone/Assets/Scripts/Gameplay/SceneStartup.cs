using UnityEngine;
using System.Collections;

public class SceneStartup : MonoBehaviour 
{
	private bool hasGameBegun = false;
	private float timeOnSetupFinished;
	public float startTimer = 3.0f;

	private DisplayScript displayScript;
	private GameObject playerPrefab;
	private PlayerManager playerMan;

	private int NUM_OF_PLAYERS;
	// Use this for initialization
	void Start () 
	{
		NUM_OF_PLAYERS = GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().numOfPlayers;
		playerMan = GetComponent<PlayerManager> ();
		playerMan.SetupPlayers (NUM_OF_PLAYERS);

		timeOnSetupFinished = Time.realtimeSinceStartup;

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
			//Determine when the starting timer has finished
			if(Time.realtimeSinceStartup - timeOnSetupFinished > startTimer)
			{
				displayScript.UpdateTimer (startTimer);
				playerMan.PlayersStart();
				hasGameBegun = true;
			}
		}
	}	
	

}
