using UnityEngine;
using System.Collections;

public class playerCollision : MonoBehaviour 
{
	private LevelTileManager tileMan;

	void Start()
	{
		tileMan = GameObject.Find ("Managers").GetComponent<LevelTileManager> ();
	}

	// Update is called once per frame
	void Update () 
	{
		// if the player has a parent
		if (gameObject.transform.parent) 
		{
			// if the player is not on water
			if (!tileMan.ObjectOnTile(gameObject, LevelTileManager.TileTypes.WATER)) 
			{
				// remove the parent
				removeParent ();
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D coll)
	{
		//Need to also test that is a carrying object and not an impact object
		// if the player is on water
		if (tileMan.ObjectOnTile(gameObject, LevelTileManager.TileTypes.WATER))
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
		//Need to also test that is a carrying object and not an impact object
		// if the player is on water
		if (tileMan.ObjectOnTile(gameObject, LevelTileManager.TileTypes.WATER)) 
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
