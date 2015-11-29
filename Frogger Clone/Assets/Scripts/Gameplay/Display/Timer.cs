using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour 
{
	private Text timer;
	[Range(0, 300)] public int startSeconds = 120;
	private float time;
	// Use this for initialization
	void Start () 
	{
		timer = GameObject.Find ("VisualDisplay").transform.Find ("Timer").GetComponent<Text> ();
		time = (float)startSeconds;
		timer.text = TimeToDisplay ();
	}

	
	// Update is called once per frame
	void Update () 
	{
		time -= Time.deltaTime;
		timer.text = TimeToDisplay ();
		if (!(time > 0)) 
		{
			ControllerManager controllerMan = GameObject.Find("ControllerManager").GetComponent<ControllerManager>();
			if(controllerMan.AtLeastOnePlayer())
			{
				controllerMan.NextLevel();
				time = 0.0f;
				timer.text = TimeToDisplay ();
			}
			else
			{
				time = (float)startSeconds;
				timer.text = TimeToDisplay ();
			}
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
