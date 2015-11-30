using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayScript : MonoBehaviour 
{
	private Text startTimerText;
	private float startTime;

	private Text gameTimerText;
	public int gameTimeSeconds = 120;
	[HideInInspector] public float time = 0;
	private bool gameBegun = false;

	// Use this for initialization
	void Start () 
	{
		startTimerText = transform.Find ("StartTimer").GetComponent<Text> ();
		gameTimerText = transform.Find ("GameTimer").GetComponent<Text> ();
		time = (float)gameTimeSeconds;
		gameTimerText.text = TimeToDisplay ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(gameBegun)
		{
			time -= Time.deltaTime;
			gameTimerText.text = TimeToDisplay ();
			if (!(time > 0)) 
			{
				time = 0.0f;
				gameTimerText.text = TimeToDisplay ();
			}
		}
	}

	public void GameBegun()
	{
		gameBegun = true;
	}

	public void SetStartTime(float _startTime)
	{
		startTime = _startTime;
	}

	public void UpdateTimer(float _time)
	{
		int currentTime = (int)Mathf.Ceil(startTime - _time);

		if (currentTime > 0) 
		{
			startTimerText.text = currentTime.ToString ();
		} 
		else 
		{
			startTimerText.text = "";
			// update the array of players
			GameObject.Find("Managers").GetComponent<PlayerManager>().updateConstantData();
			//move to next scene from here
		}
	}

	string TimeToDisplay()
	{
		int minutes = (int)Mathf.Floor(time / 60);
		string display = "0" + minutes.ToString() + ":";
		int seconds = (int)(time % 60);
		if (seconds > 9) 
		{
			display += seconds.ToString();
		} 
		else 
		{
			display += "0" + seconds.ToString();
		}
		return display;
	}
}
