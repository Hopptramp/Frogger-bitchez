using UnityEngine;
using System.Collections;

public class baseObject : MonoBehaviour {

	float speedx;
	float speedz;
	
	// Use this for initialization
	void Start () 
	{
	
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Boundary") 
		{
			Destroy(this);
		}
		
	}

	public void assignParameters(logStructs _struct)
	{
		speedx = _struct.speedX;
		speedz = _struct.speedY;
		
		transform.localScale = new Vector3 (_struct.sizeX, _struct.sizeY, 1 );
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
