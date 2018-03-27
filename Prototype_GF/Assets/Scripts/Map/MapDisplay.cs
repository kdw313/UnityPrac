using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Map display class
public class MapDisplay : MonoBehaviour {

	public Renderer textureRender;

	public void DrawNoiseMap(float[,] noiseMap){
		
		int width = noiseMap.GetLength (0);
		int height = noiseMap.GetLength (1);

		Texture2D texture = new Texture2D (width, height);

		/**FYI: faster to generate all color and set them
		 	   in a same time
 	    **/
		Color[] colorMap = new Color[width * height];

		// generates color maps once
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colorMap [y * width + x] = Color.Lerp (Color.black, Color.white, noiseMap[x, y]);
			}
		}

		// apply all together to texture
		texture.SetPixels (colorMap);
		texture.Apply ();

		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3 (width, 1, height);
	}
}
