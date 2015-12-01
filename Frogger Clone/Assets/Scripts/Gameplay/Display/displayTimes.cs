using UnityEngine;
using System.Collections;

public class displayTimes : MonoBehaviour 
{
	private int numOfPlayers;
	public GameObject[] players;
	public GameObject playerTitle;

	private float canvasWidth;
	private float canvasHeight;

	private float spacingX = 0.1f;
	private float spacingY;

	private Vector3 centrePoint;

	// Use this for initialization
	void Start ()
	{
		// initialise the player array
		//numOfPlayers = GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().numOfPlayers;
		numOfPlayers = 20;
		players = new GameObject[numOfPlayers];
		//fill the player array from constant data
		//players = GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().players;
		//get canvas height and width
		canvasWidth = GameObject.Find ("Canvas").GetComponent<RectTransform> ().rect.width;
		canvasHeight = GameObject.Find ("Canvas").GetComponent<RectTransform> ().rect.height;
		//find canvas centre point
		centrePoint = new Vector3 (canvasWidth / 2, canvasHeight / 2, 0);

		// XAV GO NUTS

		float totalWidth = canvasWidth / (numOfPlayers / 2);
		float playerBanner = totalWidth - spacingX;
		float centrePositionX = (playerBanner / 2) + (spacingX / 2);

		int height1 = 20;
		int height2 = 20;

		Vector3 bannerPos = new Vector3(0.0f, 0.0f, 0.0f);
		//Vector3 heightPos = new Vector3(0.0f, 0.0f, 0.0f);

		// Before here
		for (int i = 0; i < numOfPlayers; ++i)
		{

			if(i < (numOfPlayers / 2))
			{
				bannerPos.x += centrePositionX;
				bannerPos.y = height1;
				Instantiate(playerTitle, bannerPos , Quaternion.identity);
			}
			else if (i > (numOfPlayers / 2))
			{
				bannerPos.x += centrePositionX;
				bannerPos.y = height2;
				Instantiate(playerTitle, bannerPos , Quaternion.identity);
			}
		}
	}


	// Update is called once per frame
	void Update () 
	{
	
	}
}
