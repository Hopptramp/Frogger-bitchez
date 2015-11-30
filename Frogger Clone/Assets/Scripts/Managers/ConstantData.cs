//#define XINPUT_CONTROL

using UnityEngine;
using System.Collections;

#if XINPUT_CONTROL
using XInputDotNetPure;
#endif

public class ConstantData : MonoBehaviour 
{
	public int MAX_PLAYERS = 20;
	public int MAX_CONTROLLERS;
	public InputControl[] inputControllers;
	public int numOfPlayers;

	public GameObject[] players; 
	
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
	public void SetupInputControl(int _activePlayers, InputControl[] _allInput, bool[] _readyPlayers)
	{
		inputControllers = new InputControl[_activePlayers];
		numOfPlayers = _activePlayers;
		players = new GameObject[numOfPlayers];
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
					inputControllers[counter] = _allInput[(int)Mathf.Floor(i / 2)];
					inputControllers[counter].isDpad = true;
					++counter;
				}
				//if the player is controlling the buttons
				else
				{
					inputControllers[counter] = _allInput[(int)Mathf.Floor(i / 2)];
					inputControllers[counter].isDpad = false;
					++counter;
				}
			}
		}
	}
}
