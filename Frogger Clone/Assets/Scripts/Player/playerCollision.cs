using UnityEngine;
using System.Collections;

public class playerCollision : MonoBehaviour 
{

	// Update is called once per frame
	void Update () 
	{
		// if the player has a parent
		if (gameObject.transform.parent) 
		{
			// if the player is not on water
			if (!GameObject.Find ("Managers").GetComponent<LevelTileManager> ().playerOnWater (gameObject)) 
			{
				// remove the parent
				removeParent ();
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D coll)
	{
		// if the player is on water
		if (GameObject.Find ("Managers").GetComponent<LevelTileManager> ().playerOnWater (gameObject))
		{
			// parenting the player to the logs
			if (coll.gameObject.tag == "Platform") 
			{
				gameObject.transform.SetParent (coll.gameObject.transform);
			}
		}
	}

	// if the player steps off a log
	void OnTriggerExit2D()
	{
		// if the player is on water
		if (GameObject.Find ("Managers").GetComponent<LevelTileManager> ().playerOnWater (gameObject)) 
		{
			// send on the message the player is dead
			//GetComponent<PlayerMain>().OnDeath(); 

		}
	}

	
	//
	void removeParent()
	{
		// removing parent
		gameObject.transform.parent = null;
	}
}
