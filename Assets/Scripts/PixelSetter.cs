using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class PixelSetter : MonoBehaviour {

	public Transform playerTransform; 

	Transform planeTransform; 
	Color color; 
	ParticleSystem ps; 
	Texture2D texture; 

	int resolution = 400;
	int planeSize = 10; 
	int paintRadius = 1; 
	int floodSpeed = 20; 
	float fillDelay = .9f; 
	float drawDelay = .9f; 
	bool fillMode = false; 
	float scale; 

	struct Point
	{
		public int x, y;
		public Point(int px, int py)
		{
			x = px;
			y = py;
		}
	}

	void Start()
    	{
		GameObject go = GameObject.Find ("Particle System"); 
		ps = (ParticleSystem) go.GetComponent("ParticleSystem");; 
		planeTransform = gameObject.transform;

		scale = resolution / 50; 
		color = Color.white;

		var emission = ps.emission; 
		emission.rateOverDistance = 0; 

        	texture = new Texture2D(resolution, resolution, 
					textureFormat: TextureFormat.RGBA32, 
					mipChain: false);
		texture.filterMode = FilterMode.Point;
        	GetComponent<Renderer>().material.mainTexture = texture;

		for (var x = 0; x < resolution; x++) {
			for (var y = 0; y < resolution; y++) {
				texture.SetPixel (x, y, Color.white); 
			}
		}

        	texture.Apply();
	}

	// Called whenever the player collides with an ink monster.
	public void SetColor(Color newColor, Color particleColor) {
		// If the same color bug was consumed, increase the players line width
		if (newColor == color) {
			var em = ps.emission; 
			em.rateOverDistance = 5 + paintRadius * 2; 
			paintRadius++;
		}

		//Otherwise, change the paint color and paint particles 
		else {
			var emission = ps.emission; 
			emission.rateOverDistance = 5; 

			color = newColor; 
			ps.startColor = particleColor; 
			paintRadius = 1; 
		}
	}


	void Update() {
		StartCoroutine("DrawPoint"); 

		if (Input.GetKeyDown("space"))
        	{
			StartCoroutine("Fill"); 
        	}

		if (Input.GetKeyDown("e")) 
		{
			StartCoroutine("Clear"); 
		}

		// Saves the file 
		if (Input.GetKeyDown ("f")) { 
			byte[] bytes = texture.EncodeToPNG (); 
			System.IO.File.WriteAllBytes (Application.dataPath + "/../image.png", bytes); 
		}
	}

	// Constantly draws a trail behind the player
	IEnumerator DrawPoint() {
		Color newColor = color;
		int x = (resolution / 2 +  (int)(scale * playerTransform.position.z)); 
		int y = (resolution/2 -  (int)(scale * playerTransform.position.x)); 

		// Delay the trail so it matches up with the particles. 
		yield return new WaitForSeconds(drawDelay);

		for(var x1 = x - paintRadius; x1 < x + paintRadius + 1; x1++) {
			for(var y1 = y - paintRadius; y1 < y + paintRadius + 1; y1++) {
				texture.SetPixel(x1, y1 , newColor); 
			}
		}

		texture.Apply(); 
	}

	// Clears the floor texture by setting all pixels to white
	IEnumerator Clear() {
		for (var x = 0; x < resolution; x++) {
			for (var y = 0; y < resolution; y++) {
				texture.SetPixel (x, y, Color.white); 
			}
			if (x % 10 == 0) yield return null; 
		}
	}

	// Flood fills the area after a provided delay. 
	IEnumerator Fill() {
		yield return new WaitForSeconds(fillDelay);

		ps.Emit (100); 

		int x = (resolution / 2 +  (int)(scale * playerTransform.position.z)); 
		int y = (resolution/2 -  (int)(scale * playerTransform.position.x)); 

		Color newColor = color; 
		Color oldColor = color; 

		Queue<Point> q = new Queue<Point> (); 
		bool[,] visited = new bool[resolution, resolution]; 
		int numFilled = 0; 

		for(var x1 = x - paintRadius - 1; x1 < x + paintRadius + 2; x1++) {
			for(var y1 = y - paintRadius - 1; y1 < y + paintRadius + 2; y1++) {
				Color c = texture.GetPixel(x1, y1); 
				if (!c.Equals(color)) {
					oldColor = c; 
					q.Enqueue(new Point(x1, y1)); 
				}
			}
		}

		// Using a DFS, flood fills all pixels that have the old color. 
		while(q.Count > 0) {
			Point p = q.Dequeue(); 
			x = p.x; 
			y = p.y; 
			if(x >= 0 && x < resolution && y >= 0 && y < resolution) {

				if(!visited[x,y]){
					visited[x,y] = true; 

					if(texture.GetPixel(x, y).Equals(oldColor)) {
						texture.SetPixel(x, y, newColor); 

						// Queue adjacent pixels to have their color changed
						q.Enqueue(new Point(x+1, y)); 
						q.Enqueue(new Point(x-1, y)); 
						q.Enqueue(new Point(x, y+1)); 
						q.Enqueue(new Point(x, y-1)); 

						numFilled ++; 
						
						if(numFilled >= floodSpeed) {
							floodSpeed = (int) (floodSpeed * 1.1);
							texture.Apply(); 
							numFilled = 0; 
							yield return null; 
						}
					}
				}
			}
		}
		texture.Apply(); 
	}
}
