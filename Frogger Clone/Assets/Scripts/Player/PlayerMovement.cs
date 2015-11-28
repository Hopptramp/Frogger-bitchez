//#define XINPUT_CONTROL

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if XINPUT_CONTROL
using XInputDotNetPure;
#endif

public class PlayerMovement : MonoBehaviour 
{
	private Animator anim;
	private InputControl inputControl;
	public GameObject explosion;
	public float explosionLifetime = 1.0f;


	public float movementSpeed = 5.0f;
	public float completeMoveDistance = 3.09f;
	public float movingDistance = 0.0f;

	public bool completedMove = false;
	public bool moving = false;
	private bool isMovementPaused = false;
	public bool isAlive = true;

	private const float DPAD_GROUND = 0.95f;
	
	public AudioClip moveSound1;
	public AudioClip gameOverSound;

	enum Direction
	{
		UP = 0,
		DOWN,
		LEFT,
		RIGHT,
		MAX_DIRECTIONS
	}

	private Direction direction;
	private bool applyDirectionAgain = false;

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
#if XINPUT_CONTROL
		//Update the controller states
		inputControl.prevState = inputControl.state;
		inputControl.state = GamePad.GetState(inputControl.playerIndex);
#endif
		
		if(!isMovementPaused)
		{
			ApplyDirection ();
			ApplyMovement ();
			audioLoop();
			ApplyAnimations ();
		}
	}

#if XINPUT_CONTROL
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
	}
#endif
#if !XINPUT_CONTROL
	void ApplyDirection ()
	{
		applyDirectionAgain = false;
		//set moving to false and if input is acquired set to true
		moving = false;

		//If up is pressed
		bool up = UP ();
		CheckDirection (up, Direction.UP);
	
		//If down is pressed
		bool down = DOWN ();
		CheckDirection (down, Direction.DOWN);

		//If left is pressed
		bool left = LEFT ();
		CheckDirection (left, Direction.LEFT);

		//If right is pressed
		bool right = RIGHT();
		CheckDirection (right, Direction.RIGHT);	

		if (applyDirectionAgain) 
		{
			ApplyDirection ();
		}
	}

	void CheckDirection(bool _pressed, Direction _direction)
	{
		//If button is getting pressed
		if (_pressed) 
		{
			//and was not getting pressed last update
			if(!inputControl.prevState[(int)_direction])
			{
				//and not already heading in that direction
				if(direction != _direction)
				{
					direction = _direction;
				}
			}
			moving = true;
		}
		//If button is not getting pressed
		else 
		{
			//But still heading in that direction
			if (direction == _direction) 
			{
				//Set to general direction
				direction = Direction.MAX_DIRECTIONS;
				//Reset the previous states
				for(int i = 0; i < (int)Direction.MAX_DIRECTIONS; ++i)
				{
					inputControl.prevState[i] = false;
				}
				//Test to find new direction
				applyDirectionAgain = true;
			}
		}
		
		inputControl.prevState [(int)_direction] = _pressed;
	}
#endif

	void ApplyMovement () 
	{
		if (moving) 
		{
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
				anim.SetTrigger ("idle");
				break;
			case Direction.DOWN:
				anim.SetTrigger ("idle");
				break;
			case Direction.LEFT:
				anim.SetTrigger ("idle");
				break;
			case Direction.RIGHT:
				anim.SetTrigger ("idle");
				break;
			}
		}
	}



#if XINPUT_CONTROL
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
#endif
#if !XINPUT_CONTROL
	bool UP()
	{
		if (inputControl.isDpad) 
		{
			if (DPAD_GROUND < Input.GetAxisRaw	("dpady"+inputControl.controllerIndex)) 
			{
				return true;
			}
		} 
		else 
		{
			if (Input.GetButton("Y"+inputControl.controllerIndex)) 
			{
				return true;
			}
		}
		return false;
	}
	
	
	bool DOWN()
	{
		if (inputControl.isDpad) 
		{
			if (-DPAD_GROUND > Input.GetAxisRaw("dpady"+inputControl.controllerIndex)) 
			{
				return true;
			}
		} 
		else 
		{
			if (Input.GetButton("A"+inputControl.controllerIndex)) 
			{
				return true;
			}
		}
		return false;
	}
	
	bool LEFT()
	{
		if (inputControl.isDpad) 
		{
			if (-DPAD_GROUND > Input.GetAxisRaw("dpadx"+inputControl.controllerIndex)) 
			{
				return true;
			}
		} 
		else 
		{
			if (Input.GetButton("X"+inputControl.controllerIndex)) 
			{
				return true;
			}
		}
		return false;
	}
	
	bool RIGHT()
	{
		if (inputControl.isDpad) 
		{
			if (DPAD_GROUND < Input.GetAxisRaw("dpadx"+inputControl.controllerIndex)) 
			{
				return true;
			}
		} 
		else 
		{
			if (Input.GetButton("B"+inputControl.controllerIndex)) 
			{
				return true;
			}
		}
		return false;
	}

	public void SetStartDirection()
	{
		if (UP ()) 
		{
			direction = Direction.UP;
		}
		if (DOWN ())
		{
			direction = Direction.DOWN;
		}
		if (LEFT ())
		{
			direction = Direction.LEFT;
		}
		if (RIGHT ())
		{
			direction = Direction.RIGHT;
		}
	}
#endif

	public void SetInputControl(InputControl _inputControl)
	{
		inputControl = _inputControl;
	}

	public void SetMovementIsPaused(bool _b)
	{
		isMovementPaused = _b;
	}
	public void OnDeath()
	{
		Destroy (Instantiate(explosion, gameObject.transform.position , Quaternion.identity), explosionLifetime);
		//isAlive = false;
		SoundManager.instance.RandomizeSfx(gameOverSound);
		//GameObject sprite = transform.Find ("Sprite").gameObject; 
		//sprite.SetActive (false);
		gameObject.SetActive(false);
	}
	void audioLoop()
	{
		//if (moving == true) {
		if (isAlive == false) {
		}
		else if (movingDistance >= completeMoveDistance -0.01) 
		{
			//AudioSource.PlayClipAtPoint (moveSound1, transform.position);
			SoundManager.instance.RandomizeSfx(moveSound1);
		}
		if (movingDistance > completeMoveDistance) 
		{
			movingDistance = 0.0f;
		}
		if (moving == true) 
		{
			movingDistance += movementSpeed * Time.deltaTime;
		}
		if (moving == false) 
		{
			movingDistance = completeMoveDistance -0.02f;
		}
		//}
	}
}