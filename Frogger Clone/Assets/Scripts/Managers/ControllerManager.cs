//#define XINPUT_CONTROL

using UnityEngine;
using System.Collections;

#if XINPUT_CONTROL
using XInputDotNetPure;
#endif

//Structure to hold all the controller data
public struct InputControl
{

#if XINPUT_CONTROL
	public bool isActive;
	public GamePadState state;
	public GamePadState prevState;
	public PlayerIndex playerIndex;
#endif
#if !XINPUT_CONTROL
	public int controllerIndex;
	public bool[] prevState;
#endif
	public bool isDpad; //Means nothing till process by constant data

}

public class ControllerManager : MonoBehaviour 
{

	private int MAX_PLAYERS;
	private int MAX_CONTROLLERS;
	
	private InputControl[] inputControllers; //Contains up to date controller data including non active controllers

	private bool[] ready; //Contains the data that represents what players are ready

	public string nextScene;
	private GameObject visualDisplay;

	// Use this for initialization
	void Start () 
	{
		MAX_PLAYERS = GameObject.FindGameObjectWithTag("ConstantData").GetComponent<ConstantData>().MAX_PLAYERS; //GET FROM CONSTANT DATA
		MAX_CONTROLLERS = MAX_PLAYERS / 2;
		inputControllers = new InputControl[MAX_CONTROLLERS];
		ready = new bool[MAX_PLAYERS];
		visualDisplay = GameObject.Find ("VisualDisplay");

#if !XINPUT_CONTROL
		for(int i = 0; i < MAX_CONTROLLERS; ++i)
		{
			inputControllers[i].controllerIndex = i + 1;
		}
#endif

	}

	// Update is called once per frame
	void Update () 
	{

#if XINPUT_CONTROL
		SetupXInputControl ();

		for(int i = 0; i < MAX_CONTROLLERS; ++i)
		{
			//If controller is active
			if(inputControllers[i].isActive)
			{
				//Update states of the controller
				inputControllers[i].prevState = inputControllers[i].state;
				inputControllers[i].state = GamePad.GetState(inputControllers[i].playerIndex);

				// If left shoulder button has being pressed and not ready
				if (inputControllers[i].prevState.Buttons.LeftShoulder == ButtonState.Released)
				{
					if(inputControllers[i].state.Buttons.LeftShoulder  == ButtonState.Pressed)
					{
						if(ready[i * 2] == false)
						{
							ready[i * 2] = true;
							visualDisplay.GetComponent<JoinDisplay>().SetReadyDisplay(true, i * 2);
						}
					}
				}
				//If right shoulder button has being pressed and not ready
				if (inputControllers[i].prevState.Buttons.RightShoulder == ButtonState.Released)
				{
					if(inputControllers[i].state.Buttons.RightShoulder == ButtonState.Pressed)
					{
						if(ready[(i * 2) + 1] == false)
						{
							ready[(i * 2) + 1] = true;
							visualDisplay.GetComponent<JoinDisplay>().SetReadyDisplay(true, (i * 2)+1);
						}
					}
				}
			}
		}

		//If master controller is connected
		if(inputControllers[0].state.IsConnected)
		{
			//Master controller presses start to automatically begin game
			if(inputControllers[0].state.Buttons.Start == ButtonState.Pressed)
			{
				OutputToConstantData();
				Application.LoadLevel (nextScene);
			}
		}
#endif
#if !XINPUT_CONTROL
		//Loop through all possible controllers
		for(int i = 0; i < 5; ++i)
		{
			//If left shoulder button has been pressed and not ready
			if (Input.GetButtonDown("bumperleft" + (i + 1)))
			{
				if(ready[i * 2] == false)
				{
					ready[i * 2] = true;
					visualDisplay.GetComponent<JoinDisplay>().SetReadyDisplay(true, i * 2);
				}
			}
			//If right shoulder button has being pressed and not ready
			if (Input.GetButtonDown("bumperright" + (i + 1)))
			{
				if(ready[(i * 2) + 1] == false)
				{
					ready[(i * 2) + 1] = true;
					visualDisplay.GetComponent<JoinDisplay>().SetReadyDisplay(true, (i * 2) + 1);
				}
			}
		}

		//If master controller is connected
		if(Input.GetButtonDown("start1"))
		{
			OutputToConstantData();
			Application.LoadLevel (nextScene);
		}
#endif

	}

#if XINPUT_CONTROL
	//Attempt to find out if any new controllers have being plugged in or if any have being plugged out
	void SetupXInputControl()
	{
		// Find a PlayerIndex, for a single player game
		// Will find the first controller that is connected ans use it
		for (int i = 0; i < MAX_CONTROLLERS; ++i) 
		{
			//If controller previously wasn't active
			if(!inputControllers[i].isActive)
			{
				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				GamePadState testState = GamePad.GetState (testPlayerIndex);
				//And the controller is active now
				if (testState.IsConnected) 
				{
					inputControllers[i].isActive = true;
					inputControllers[i].state = testState;
					inputControllers[i].playerIndex = testPlayerIndex;
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
					inputControllers[i].isActive = false;
					//XInputPlayers[i].state;
					//XInputPlayers[i].playerIndex;
				}
			}
		}
	}
#endif

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

		GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().SetupInputControl (activePlayers, inputControllers, ready);

	}
}
