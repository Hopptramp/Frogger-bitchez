using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JoinDisplay : MonoBehaviour 
{
	private int MAX_PLAYERS;
	private int MAX_CONTROLLERS;
	private Image[] readyDisplays;

	public Image readyPrefab;

	// Use this for initialization
	void Start () 
	{
		MAX_PLAYERS = GameObject.FindGameObjectWithTag("ConstantData").GetComponent<ConstantData>().MAX_PLAYERS; 
		MAX_CONTROLLERS = MAX_PLAYERS / 2;
		readyDisplays = new Image[MAX_PLAYERS];

		int displayWidth = (int)GetComponent<RectTransform> ().rect.width;
		int displayHeight = (int)GetComponent<RectTransform> ().rect.height;
		Vector3 scaleMod = GetComponent<RectTransform> ().localScale;

		//Multiplier to determine how far left or right along the screen to spawn the player
		float multiplier;
		if(MAX_PLAYERS < 2)
		{
			multiplier = 0;
		}
		else
		{
			multiplier = ((float)(displayWidth/2) - ((float)displayWidth/200)) / Mathf.Ceil((float)MAX_CONTROLLERS/4) * scaleMod.x;
		}


		for(int i = 0; i < MAX_CONTROLLERS; ++i)
		{
			float height = ((displayHeight * 5) / 8) * scaleMod.y;
			if(!(i < MAX_CONTROLLERS/2))
			{
				height = ((displayHeight * 3) / 8) * scaleMod.y;
			}

			float width = (displayWidth / 2) * scaleMod.x;
			if(!(i < MAX_CONTROLLERS/2))
			{
				width += (((Mathf.Ceil(-(float)MAX_CONTROLLERS/4)) + (i-(MAX_CONTROLLERS/2))) * multiplier);
			}
			else
			{
				width += (((Mathf.Ceil(-(float)MAX_CONTROLLERS/4)) + i) * multiplier);
			}

			float size = readyPrefab.rectTransform.rect.width / 2;

			Vector3 spawnPoint = new Vector3(width-size,height,0); 
			readyDisplays[i * 2] = Instantiate(readyPrefab, spawnPoint, Quaternion.identity) as Image;
			readyDisplays[i * 2].transform.parent = transform;

			spawnPoint = new Vector3(width+size, height,0); 
			readyDisplays[(i * 2)+1] = Instantiate(readyPrefab, spawnPoint, Quaternion.identity) as Image;
			readyDisplays[(i * 2)+1].transform.parent = transform;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void SetReadyDisplay(bool _on, int _player)
	{
		if (_on) 
		{
			readyDisplays[_player].color = Color.green;
		} 
		else 
		{
			readyDisplays[_player].color = Color.red;
		}
	}
}
