using UnityEngine;
using System.Collections;


[System.Serializable]
public struct statsStruct
    {
        public float sizeX;
        public float sizeY;
        public float speedX;
        public float speedY;
        public float delay;
    }

public class Spawner : MonoBehaviour 
{
    
    public statsStruct crocStats;
    public statsStruct logStats;

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
			platform.GetComponent<baseObject>().assignParameters(logStats);
			if(logStats.speedX < 0)
			{
				// invert the speed of the log
				logStats.speedX *= -1;
			}
			//set the delay
			delay = logStats.delay;
			// sets platform's parent to the parent of the spawner
			platform.transform.parent = transform.parent;

			break;

		case 2: // platform left
			GameObject platformLeft;
			//instantiate the object from a prefab
			platformLeft = Instantiate(objectToSpawn[spawnWhat - 1], gameObject.transform.position, Quaternion.identity) as GameObject;
			// find the settings from a struct
			
			if(logStats.speedX > 0)
			{
				// invert the speed of the log
				logStats.speedX *= -1;
			}
			//apply settings
			platformLeft.GetComponent<baseObject>().assignParameters(logStats);
			//reset the delay
			delay = logStats.delay;

			// sets platform's parent to the parent of the spawner
			platformLeft.transform.parent = transform.parent;
			break;

		case 3: // croc right
			GameObject Croc;
			//instantiate croc
			Croc = Instantiate(objectToSpawn[spawnWhat - 1], gameObject.transform.position, Quaternion.identity) as GameObject;
			// find it's settings
			
			// assign it's settings
			Croc.GetComponent<baseObject>().assignParameters(crocStats);
			if(crocStats.speedX < 0)
			{
				crocStats.speedX *= -1;
				crocStats.sizeX *= -1;
			}
			//reset the delay
			delay = crocStats.delay;


			// sets platform's parent to the parent of the spawner
			Croc.transform.parent = transform.parent;
			break;

		case 4:
			GameObject CrocLeft;
			//instantiate
			CrocLeft = Instantiate(objectToSpawn[spawnWhat - 1], gameObject.transform.position, Quaternion.identity) as GameObject;
			//find settings
			
			//invert the speed
			if(crocStats.speedX > 0)
			{
				crocStats.speedX *= -1;
				crocStats.sizeX *= -1;
			}
			//assign
			CrocLeft.GetComponent<baseObject>().assignParameters(crocStats);
			//reset the delay
			delay = crocStats.delay;


			// sets platform's parent to the parent of the spawner
			CrocLeft.transform.parent = transform.parent;
			break;
		}
	
	}
}
