using UnityEngine;
using System.Collections;

public class Scrolling : MonoBehaviour {

	public float speedx = 1.0f;
	public float speedy = 1.0f;
	public float speedz = 1.0f;


	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate() {

		transform.Translate ( speedx * Time.deltaTime , speedy * Time.deltaTime , speedz * Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
