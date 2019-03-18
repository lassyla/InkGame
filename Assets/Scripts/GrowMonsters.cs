using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowMonsters : MonoBehaviour {

	//range -20 to 20 for x and z. y = 1.5. 
	GameObject yellow, magenta, cyan; 
	float minRate = 4; 
	float maxRate = 7; 
	int cap = 8; 
	void Start () {
		cyan = GameObject.Find ("cyanMonster"); 
		yellow = GameObject.Find ("yellowMonster"); 
		magenta = GameObject.Find ("magentaMonster"); 
	
		Invoke ("addMonster", 0.0f); 

	}
	
	// Update is called once per frame
	void Update () {
	}

	void addMonster() {
		if (gameObject.transform.childCount < cap) {
			float random = Random.Range (0.0f, 1.0f); 
			GameObject newMonster; 
			if (random < 0.33f) {
				newMonster = Instantiate (cyan, new Vector3 (Random.Range (-20.0f, 20.0f), 0, Random.Range (-20.0f, 20.0f)), Quaternion.identity); 
			}
			else if (random < 0.66f) {
				newMonster = Instantiate (yellow, new Vector3 (Random.Range (-20.0f, 20.0f), 0, Random.Range (-20.0f, 20.0f)), Quaternion.identity); 
			}
			else {
				newMonster = Instantiate (magenta, new Vector3 (Random.Range (-20.0f, 20.0f), 0, Random.Range (-20.0f, 20.0f)), Quaternion.identity); 
			}
			MonsterLookAt script = (MonsterLookAt)newMonster.GetComponent ("MonsterLookAt");
			script.isOG = false; 
			newMonster.transform.parent = gameObject.transform; 
		}

		Invoke ("addMonster", Random.Range(minRate, maxRate)); 
	}
}
