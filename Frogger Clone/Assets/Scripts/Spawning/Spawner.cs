using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
	// input parameters to define what spawns
	public GameObject[] objectToSpawn;
	public int spawnWhat; // the prefab in the array must match the case number in the switch statement (with a minus 1)
	// 1 = log going right, 2 = log going left, 3 = nothing
	

	float delay = 0;
	bool delaySet = false;

	// Use this for initialization
	void Start () 
	{
		
	}

	void Awake()
	{
		spawnObject ();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		whenToSpawn ();
	}

	void whenToSpawn()
	{
		if (!delaySet) {
			if (spawnWhat == 1) { // log
				delay = 3;
			}
			if (spawnWhat == 2) { // other thing
				delay = 3;
			}
			if (spawnWhat == 3) { // other other thing
				delay = 3;
			}
			delaySet = true;
		}

		delay -= Time.deltaTime;
		
		if (delay <= 0)
		{
			spawnObject();
			delaySet = false;
		}
	}

	void spawnObject()
	{
		switch (spawnWhat) 
		{
		case 1: // platform right
			GameObject platform;
			//instantiate the object from a prefab
			platform = Instantiate(objectToSpawn[spawnWhat - 1], gameObject.transform.position, Quaternion.identity) as GameObject;
			// assign the parameters by passing through a struct from the prefab
			logStructs settings = platform.GetComponent<logStructs>();
			platform.GetComponent<baseObject>().assignParameters(settings);
			break;
		case 2: // platform left
			GameObject platformLeft;
			//instantiate the object from a prefab
			platformLeft = Instantiate(objectToSpawn[spawnWhat - 1], gameObject.transform.position, Quaternion.identity) as GameObject;
			// assign the parameters by passing through a struct from the prefab
			logStructs settingsLeft = platformLeft.GetComponent<logStructs>();
			settingsLeft.speedX = -settingsLeft.speedX;
			platformLeft.GetComponent<baseObject>().assignParameters(settingsLeft);
			break;
		case 3: // something else?

			break;
		}
	
	}
}
