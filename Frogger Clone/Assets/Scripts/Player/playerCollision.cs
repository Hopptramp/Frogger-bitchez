using UnityEngine;
using System.Collections;

public class playerCollision : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		// parenting the player to the logs
		if (coll.gameObject.tag == "Platform") 
		{
			gameObject.transform.SetParent (coll.gameObject.transform);
		} 
	}

	//
	void OnTriggerExit2D()
	{
		// removing parent
		gameObject.transform.parent = null;
	}
}
