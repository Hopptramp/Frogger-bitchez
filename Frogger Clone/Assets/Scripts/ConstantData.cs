using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class ConstantData : MonoBehaviour 
{
	public int MAX_PLAYERS = 20;
	public int MAX_CONTROLLERS;
	public XInputControl[] XInputPlayers;
	public int numOfPlayers;
	
	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad (this);
		MAX_CONTROLLERS = MAX_PLAYERS / 2;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	//Takes the current controller structure and amount of ready structure and cuts it done to only the useful elements to set up the players in the main game
	public void SetupXInputControl(int _activePlayers, XInputControl[] _allXInput, bool[] _readyPlayers)
	{
		XInputPlayers = new XInputControl[_activePlayers];
		numOfPlayers = _activePlayers;
		int counter = 0;   //Counter which is used to place elements into the new XInputPlayers structure

		//For all players
		for (int i = 0; i < MAX_PLAYERS; ++i) 
		{
			//If they are ready
			if(_readyPlayers[i])
			{
				//If the player is controlling the dpad
				if((i % 2) == 0)
				{
					XInputPlayers[counter] = _allXInput[(int)Mathf.Floor(i / 2)];
					XInputPlayers[counter].isDpad = true;
					++counter;
				}
				//if the player is controlling the buttons
				else
				{
					XInputPlayers[counter] = _allXInput[(int)Mathf.Floor(i / 2)];
					XInputPlayers[counter].isDpad = false;
					++counter;
				}
			}
		}
	}
}
