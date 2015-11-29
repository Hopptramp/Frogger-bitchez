using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class LevelTileManager : MonoBehaviour
{
	public enum TileTypes
	{
		PLAIN = 0,
		WATER,
		ROAD,
		ALL_TYPES
	}

	private TileTypes[] tileStructure;

	public int bottomBound = 3;
	public int topBound = 5;

    //For storing bound pairs.
    [System.Serializable]
    public struct intPair
    {
        public int top, bottom;
    }
    
    //Size of world
    public int columns = 150;
    public int rows = 75;
    public float mapScale = 1;

    //Objects to use as tiles
    public List<GameObject> floorTile = new List<GameObject>();
    public List<GameObject> waterTile = new List<GameObject>();
    public GameObject roadTile;
    

    //Spawner object
    public GameObject aSpawner;
    public GameObject destructionBox;

    //Ranges for tiles
    public List <intPair> waterBounds = new List<intPair>();
    public List <intPair> roadBounds = new List<intPair>();
    
    

    //Transform to parent the tiles to.
    private Transform mapHolder;


    private List<Vector3> gridPositions = new List<Vector3>();

    //Reference Positions -----Not used yet
    void InitialiseList()
    {
        gridPositions.Clear();
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y< rows; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

	bool IsEven(int _number)
	{
		if (_number % 2 == 0) 
		{
			return true;
		}
		return false;
	}
    
    void Validate()
    {
		//The upper and lower bound both need to be odd numbers
		if (IsEven (bottomBound)) 
		{
			++bottomBound;
		}
		if (IsEven (topBound)) 
		{
			++topBound;
		}

		ValidateList (waterBounds);
		ValidateList (roadBounds);
    }

	void ValidateList(List <intPair> _list)
	{
		for(int i = 0;i < _list.Count; i++)
		{
			//If bottom bound is above top bound the switch them over
			if(_list[i].bottom > _list[i].top)
			{
				intPair temp;
				temp.top = _list[i].bottom;
				temp.bottom = _list[i].top;
				_list[i] = temp;
			}

			//Bottom value should be odd
			if(IsEven(_list[i].bottom))
			{
				intPair temp = _list[i];
				//If on same space decrease otherwise increase
				if(temp.top == temp.bottom)
				{
					temp.bottom -= 1;
				}
				else
				{
					temp.bottom += 1;
				}

				_list[i] = temp;

			}
			//Top value should be even
			if(!IsEven(_list[i].top))
			{
				intPair temp = _list[i];
				//If on same space increase otherwise decrease
				if(temp.top == temp.bottom)
				{
					temp.bottom += 1;
				}
				{
					temp.top -= 1;
				}
				_list[i] = temp;
			}

			//Move bounds so they dont go over bottom boundaries
			if (_list[i].bottom < bottomBound)
			{
				intPair temp = _list[i];
				temp.bottom = bottomBound;
				_list[i] = temp;

				//If previous bottom boundary is now above top boundary
				if(_list[i].bottom >= _list[i].top)
				{
					intPair temp2 = _list[i];
					temp2.top = temp2.bottom + 1;
					_list[i] = temp2;
				}
			}
			//Move so don't go over top boundaries
			if (_list[i].top > rows - topBound)
			{
				intPair temp = _list[i];
				temp.top = rows - topBound;
				_list[i] = temp;

				//If previous top boundary is now below bottom boundary
				if(_list[i].top <= _list[i].bottom)
				{
					intPair temp2 = _list[i];
					temp2.bottom = temp2.bottom - 1;
					_list[i] = temp2;
				}
			}
		}
	}


    void MapSetup()
    {
        mapHolder = new GameObject("Map").transform;

        //Instantiate an object for each x,y co-oord below number of rows and columns
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                //Set the object to instantiate to a random floor tile;
                GameObject toInstantiate = floorTile[Random.Range(0,floorTile.Count)];

                // Put logic here for different tiles
                //---------------------------------------------------------------------------//


                // Check if tile is between any pair of bounding values, and chnage to instantiate accordingly
                foreach (intPair ints in waterBounds)
                {
                    if (y >= ints.bottom && y <= ints.top)
                    {
                        toInstantiate = waterTile[Random.Range(0, waterTile.Count)];   
						tileStructure[y] = TileTypes.WATER;
                    }
                }
                foreach (intPair ints in roadBounds)
                {
                    if (y >= ints.bottom && y <= ints.top)
                    {
                        toInstantiate = roadTile;
						tileStructure[y] = TileTypes.ROAD;
                    }
                }
                

                //---------------------------------------------------------------------------//


                //Instantiate the object
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set object parent to clean up hierarchy
                instance.transform.SetParent(mapHolder);
                //Set to be static.
                instance.isStatic = true;

            }
        }


        // for all areas that are water or roads, place spawners.
        /*for (int y = 0; y < rows; y++)
        {

            Vector3 spawnerPos = Vector3.zero;
            bool waterSpawn = false;
            bool moveLeft = false;
            foreach (intPair ints in waterBounds)
            {

                if (y >= ints.bottom && y <= ints.top)
                {
                    if (Random.value <= 0.5f)
                    {
                        spawnerPos = new Vector3(columns + 2, y, 0);
                        moveLeft = true;
                    }
                    else
                    {
                        spawnerPos = new Vector3(-3, y, 0);
                        moveLeft = false;
                    }
                    waterSpawn = true;
                }
            }
            foreach (intPair ints in roadBounds)
            {
                if (y >= ints.bottom && y <= ints.top)
                {
                    if (Random.value <= 0.5f)
                    {
                        spawnerPos = new Vector3(columns + 2, y, 0);
                        moveLeft = true;
                    }
                    else
                    {
                        spawnerPos = new Vector3(-3, y, 0);
                        moveLeft = false;
                    }
                    waterSpawn = false;
                    
                }
            }*/

			//For every grouping of two rows place a spawner
			for(int i = 0; i < rows; ++i)
			{
				switch(tileStructure[i])
				{
				case TileTypes.WATER:
					SetupSpawner(i + 0.5f, tileStructure[i]);
					++i;
					break;
				case TileTypes.ROAD:
					SetupSpawner(i + 0.5f, tileStructure[i]);
					++i;
					break;
				}
			}
            
            //if (spawnerPos != Vector3.zero)
           // {
				
            //}
        //}

		//Instantiate death box for objects !!!Needs to be configured as to not delete the larger spawned objects immediately
        GameObject leftBox = Instantiate(destructionBox, new Vector3(-30, rows/2 ,0), Quaternion.identity) as GameObject;
        leftBox.transform.localScale = new Vector3 (1,rows,1);
        leftBox.transform.parent = mapHolder;
        GameObject rightBox = Instantiate(destructionBox, new Vector3(columns + 29, rows / 2, 0), Quaternion.identity) as GameObject;
        rightBox.transform.localScale = new Vector3(1, rows, 1);
        rightBox.transform.parent = mapHolder;

        //Move mapholder to correct position
        mapHolder.transform.position = new Vector3(- mapScale * (columns / 2) +(0.5f*mapScale), -mapScale*(rows / 2), 0f);
        mapHolder.transform.localScale *=  mapScale;
        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        //Make sure camera orthographic size is set to half the row number in the inspector. Won't work properly here.
        mainCam.GetComponent<Camera>().orthographicSize *= mapScale;
        
    }

	void SetupSpawner(float _yPos, TileTypes _type)
	{
		//Determine spawner position !!!Needs to be altered as larger objects pop onto screen
		Vector3 spawnerPos;
		Spawner.SpawnDirection dir;
		if (Random.value <= 0.5f)
		{
			spawnerPos = new Vector3(columns + 2, _yPos, 0);
			dir = Spawner.SpawnDirection.LEFT;
		}
		else
		{
			spawnerPos = new Vector3(-3, _yPos, 0);
			dir = Spawner.SpawnDirection.RIGHT;
		}

		//If a position for a spawner has been designated, then instantiate it and pass in the mapscale.
		GameObject spawnerInstance = Instantiate(aSpawner, spawnerPos, Quaternion.identity) as GameObject;
		Spawner spawner = spawnerInstance.GetComponent<Spawner>();

		
		// spawnerInstance.GetComponent<Spawner>().onWater = waterSpawn;
		// spawnerInstance.GetComponent<Spawner>().moveLeft = moveLeft;

		//The types of things that can be spawned on each tile type
		switch(_type)
		{
		case TileTypes.WATER:
			//Can spawn 1 type of object
			spawner.SetupSpawnerBasics(1, mapScale, dir);
			//Random variation for what objects to use
			float rand = Random.value;
			if(rand <= 0.3)
			{
				spawner.SetupSpawnableObject(ObjectType.SMALL_LOG);
			}
			else if(rand <= 0.7)
			{
				spawner.SetupSpawnableObject(ObjectType.MEDIUM_LOG);
			}
			else
			{
				spawner.SetupSpawnableObject(ObjectType.BIG_LOG);
			}
			break;
		case TileTypes.ROAD:
			spawner.SetupSpawnerBasics(2, mapScale, dir);
			float rand2 = Random.value;
			if(rand2 <= 0.3)
			{
				spawner.SetupSpawnableObject(ObjectType.CAR1);
				spawner.SetupSpawnableObject(ObjectType.CAR2);
			}
			else if(rand2 <= 0.7)
			{
				spawner.SetupSpawnableObject(ObjectType.CAR3);
				spawner.SetupSpawnableObject(ObjectType.CAR2);
			}
			else
			{
				spawner.SetupSpawnableObject(ObjectType.CAR2);
				spawner.SetupSpawnableObject(ObjectType.CAR3);
			}
			break;
		}

		//Alters position of spawner based on largest object it can spawn
		spawner.AdjustPosition ();

		//spawner.mapScale = mapScale;
		//spawner.SetupDirection(dir);

		//Parent spawners to the map
		spawnerInstance.transform.SetParent(mapHolder);
	}

	public TileTypes TileAtPosition(int _position)
	{
		return tileStructure [_position];
	}

	public bool PositionOnTile(int _position, TileTypes _tile)
	{
		_position = (int)(_position + (rows/2) + 1);
		if (tileStructure [_position] == _tile) 
		{
			return true;
		}
		return false;
	}

	public bool ObjectOnTile(GameObject _object, TileTypes _tile)
	{
		int objectPos = (int)((_object.transform.position.y) + (rows/2) + 1);
		if (tileStructure [objectPos] == _tile) 
		{
			return true;
		}
		return false;
	}


	/*//check if the player is on water
	public bool playerOnWater(GameObject _player, )
	{
		float playerPos = (_player.transform.position.y) + (rows/2)+ 1;
		intPair bridge;
		bridge.top = 1;
		bridge.bottom = 1;

		foreach (intPair floats in waterBounds) 
		{	
			foreach (intPair roads in roadBounds)
			{
				if (roads.bottom > floats.bottom && roads.top < floats.top)
				{
					bridge.top = roads.top;
					bridge.bottom = roads.bottom;
				}
			}
		}
		
		foreach (intPair floats in waterBounds) 
		{
			if (playerPos >= floats.bottom && playerPos <= floats.top) 
			{
				if (playerPos >= bridge.top || playerPos <= bridge.bottom)
				{
					// you are on water
					return true;
				}
			}
		}
		//you are not on water
		return false;
	}*/


    // Use this for initialization
    void Start()
    {
		tileStructure = new TileTypes[rows];
        Validate();
        InitialiseList();
        MapSetup();

    }

    // Update is called once per frame
    void Update () {
	
	}
}
