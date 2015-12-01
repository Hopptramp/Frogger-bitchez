using UnityEngine;
using System.Collections;

public class TurtleScript : MonoBehaviour 
{
	Animator anim ;
	float count = 0;
	float maxSwim = 10.0f;
	float maxDive = 2.5f;

	// Use this for initialization
	void Start () 
	{
		anim = gameObject.GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Swimming ();
	}

	public void ApplyRandomDive(float _swim, float _dive, float _start)
	{
		maxSwim += _swim;
		maxDive += _dive;
		count += _start;
	}

	void Swimming()
	{
		float total = maxSwim + maxDive;

		if (count >= 0 && count < maxSwim) 
		{
			anim.SetBool("isDiving", false);
			//gameObject.GetComponent<Collider2D>().enabled = true;
			gameObject.GetComponent<Collider2D>().offset = new Vector2(0,0);

		} else if (count >= maxSwim && count < total)// && dived == false) 
		{
			anim.SetBool("isDiving", true);
			if(count > maxSwim + 0.6f)
			{
				gameObject.GetComponent<Collider2D>().offset = new Vector2(200,200);
				//gameObject.GetComponent<Collider2D>().enabled = false;
			}
		}
		else if (count > total)
		{
			count = 0;
		}
		count += Time.deltaTime;
	}

}
