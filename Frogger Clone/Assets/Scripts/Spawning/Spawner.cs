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
		//spawnObject ();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		whenToSpawn ();
	}

	void whenToSpawn()
	{
		if (!delaySet) 
		{
			delay -= Time.deltaTime;
		
			if (delay <= 0) {
				spawnObject ();
				delaySet = false;
			}
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
			logStructs settings = GetComponent<logStructs>();
			platform.GetComponent<baseObject>().assignParametersLog(settings);
			if(settings.speedX < 0)
			{
				// invert the speed of the log
				settings.speedX *= -1;
			}
			//set the delay
			delay = settings.delay;
			// sets platform's parent to the parent of the spawner
			platform.transform.parent = transform.parent;

			break;

		case 2: // platform left
			GameObject platformLeft;
			//instantiate the object from a prefab
			platformLeft = Instantiate(objectToSpawn[spawnWhat - 1], gameObject.transform.position, Quaternion.identity) as GameObject;
			// find the settings from a struct
			logStructs settingsLeft = GetComponent<logStructs>();
			if(settingsLeft.speedX > 0)
			{
				// invert the speed of the log
				settingsLeft.speedX *= -1;
			}
			//apply settings
			platformLeft.GetComponent<baseObject>().assignParametersLog(settingsLeft);
			//reset the delay
			delay = settingsLeft.delay;

			// sets platform's parent to the parent of the spawner
			platformLeft.transform.parent = transform.parent;
			break;

		case 3: // croc right
			GameObject Croc;
			//instantiate croc
			Croc = Instantiate(objectToSpawn[spawnWhat - 1], gameObject.transform.position, Quaternion.identity) as GameObject;
			// find it's settings
			CrocStruct settingsCroc = GetComponent<CrocStruct>();
			// assign it's settings
			Croc.GetComponent<baseObject>().assignParametersCroc(settingsCroc);
			if(settingsCroc.speedX < 0)
			{
				settingsCroc.speedX *= -1;
				settingsCroc.sizeX *= -1;
			}
			//reset the delay
			delay = settingsCroc.delay;


			// sets platform's parent to the parent of the spawner
			Croc.transform.parent = transform.parent;
			break;

		case 4:
			GameObject CrocLeft;
			//instantiate
			CrocLeft = Instantiate(objectToSpawn[spawnWhat - 1], gameObject.transform.position, Quaternion.identity) as GameObject;
			//find settings
			CrocStruct settingsCrocLeft = GetComponent<CrocStruct>();
			//invert the speed
			if(settingsCrocLeft.speedX > 0)
			{
				settingsCrocLeft.speedX *= -1;
				settingsCrocLeft.sizeX *= -1;
			}
			//assign
			CrocLeft.GetComponent<baseObject>().assignParametersCroc(settingsCrocLeft);
			//reset the delay
			delay = settingsCrocLeft.delay;


			// sets platform's parent to the parent of the spawner
			CrocLeft.transform.parent = transform.parent;
			break;
		}
	
	}
}
