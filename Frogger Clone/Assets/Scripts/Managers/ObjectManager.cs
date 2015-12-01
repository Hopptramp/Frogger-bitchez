using UnityEngine;
using System.Collections;

[System.Serializable]
public enum ObjectType
{
	SMALL_LOG = 0,
	MEDIUM_LOG,
	BIG_LOG,
	CROC,
	CAR1,
	CAR2,
	CAR3,
	VAN,
	SINGLE_TURTLE,
	DOUBLE_TURTLE,
	TRIPLE_TURTLE,
	ALL_TYPES
}

[System.Serializable]
public struct StatsStruct
{
	public ObjectType type;
	public float sizeX;
	public float sizeY;
	[Range(0, 1000)] public float speedX;
	[Range(0, 1000)] public float speedY;
	public GameObject objectToSpawn;
	public int weightedSpawnChance;
}

public class ObjectManager : MonoBehaviour 
{
	//All types of spawnable objects
	public StatsStruct[] objects;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public StatsStruct GetObjectData(ObjectType _type)
	{
		return objects[(int)_type];
	}
}


