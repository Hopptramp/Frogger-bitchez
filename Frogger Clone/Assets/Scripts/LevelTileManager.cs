using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class LevelTileManager : MonoBehaviour {

    //For storing bound pairs.
    [System.Serializable]
    public struct floatPair
    {
        public float top, bottom;
    }



    //Size of world
    public int columns = 150;
    public int rows = 75;
    public float mapScale = 1;

    //Objects to use as tiles, and ranges to place them in
    public List<GameObject> floorTile = new List<GameObject>();
    public List<GameObject> waterTile = new List<GameObject>();
    public GameObject roadTile;

    public GameObject aSpawner;

    public List <floatPair> waterBounds = new List<floatPair>();
    public List <floatPair> roadBounds = new List<floatPair>();
    
    

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
                foreach (floatPair floats in waterBounds)
                {

                    if (y >= floats.bottom && y <= floats.top)
                    {
                        toInstantiate = waterTile[Random.Range(0, waterTile.Count)];
                    }
                }
                foreach (floatPair floats in roadBounds)
                {
                    if (y >= floats.bottom && y <= floats.top)
                    {
                        toInstantiate = roadTile;
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

        for (int y = 0; y < rows; y++)
        {
            Vector3 spawnerPos = Vector3.zero;
            int toSpawn = 0;
            foreach (floatPair floats in waterBounds)
            {

                if (y >= floats.bottom && y <= floats.top)
                {
                    spawnerPos = new Vector3(-3,y,-0.2f);
                    toSpawn = 1;
                }
            }
            foreach (floatPair floats in roadBounds)
            {
                if (y >= floats.bottom && y <= floats.top)
                {
                    spawnerPos = new Vector3(columns + 2, y, -0.2f);
                    toSpawn = 2;
                }
            }
            if (spawnerPos != Vector3.zero)
            {
                GameObject spawnerInstance = Instantiate(aSpawner, spawnerPos, Quaternion.identity) as GameObject;
                spawnerInstance.GetComponent<Spawner>().spawnWhat = toSpawn;
                spawnerInstance.GetComponent<Spawner>().logStats.sizeX *= mapScale;
                spawnerInstance.GetComponent<Spawner>().logStats.sizeY *= mapScale;
                spawnerInstance.transform.SetParent(mapHolder);
            }
        }
        //Move mapholder to correct position
        mapHolder.transform.position = new Vector3(- mapScale * (columns / 2) +(0.5f*mapScale), -mapScale*(rows / 2), 0f);
        mapHolder.transform.localScale *=  mapScale;
        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        //Make sure camera orthographic size is set to half the row number in the inspector. Won't work properly here.
        mainCam.GetComponent<Camera>().orthographicSize *= mapScale;
        
    }


    // Use this for initialization
    void Start()
    {
        InitialiseList();
        MapSetup();

    }

    // Update is called once per frame
    void Update () {
	
	}
}
