using UnityEngine;
using System.Collections;

public class TurtleScript : MonoBehaviour 
{
	Animator anim ;
	float count = 0;
	float maxSwim = 5.0f;
	float maxDive = 5.0f;
	bool dived = false;

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

	void Swimming()
	{
		float total = maxSwim + maxDive;

		if (count >= 0 && count < maxSwim) 
		{
			anim.SetBool("isDiving", false);
			gameObject.GetComponent<Collider2D>().isTrigger = false;

		} else if (count >= maxSwim && count < total && dived == false) 
		{
			anim.SetBool("isDiving", true);
			gameObject.GetComponent<Collider2D>().isTrigger = true;
			dived = true;
		}
		else if (count > total)
		{
			count = 0;
			dived = false;
		}
		count += Time.deltaTime;
	}
}
