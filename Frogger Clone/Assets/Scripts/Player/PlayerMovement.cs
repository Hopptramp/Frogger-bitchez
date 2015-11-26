using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XInputDotNetPure;

public class PlayerMovement : MonoBehaviour 
{
	//public GameObject frog;
	private Animator anim;
	private XInputControl inputControl;


	public float movementSpeed = 5.0f;
	public float movingDistance = 0;
	public float completeMoveDistance = 5.0f;

	public bool completedMove = false;
	private bool moving = false;
	private bool isMovementPaused = false;
	
	public AudioClip moveSound1;
	public AudioClip gameOverSound;
	//public bool movingHorizontally = false;
	//public bool movingVertically = false;

	enum Direction
	{
		UP = 0,
		DOWN,
		LEFT,
		RIGHT
	}

	Direction direction;

	//Use ENUM 
	/*int stateIdle = 0;
	int stateUp = 1;
	int stateDown = 2;
	int stateLeft = 3;
	int stateRight = 4;*/

	public int currentState;
	int oldState;


	//float inputHor;
	//float inputVert;

	void awake () 
	{

	}

	void Start () 
	{

		anim = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update () 
	{
		//Update the controller states
		inputControl.prevState = inputControl.state;
		inputControl.state = GamePad.GetState(inputControl.playerIndex);

		//inputHor = Input.GetAxis ("Horizontal");
		//inputVert = Input.GetAxis ("Vertical");
		if(!isMovementPaused)
		{
			ApplyDirection ();
			fullMove();
			ApplyMovement ();
			ApplyAnimations ();
		}
	}

	void ApplyDirection ()
	{
		//set moving to false and if input is acquired set to true
		moving = false;
		//If up is pressed
		if (UP (inputControl.state)) 
		{
			//and up was not getting pressed last update
			if(!UP (inputControl.prevState))
			{
				//and not already heading up
				if(direction != Direction.UP)
				{
					direction = Direction.UP;
				}
			}
			moving = true;
		}
		//If down is pressed
		if (DOWN (inputControl.state)) 
		{
			//and down was not getting pressed last update
			if(!DOWN (inputControl.prevState))
			{
				//and not already heading heading
				if(direction != Direction.DOWN)
				{
					direction = Direction.DOWN;
				}
			}
			moving = true;
		}
		//If left is pressed
		if (LEFT (inputControl.state)) 
		{
			//and left was not getting pressed last update
			if(!LEFT (inputControl.prevState))
			{
				//and not already heading left
				if(direction != Direction.LEFT)
				{
					direction = Direction.LEFT;
				}
			}
			moving = true;
		}
		//If right is pressed
		if (RIGHT (inputControl.state)) 
		{
			//and right was not getting pressed last update
			if(!RIGHT (inputControl.prevState))
			{
				//and not already heading right
				if(direction != Direction.RIGHT)
				{
					direction = Direction.RIGHT;
				}
			}
			moving = true;
		}
	
		/*//checks if moving in any direction
		if ((_inputHor != 0) || (_inputVert != 0)) {
			moving = true;
		} 
		else {
			moving = false;
		}

		//checks if moving vertically
		if (_inputVert != 0) {
			movingVertically = true;
			movingHorizontally = false;

			//check if moving up
				if(_inputVert > 0) {

				facingLeft = false;
				facingRight = false;
				facingDown = false;
				facingUp = true;
			}
			//check if moving up
			if(_inputVert < 0) {

				facingLeft = false;
				facingRight = false;
				facingDown = true;
				facingUp = false;
			}
		} 
		//not moving vertically
		else {
			movingVertically = false;
		}

		//checks if moving horizontally
		if (_inputHor != 0) {
			
			movingHorizontally = true;
			movingVertically = false;
			//checks if moving right
			if(_inputHor > 0) {
				
				facingLeft = false;
				facingRight = true;
				facingDown = false;
				facingUp = false;
			}
			//checks if moving Left
			if(_inputHor < 0) {
				
				facingLeft = true;
				facingRight = false;
				facingDown = false;
				facingUp = false;
	
			}
		}
		//not moving horizontally
		else 
		{
			movingHorizontally = false;
		}*/
	}

	void ApplyMovement () 
	{
		if (moving) 
		{
			if ( completedMove == true){
			AudioSource.PlayClipAtPoint(moveSound1, transform.position);
			}
			switch(direction)
			{
			case Direction.UP:
				transform.Translate (0.0f, movementSpeed * Time.deltaTime, 0.0f);
				break;
			case Direction.DOWN:
				transform.Translate (0.0f, -movementSpeed * Time.deltaTime, 0.0f);
				break;
			case Direction.LEFT:
				transform.Translate (-movementSpeed * Time.deltaTime, 0.0f, 0.0f);
				break;
			case Direction.RIGHT:
				transform.Translate (movementSpeed * Time.deltaTime, 0.0f, 0.0f);
				break;
			}
		}
		/*if (movingHorizontally == true) {
			transform.Translate (movementSpeed * _inputHor * Time.deltaTime, 0.0f, 0.0f);
		}
		if (movingVertically == true) {
			transform.Translate (0.0f, movementSpeed * _inputVert * Time.deltaTime, 0.0f);
		}*/

	}

	void ApplyAnimations () 
	{
		//if moving do these animations
		if (moving == true) 
		{
			switch(direction)
			{
			case Direction.UP:
				anim.SetTrigger ("moveUp");
				break;
			case Direction.DOWN:
				anim.SetTrigger ("moveDown");
				break;
			case Direction.LEFT:
				anim.SetTrigger ("moveLeft");
				break;
			case Direction.RIGHT:
				anim.SetTrigger ("moveRight");
				break;
			}
		} 
		//else do idle animations
		else 
		{
			switch(direction)
			{
			case Direction.UP:
				anim.SetTrigger ("moveUp");
				break;
			case Direction.DOWN:
				anim.SetTrigger ("moveDown");
				break;
			case Direction.LEFT:
				anim.SetTrigger ("moveLeft");
				break;
			case Direction.RIGHT:
				anim.SetTrigger ("moveRight");
				break;
			}
		}
	}

	public void SetStartDirection()
	{
		if (UP (inputControl.state)) 
		{
			direction = Direction.UP;
		}
		if (DOWN (inputControl.state)) 
		{
			direction = Direction.DOWN;
		}
		if (LEFT (inputControl.state)) 
		{
			direction = Direction.LEFT;
		}
		if (RIGHT (inputControl.state)) 
		{
			direction = Direction.RIGHT;
		}

	}

	bool UP(GamePadState _state)
	{
		if (inputControl.isDpad) 
		{
			if (_state.DPad.Up == ButtonState.Pressed) 
			{
				return true;
			}
		} 
		else 
		{
			if (_state.Buttons.Y == ButtonState.Pressed) 
			{
				return true;
			}
		}
		return false;
	}


	bool DOWN(GamePadState _state)
	{
		if (inputControl.isDpad) 
		{
			if (_state.DPad.Down == ButtonState.Pressed) 
			{
				return true;
			}
		} 
		else 
		{
			if (_state.Buttons.A == ButtonState.Pressed) 
			{
				return true;
			}
		}
		return false;
	}

	bool LEFT(GamePadState _state)
	{
		if (inputControl.isDpad) 
		{
			if (_state.DPad.Left == ButtonState.Pressed) 
			{
				return true;
			}
		} 
		else 
		{
			if (_state.Buttons.X == ButtonState.Pressed) 
			{
				return true;
			}
		}
		return false;
	}

	bool RIGHT(GamePadState _state)
	{
		if (inputControl.isDpad) 
		{
			if (_state.DPad.Right == ButtonState.Pressed) 
			{
				return true;
			}
		} 
		else 
		{
			if (_state.Buttons.B == ButtonState.Pressed) 
			{
				return true;
			}
		}
		return false;
	}

	public void SetInputControl(XInputControl _inputControl)
	{
		inputControl = _inputControl;
	}

	public void SetMovementIsPaused(bool _b)
	{
		isMovementPaused = _b;
	}

	void fullMove() {

		if (movingDistance == 0.0f) {
			completedMove = true;
		}
		if (movingDistance <= completeMoveDistance && moving == true) {

			completedMove = false;
			movingDistance += movementSpeed * Time.deltaTime;
		} 
		else if(movingDistance > completeMoveDistance) {
			movingDistance = 0;
			completedMove = false;
		}
		else if (moving == false) {
			movingDistance = 0;
			completedMove = false;
		} 
		else {
			movingDistance = 0;
			completedMove = false;
		} 
	}

}