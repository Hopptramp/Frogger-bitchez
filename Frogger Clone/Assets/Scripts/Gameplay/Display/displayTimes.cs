using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class displayTimes : MonoBehaviour 
{
	private int numOfPlayers;
	public GameObject[] players;
	public GameObject playerTitle;
	private GameObject _playerTitleTextComponent;
	private GameObject[] texts;

	private float canvasWidth;
	private float canvasHeight;
	private float titleWidth;

	private float spacingX = 0.1f;
	private float spacingY;

	private Vector3 centrePoint;
	bool firstTime = true;

	// Use this for initialization
	void Start ()
	{
		// initialise the player array
		//numOfPlayers = GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().numOfPlayers;
		numOfPlayers = 20;
		players = new GameObject[numOfPlayers];
		//fill the player array from constant data
		players = GameObject.FindGameObjectWithTag ("ConstantData").GetComponent<ConstantData> ().players;
		texts = new GameObject[numOfPlayers];
		//get canvas height and width
		canvasWidth = GameObject.Find ("Canvas").GetComponent<RectTransform> ().rect.width;
		canvasHeight = GameObject.Find ("Canvas").GetComponent<RectTransform> ().rect.height;
		//find canvas centre point
		centrePoint = new Vector3 (canvasWidth / 2, canvasHeight / 2, 0);

		//banner height and width
		titleWidth = 100;

		// starting heights
		float height1 = 200;
		float height2 = -200;

		// starting X pos for each banner
		float startPoint = centrePoint.x - ((numOfPlayers / 2) * titleWidth);

		// initialise the banner pos variable
		Vector3 bannerPos = new Vector3(0.0f, 0.0f, 0.0f);

		// iterate through the players and instance new banners
		for (int i = 0; i < numOfPlayers; ++i)
		{
			// set the x start point of the banner
			bannerPos.x = startPoint;
			if(i < (numOfPlayers / 2))
			{
				// set the y start point of the banner
				bannerPos.y = height1;
			}
			else if (i >= (numOfPlayers / 2))
			{
				if (firstTime)
				{
					startPoint = centrePoint.x - ((numOfPlayers / 2) * titleWidth);
					firstTime = false;
					bannerPos.x = startPoint;
				}
				bannerPos.y = height2;
			}

			//instantiate the object and parent to the canvas
			_playerTitleTextComponent =  Instantiate(playerTitle, bannerPos , Quaternion.identity) as GameObject;
			_playerTitleTextComponent.transform.SetParent(GameObject.Find("Canvas").transform);

			// increment the start point for the next banner
			startPoint += titleWidth + spacingX;

			texts[i] = _playerTitleTextComponent;
			changeTextComponents(i);
		}

		//for (int i = 0; i < players.Length; ++i)
		//{
	//		changeTextComponents(i);
	//	}
	}

	void changeTextComponents(int i)
	{
		//initialise variables
		int score = 0;
		int timeTaken = 0;
		string result = " ";
		string playerID = " ";
		int timesDied = GameObject.FindGameObjectWithTag("ConstantData").GetComponent<ConstantData>().timesDied[i];

		// if the player existed
		if (GameObject.FindGameObjectWithTag("ConstantData").GetComponent<ConstantData>().scores[i] != 0)
		{
			score = GameObject.FindGameObjectWithTag("ConstantData").GetComponent<ConstantData>().scores[i];
		}
		// if the score is 0 they were not playing
		if (score != 0)
		{
			if (!GameObject.FindGameObjectWithTag("ConstantData").GetComponent<ConstantData>().instantRespawn)
			{
				timeTaken = (int)GameObject.FindGameObjectWithTag("ConstantData").GetComponent<ConstantData>().timeTaken[i];
				if (timeTaken == 0)
				{
					result = "FAILED";
				}
				else if (score < 3)
				{
					if (score == 1)
					{
						result = score + "st \n" + timeTaken + " seconds";
					}
					if (score == 2)
					{
						result = score + "nd \n" + timeTaken + " seconds";
					}
					if (score == 3)
					{
						result = score + "rd \n" + timeTaken + " seconds";
					}
				}
				else 
				{
					result = score + "th /n" + timeTaken + "seconds";
				}
			}
			else
			{
				int prescore = (GameObject.FindGameObjectWithTag("ConstantData").GetComponent<ConstantData>().players.Length - score) + 1;
				if (score < 3)
				{
					if (prescore == 1)
					{
						result = prescore + "st \n died" + timesDied + " times";
					}
					if (prescore == 2)
					{
						result = prescore + "nd \n died" + timesDied + " times";
					}
					if (prescore == 3)
					{
						result = prescore + "rd \n died" + timesDied + " times";
					}
				}
				else
				{
					result = prescore + "th \n times" + timesDied + " times";
				}
				
			}
			// find the player number
			playerID = "Player" + (i + 1);

		}
		else 
		{
			texts[i].SetActive(false);
		}

		//find the playerName text component
		Transform textPlayerName = texts[i].transform.Find("Panel/Name");
		Text playerName = textPlayerName.GetComponent<Text>();
		// change the text
		playerName.text = playerID;

		// find the player result text component
		Transform textTimeTaken = texts[i].transform.Find ("Panel/Result");
		Text playerTime = textTimeTaken.GetComponent<Text> ();
		playerTime.text = result;

	}



	// Update is called once per frame
	void Update () 
	{
	
	}
}
