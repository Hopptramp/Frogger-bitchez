using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
	// input parameters to define what spawns
	public GameObject[] objectToSpawn;
	public int spawnWhat; // the prefab in the array must match the case number in the switch statement (with a minus 1)
	

	// Use this for initialization
	void Start () 
	{
		
	}

	void Awake()
	{
		spawnObject ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void spawnObject()
	{
		switch (spawnWhat) 
		{
		case 1: // platform
			GameObject platform;
			//instantiate the object from a prefab
			platform = Instantiate(objectToSpawn[spawnWhat - 1], gameObject.transform.position, Quaternion.identity) as GameObject;
			// assign the parameters by passing through a struct from the prefab
			logStructs settings = objectToSpawn[spawnWhat - 1].GetComponent<logStructs>();
			platform.GetComponent<baseObject>().assignParameters(settings);
			break;
		case 2: // enemy

			break;
		case 3: // something else?

			break;
		}
	
	}
}
