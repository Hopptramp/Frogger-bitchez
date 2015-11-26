using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerMain : MonoBehaviour 
{
	private int playerNumber;
	private XInputControl inputControl;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void SetupPlayer(int _playerNumber, XInputControl _inputControl)
	{
		playerNumber = _playerNumber;
		inputControl = _inputControl;
		GetComponent<PlayerMovement> ().SetInputControl (inputControl);
	}


}
