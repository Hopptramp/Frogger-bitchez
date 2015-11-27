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
		if (gameObject.transform.parent) 
		{
			if (!GameObject.Find ("Managers").GetComponent<LevelTileManager> ().playerOnWater (gameObject)) 
			{
				removeParent ();
			}
		}
	}
	
	void OnTriggerStay2D(Collider2D coll)
	{
		if (GameObject.Find ("Managers").GetComponent<LevelTileManager> ().playerOnWater (gameObject))
		{
			// parenting the player to the logs
			if (coll.gameObject.tag == "Platform") 
			{
				gameObject.transform.SetParent (coll.gameObject.transform);
			}
		}
	}
	
	//
	public void removeParent()
	{
		// removing parent
		gameObject.transform.parent = null;
	}
}
