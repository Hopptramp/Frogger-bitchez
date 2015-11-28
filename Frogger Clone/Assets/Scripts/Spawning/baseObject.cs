using UnityEngine;
using System.Collections;


public class baseObject : MonoBehaviour 
{

	private float speedx;
	private float speedz;
	
	// Use this for initialization
	void Start () 
	{
	
	}


    public void test()
    {

    }

	public void assignParameters(StatsStruct _struct, float _mapScale)
	{
		speedx = _struct.speedX * _mapScale;
		speedz = _struct.speedY;
		
		transform.localScale = new Vector3 (_struct.sizeX * _mapScale, _struct.sizeY *_mapScale, 1 );

	}

	public void InvertDirection()
	{
		speedx = -speedx;
		speedz = -speedz;
	}
    

	void FixedUpdate() 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate ( speedx * Time.deltaTime , 0 , 0);
	}
}
