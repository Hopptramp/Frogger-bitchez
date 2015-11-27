//#define XINPUT_CONTROL

using UnityEngine;
using System.Collections;

#if XINPUT_CONTROL
using XInputDotNetPure;
#endif


public class PlayerMain : MonoBehaviour 
{
	private int playerNumber;
	private InputControl inputControl;
	private bool isAlive = true;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void SetupPlayer(int _playerNumber, InputControl _inputControl)
	{
		playerNumber = _playerNumber;
		inputControl = _inputControl;
#if !XINPUT_CONTROL
		inputControl.prevState = new bool[4];
#endif
		GetComponent<PlayerMovement> ().SetInputControl (inputControl);
	}

	public void OnDeath()
	{
		if (isAlive == true) 
		{
			isAlive = false;
			//GetComponent<PlayerMovement> ().OnDeath ();
		}
	}
}
