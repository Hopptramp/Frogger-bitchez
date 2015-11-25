using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public GameObject frog;
	private Animator anim;

	bool moving = false;
	public bool movingHorizontally = false;
	public bool movingVertically = false;
	bool facingLeft = false;
	bool facingRight = false;
	bool facingUp = false;
	bool facingDown = false;

	int stateIdle = 0;
	int stateUp = 1;
	int stateDown = 2;
	int stateLeft = 3;
	int stateRight = 4;

	public int currentState;
	int oldState;

	public float movementSpeed = 1.0f;

	float inputHor;
	float inputVert;

	void awake () {
	}
	// Use this for initialization
	void Start () {


	}
	void FixedUpdate () {

		inputHor = Input.GetAxis ("Horizontal");
		inputVert = Input.GetAxis ("Vertical");
		anim = GetComponent<Animator>();

		Direction (inputHor, inputVert);
		Movement ( inputHor, inputVert);
		Animations (inputHor, inputVert, anim);

	}
	// Update is called once per frame
	void Update () {
	}
	void Movement (float _inputHor, float _inputVert) {

		//if (movingHorizontally == true)
		if ((movingHorizontally == true) && (movingVertically == false)) 
		{
			transform.Translate (movementSpeed * _inputHor * Time.deltaTime, 0.0f, 0.0f);
		}
		//if (movingVertically == true)
		if ((movingHorizontally == false) && (movingVertically == true)) 
		{
			transform.Translate (0.0f, movementSpeed * _inputVert * Time.deltaTime, 0.0f);
		}

	}

	void Animations ( float _inputHor, float _inputVert, Animator _anim) {

		//if moving do these animations
		if (moving == true) {

			if (facingRight == true) { 

				_anim.SetTrigger ("moveRight");
			}
			if (facingLeft == true) { 
			
				_anim.SetTrigger ("moveLeft");
			
			}
			if (facingUp == true) {

				_anim.SetTrigger ("moveUp");
			}
			if (facingDown == true) {
			
				_anim.SetTrigger ("moveDown");
			}
		} 
		//else do idle animations
		else {

			if (facingRight == true) { 
				
				_anim.SetTrigger ("moveRight");
			}
			if (facingLeft == true) { 
				
				_anim.SetTrigger ("moveLeft");
				
			}
			if (facingUp == true) {
				
				_anim.SetTrigger ("moveUp");
			}
			if (facingDown == true) {
				
				_anim.SetTrigger ("moveDown");
			}
		}
	}

	void Direction (float _inputHor, float _inputVert)
	{
		//checks if moving in any direction
		if ((_inputHor != 0) || (_inputVert != 0)) {
			moving = true;
		} 
		else {
			moving = false;
		}
		
		//checks if moving vertically
		if (_inputVert != 0) {
			movingVertically = true;
			//movingHorizontally = false;
			
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
			//movingVertically = false;
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
		else {
			movingHorizontally = false;
		}
	}
}