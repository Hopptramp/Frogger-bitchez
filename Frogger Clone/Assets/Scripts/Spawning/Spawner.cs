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

    public float mapScale;
    public bool moveLeft;
    public bool onWater;

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
        if(onWater)
        {
            if(Random.value <=0.5f)
            {
                spawnWhat = 1;
            }
            else
            {
                spawnWhat = 2;
            }
        }
        else
        {
            spawnWhat = 1;
        }

		switch (spawnWhat) 
		{
		case 1: // platform right
			GameObject platform;
			//instantiate the object from a prefab
			platform = Instantiate(objectToSpawn[spawnWhat - 1], gameObject.transform.position, Quaternion.identity) as GameObject;

            if((logStats.speedX < 0 && !moveLeft) || (logStats.speedX > 0 && moveLeft))
            {    
				// invert the speed of the log
				logStats.speedX *= -1;
                    
			}            

			// assign the parameters by passing through a struct from the prefab
			platform.GetComponent<baseObject>().assignParameters(logStats,mapScale);
			

			//set the delay
			delay = logStats.delay;
			// sets platform's parent to the parent of the spawner
			platform.transform.parent = transform.parent;

			break;

		case 2: // croc right
			GameObject Croc;
			//instantiate croc
			Croc = Instantiate(objectToSpawn[spawnWhat - 1], gameObject.transform.position, Quaternion.identity) as GameObject;
                // find it's settings

            if ((crocStats.speedX < 0 && !moveLeft) || (crocStats.speedX > 0 && moveLeft))
            {
                    crocStats.speedX *= -1;
                    
            }
            if (crocStats.sizeX >0 && moveLeft)
                {
                    crocStats.sizeX *= -1;
                }

            // assign it's settings
            Croc.GetComponent<baseObject>().assignParameters(crocStats,mapScale);
			
			//reset the delay
			delay = crocStats.delay;


			// sets platform's parent to the parent of the spawner
			Croc.transform.parent = transform.parent;
			break;
                
		}
	
	}
}
