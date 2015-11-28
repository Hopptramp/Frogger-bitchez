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
        for (int y = 0; y < rows; y++)
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
            }
            
            if (spawnerPos != Vector3.zero)
            {
                //If a position for a spawner has been designated, then instantiate it and pass in the mapscale.
                GameObject spawnerInstance = Instantiate(aSpawner, spawnerPos, Quaternion.identity) as GameObject;
                spawnerInstance.GetComponent<Spawner>().onWater = waterSpawn;
                spawnerInstance.GetComponent<Spawner>().moveLeft = moveLeft; 
                spawnerInstance.GetComponent<Spawner>().mapScale = mapScale;
                //Parent spawners to the map
                spawnerInstance.transform.SetParent(mapHolder);
            }
        }
        GameObject leftBox = Instantiate(destructionBox, new Vector3(-6, rows/2 ,0), Quaternion.identity) as GameObject;
        leftBox.transform.localScale = new Vector3 (1,rows,1);
        leftBox.transform.parent = mapHolder;
        GameObject rightBox = Instantiate(destructionBox, new Vector3(columns + 5, rows / 2, 0), Quaternion.identity) as GameObject;
        rightBox.transform.localScale = new Vector3(1, rows, 1);
        rightBox.transform.parent = mapHolder;

        //Move mapholder to correct position
        mapHolder.transform.position = new Vector3(- mapScale * (columns / 2) +(0.5f*mapScale), -mapScale*(rows / 2), 0f);
        mapHolder.transform.localScale *=  mapScale;
        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        //Make sure camera orthographic size is set to half the row number in the inspector. Won't work properly here.
        mainCam.GetComponent<Camera>().orthographicSize *= mapScale;
        
    }

	public TileTypes TileAtPosition(int _position)
	{
		return tileStructure [_position];
	}

	public bool PositionOnTile(int _position, TileTypes _tile)
	{
		if (tileStructure [_position] == _tile) 
		{
			return true;
		}
		return false;
	}

	public bool ObjectOnTile(GameObject _object, TileTypes _tile)
	{
		float objectPos = (_object.transform.position.y) + (rows/2)+ 1;
		if (tileStructure [(int)objectPos] == _tile) 
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
