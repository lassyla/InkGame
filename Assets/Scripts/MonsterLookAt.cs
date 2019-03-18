using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLookAt : MonoBehaviour {

	Vector3 destination; 
	float rate = .1f; 
	float distance = 1.0f; 
	float scale;
	float growSize = 1f; 
	float finalSize = 20; 
	public bool isOG; 

	// Use this for initialization
	void Start () {
		if (!isOG) {
			
			Invoke ("newDestination", 0.0f); 
			scale = 0.01f; 
			transform.localScale = new Vector3 (scale, scale, scale); 
		}
	}

	// Update is called once per frame
	void Update () {
		if (!isOG) {
			if (Vector3.Distance (destination, transform.position) > 1.0) {
				Vector3 direction = Vector3.Normalize (destination - transform.position); 
			
				transform.position += direction * rate; 
			}
			if (scale < finalSize) {
				scale += growSize; 
				transform.localScale = new Vector3 (scale, scale, scale); 
			}
		}
	}


	void newDestination() {
		destination = new Vector3 (Random.Range (-20.0f, 20.0f), 0, Random.Range (-20.0f, 20.0f)); 
		distance = Vector3.Distance (destination, transform.position); 
		transform.LookAt(destination); 
		if (gameObject.tag == "magenta" || gameObject.tag == "cyan") {
			transform.Rotate (0, 180, 0); 
		}



		Invoke ("newDestination", Random.Range(4, 7)); 
	}
}
