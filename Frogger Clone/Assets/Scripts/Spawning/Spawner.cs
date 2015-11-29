using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
	private ObjectManager objMan;

	private float spawnRate = 1.0f; //Need validate based on size of object and speed of object for spawnRate
	private float timeOnLastSpawn = 0.0f;
	private const float SPAWNRATE_SCALER = 6; //Adjusts spawnrate based on speed and size (6 seems to be the sweet spot)

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

	private float randSpeedMod = 1;

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
		randSpeedMod = Random.Range (0.5f, 2.0f);
	}

	//Adds a spawnable object type to list of objects that can be spawned
	public void SetupSpawnableObject(ObjectType _object)
	{
		//Get prefab from object manager
		objects[currentObjects] = objMan.GetObjectData(_object);

		//Adds random variation to speed
		objects[currentObjects].speedX *= randSpeedMod;
		//Adds random variation to spawn rate based on size and speed
		if (objects [currentObjects].sizeX / 2 > spawnRate) 
		{
			spawnRate = ((objects [currentObjects].sizeX / 2) / (objects [currentObjects].speedX/SPAWNRATE_SCALER)) * Random.Range(0.75f, 2.0f);
		}

		//Begin setting up the weighted chances as some spawners can spawn multiple different things
		totalWeightedSpawnChance += objects[currentObjects].weightedSpawnChance;
		weightedSpawnChanceSum[currentObjects] = totalWeightedSpawnChance;

		++currentObjects;
	}

	//Alters the location of the spawner based on the largest object that can be spawned
	public void AdjustPosition()
	{
		float largestSize = 0;
		for(int i = 0; i < numObjects; ++i)
		{
			if(objects [i].sizeX > largestSize)
			{
				largestSize = objects [i].sizeX;
			}
		}
		Vector3 newPos = transform.position;
		switch (direction) 
		{
		case SpawnDirection.LEFT:
			newPos.x += largestSize;
			break;
		case SpawnDirection.RIGHT:
			newPos.x -= largestSize;
			break;
		}
		transform.position = newPos;
	}

	public void SpawnInitialObjects ()
	{
		int randObject = SelectRandomObject ();
		float distance = (objects [randObject].speedX) * spawnRate;
		int duplicates = 0;
		while(true)
		{
			Vector3 spawnPos = gameObject.transform.position;
			spawnPos.x += (distance * duplicates);
			if(spawnPos.x < 150f && direction == SpawnDirection.RIGHT)
			{
				GameObject spawnedObject = SpawnLogic (randObject);
				spawnedObject.transform.position = spawnPos;
				spawnedObject.transform.parent = transform.parent;
					
				++duplicates;
			}
			else if(spawnPos.x > 0.0f && direction == SpawnDirection.LEFT)
			{
				GameObject spawnedObject = SpawnLogic (randObject);
				spawnedObject.transform.position = spawnPos;
				spawnedObject.transform.parent = transform.parent;	

				--duplicates;
			}
			else
			{
				break;
			}

		}

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

	public void SpawnObject()
	{
		GameObject spawnedObject = SpawnLogic (SelectRandomObject());
		spawnedObject.transform.parent = transform.parent;	
	}

	int SelectRandomObject()
	{
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
		return objectID;
	}

	GameObject SpawnLogic(int _objectID)
	{
		//Instantiate object, assign parameters and set direction 
		GameObject spawnedObject = Instantiate(objects[_objectID].objectToSpawn, gameObject.transform.position, Quaternion.identity) as GameObject;
		baseObject baseScript = spawnedObject.GetComponent<baseObject> ();
		baseScript.assignParameters(objects[_objectID],mapScale);
		if (direction == SpawnDirection.LEFT) 
		{
			baseScript.InvertDirection();
		}
		return spawnedObject;
	}
}
