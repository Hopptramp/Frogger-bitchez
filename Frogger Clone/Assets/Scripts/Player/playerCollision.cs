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
		Debug.Log (tileMan.TileAtPosition ((int)(transform.position.y+((75/2)+1))).ToString ());
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
	
	void OnTriggerEnter2D(Collider2D col)
	{
		// parenting the player to the logs
		if (col.gameObject.tag == "Platform") 
		{
			if (tileMan.ObjectOnTile (gameObject, LevelTileManager.TileTypes.WATER)) 
			{
				gameObject.transform.SetParent (col.gameObject.transform);
			}
		}
	}

	//Needed to get on the initial platform as you can collide with log before being officially on a water tile then trigger enter isn't called
	void OnTriggerStay2D(Collider2D col)
	{
		//If a platform
		if (col.gameObject.tag == "Platform") 
		{
			//And not already parented to something
			if (!gameObject.transform.parent) 
			{
				//And on a water tile
				if (tileMan.ObjectOnTile (gameObject, LevelTileManager.TileTypes.WATER)) 
				{
					//Parent to object
					gameObject.transform.SetParent (col.gameObject.transform);
				}
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
