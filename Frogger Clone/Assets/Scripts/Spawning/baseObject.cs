using UnityEngine;
using System.Collections;


public class baseObject : MonoBehaviour 
{

	private float speedx;
	private float speedz;
	
	// All sprites are facing wrong way in context of game so reverse sprite here by default
	void Start () 
	{
		Vector3 scale = gameObject.transform.GetChild (0).localScale;
		scale.x *= -1;
		gameObject.transform.GetChild (0).localScale = scale;
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
		Vector3 scale = gameObject.transform.GetChild (0).localScale;
		scale.x *= -1;
		gameObject.transform.GetChild (0).localScale = scale;
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
