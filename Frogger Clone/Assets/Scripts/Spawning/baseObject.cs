﻿using UnityEngine;
using System.Collections;


public class baseObject : MonoBehaviour {

	float speedx;
	float speedz;
	
	// Use this for initialization
	void Start () 
	{
	
	}


    public void test()
    {

    }

	public void assignParameters(statsStruct _struct, float _mapScale)
	{
		speedx = _struct.speedX * _mapScale;
		speedz = _struct.speedY;
		
		transform.localScale = new Vector3 (_struct.sizeX * _mapScale, _struct.sizeY *_mapScale, 1 );

	}
    

	void FixedUpdate() 
	{
		transform.Translate ( speedx * Time.deltaTime , 0 , 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
