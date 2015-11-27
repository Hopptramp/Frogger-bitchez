using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayScript : MonoBehaviour 
{
	private Text startTimerText;
	private float startTime;

	// Use this for initialization
	void Start () 
	{
		startTimerText = transform.Find ("StartTimer").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
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
		}
	}
}
