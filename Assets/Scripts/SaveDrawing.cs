using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDrawing : MonoBehaviour {

	public RenderTexture rt; 
	private Texture2D texture; 

	void Start()
    {
        texture = new Texture2D(128, 128);
        GetComponent<Renderer>().material.mainTexture = texture;
    }

//doesnt do anything :')))))))))))))0000000
	void Update() {

		RenderTexture.active = rt; 
		texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0); 
        texture.Apply();
	}
}