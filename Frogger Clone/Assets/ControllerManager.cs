using UnityEngine;
using System.Collections;
using XInputDotNetPure;

//Structure to hold all the controller data
[System.Serializable]
public struct XInputControl
{
	public bool isActive;
	public GamePadState state;
	public GamePadState prevState;
	public PlayerIndex playerIndex;
	public bool isDpad; //Means nothing till process by constant data
}

public class ControllerManager : MonoBehaviour 
{
	private int MAX_PLAYERS;
	private int MAX_CONTROLLERS;
	
	public XInputControl[] XInputPlayers; //Contains up to date controller data including non active controllers

	public bool[] ready; //Contains the data that represents what players are ready

	// Use this for initialization
	void Start () 
	{
		MAX_PLAYERS = GameObject.FindGameObjectWithTag("GlobalConstant").GetComponent<ConstantData>().MAX_PLAYERS; //GET FROM CONSTANT DATA
		MAX_CONTROLLERS = MAX_PLAYERS / 2;
		XInputPlayers = new XInputControl[MAX_PLAYERS/2];
		ready = new bool[MAX_PLAYERS];
	}
	
	// Update is called once per frame
	void Update () 
	{
		SetupXInputControl ();
		for(int i = 0; i < MAX_CONTROLLERS; ++i)
		{
			//If controller is active
			if(XInputPlayers[i].isActive)
			{
				//Update states of the controller
				XInputPlayers[i].prevState = XInputPlayers[i].state;
				XInputPlayers[i].state = GamePad.GetState(XInputPlayers[i].playerIndex);

				// If left shoulder button has being pressed and not ready
				if (XInputPlayers[i].prevState.Buttons.LeftShoulder == ButtonState.Released)
				{
					if(XInputPlayers[i].state.Buttons.LeftShoulder  == ButtonState.Pressed)
					{
						if(ready[i * 2] == false)
						{
							ready[i * 2] = true;
						}
					}
				}
				//If right shoulder button has being pressed and not ready
				if (XInputPlayers[i].prevState.Buttons.RightShoulder == ButtonState.Released)
				{
					if(XInputPlayers[i].state.Buttons.RightShoulder == ButtonState.Pressed)
					{
						if(ready[(i * 2) + 1] == false)
						{
							ready[(i * 2) + 1] = true;
						}
					}
				}
			}
		}
	}

	//Attempt to find out if any new controllers have being plugged in or if any have being plugged out
	void SetupXInputControl()
	{
		// Find a PlayerIndex, for a single player game
		// Will find the first controller that is connected ans use it
		for (int i = 0; i < MAX_CONTROLLERS; ++i) 
		{
			//If controller previously wasn't active
			if(!XInputPlayers[i].isActive)
			{
				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				GamePadState testState = GamePad.GetState (testPlayerIndex);
				//And the controller is active now
				if (testState.IsConnected) 
				{
					XInputPlayers[i].isActive = true;
					XInputPlayers[i].state = testState;
					XInputPlayers[i].playerIndex = testPlayerIndex;
				}
			}
			//If the controller was active
			else
			{
				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				GamePadState testState = GamePad.GetState (testPlayerIndex);
				//And the controller isn't active anymore
				if (!testState.IsConnected) 
				{
					XInputPlayers[i].isActive = false;
					//XInputPlayers[i].state;
					//XInputPlayers[i].playerIndex;
				}
			}
		}
	}

	//Prepares to move to constant data
	void OutputToConstantData()
	{
		int activePlayers = 0;
		//Determine how many ready players there are
		for(int i = 0; i < MAX_PLAYERS; ++i)
		{
			if(ready[i])
			{
				++activePlayers;
			}
		}

		GameObject.FindGameObjectWithTag ("GlobalConstant").GetComponent<ConstantData> ().SetupXInputControl (activePlayers, XInputPlayers, ready);

	}
}
