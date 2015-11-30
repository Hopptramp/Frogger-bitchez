using UnityEngine;
using System.Collections;

public class displayTimes : MonoBehaviour 
{
	private int numOfPlayers;
	public GameObject[] players;
	public GameObject playerTitle;

	private float canvasWidth;
	private float canvasHeight;
	private Vector3 centrePoint;
	private float spacingX = 0.1f;
	private float spacingY;

	// Use this for initialization
	void Start ()
	{
		// initialise the player array
		numOfPlayers = GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().numOfPlayers;
		players = new GameObject[numOfPlayers];
		//fill the player array from constant data
		players = GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().players;
		//get canvas height and width
		canvasWidth = GameObject.Find ("Canvas").GetComponent<RectTransform> ().rect.width;
		canvasHeight = GameObject.Find ("Canvas").GetComponent<RectTransform> ().rect.height;
		//find canvas centre point
		centrePoint = new Vector3 (canvasWidth / 2, canvasHeight / 2, 0);

		// XAV GO NUTS

		float totalWidth = canvasWidth / (numOfPlayers / 2);
		float playerBanner = totalWidth - spacingX;
		float centrePositionX = (playerBanner / 2) + (spacingX / 2);

		Vector3 count = new Vector3(0.0f, 0.0f, 0.0f);

		// Before here
		for (int i = 0; i < numOfPlayers; ++i)
		{
			count.x += centrePositionX;
			Instantiate(playerTitle, count , Quaternion.identity);
		}
	}


	// Update is called once per frame
	void Update () 
	{
	
	}
}
