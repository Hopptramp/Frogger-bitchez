using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
	private ObjectManager objMan;

	private float spawnRate = 1.0f; //Need validate based on size of object and speed of object for spawnRate
	private float timeOnLastSpawn = 0.0f;

    public float mapScale;
    //public bool moveLeft;
    //public bool onWater;

	// input parameters to define what spawns
	//public GameObject[] objectToSpawn;

	//Directions to spawn object
	public enum SpawnDirection
	{
		LEFT = 0,
		RIGHT,
		NONE,
		ALL_TYPES
	}

	private SpawnDirection direction;

	private int numObjects;   //Maximum number of types of objects that can be spawned
	private StatsStruct[] objects;   //All the types of objects that this spawner can spawn
	private int currentObjects = 0;  //The amount of objects that have been added to currentObjects so far
	//The sum of the chance of all objects spawning

	private int[] weightedSpawnChanceSum;   //The additive chances that each object can be spawned over another
	private int totalWeightedSpawnChance = 0;   //The sum of all the weighted chances

	//public int spawnWhat; // the prefab in the array must match the case number in the switch statement (with a minus 1)
	// 1 = log going right, 2 = log going left, 3 = nothing

	void Awake()
	{
		objMan = GameObject.Find ("Managers").GetComponent<ObjectManager> ();
	}

	// Use this for initialization
	void Start () 
	{
		timeOnLastSpawn = Time.realtimeSinceStartup;
	}

	//Setup initial values of spawner
	public void SetupSpawnerBasics(int _numObjects, float _mapScale, SpawnDirection _direction)
	{
		numObjects = _numObjects;
		mapScale = _mapScale;
		direction = _direction;
		objects = new StatsStruct[numObjects];
		weightedSpawnChanceSum = new int[numObjects];
	}

	//Adds a spawnable object type to list of objects that can be spawned
	public void SetupSpawnableObject(ObjectType _object)
	{
		//Get prefab from object manager
		objects[currentObjects] = objMan.GetObjectData(_object);

		//Adds random variation to speed and spawn rate
		objects[currentObjects].speedX *= Random.Range(0.5f, 2.0f);
		
		if (objects [currentObjects].sizeX / 2 > spawnRate) 
		{
			spawnRate = (objects [currentObjects].sizeX / 2) * Random.Range(0.75f, 1.25f);
		}

		//Begin setting up the weighted chances as some spawners can spawn multiple different things
		totalWeightedSpawnChance += objects[currentObjects].weightedSpawnChance;
		weightedSpawnChanceSum[currentObjects] = totalWeightedSpawnChance;

		++currentObjects;
	}
	
	// Update is called once per frame
	void Update () 
	{
		TryToSpawn ();
	}

	void TryToSpawn()
	{
		if (Time.realtimeSinceStartup - timeOnLastSpawn > spawnRate) 
		{
			SpawnObject ();
			timeOnLastSpawn = Time.realtimeSinceStartup;
		}
	}

	void SpawnObject()
	{
       /* if(onWater)
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
        }*/

		//Determine which object of the set it can spawn should be spawned in this instance
		int objectID = 0;
		int random = Random.Range (0, weightedSpawnChanceSum [numObjects - 1]);
		int currentSum = 0;
		for(int i = 0; i < numObjects; ++i)
		{
			currentSum += objects[i].weightedSpawnChance;
			if(currentSum >= random)
			{
				objectID = i;
				break;
			}
		}

		//Instantiate object, assign parameters and set direction 
		GameObject spawnedObject = Instantiate(objects[objectID].objectToSpawn, gameObject.transform.position, Quaternion.identity) as GameObject;
		baseObject baseScript = spawnedObject.GetComponent<baseObject> ();
		baseScript.assignParameters(objects[objectID],mapScale);
		if (direction == SpawnDirection.LEFT) 
		{
			baseScript.InvertDirection();
		}
		spawnedObject.transform.parent = transform.parent;
			
	

		/*switch (spawnWhat) 
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
                
		}*/
	
	}
}
