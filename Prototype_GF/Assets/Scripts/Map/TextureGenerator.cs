﻿using System.Collections;
using UnityEngine;

public static class TextureGenerator {

	public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height) {
		Texture2D texture = new Texture2D (width, height);

		//Make texture less blurry//block


		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;

		texture.SetPixels (colorMap);
		texture.Apply ();
		return texture;
	}

	public static Texture2D TextureFromHeightMap(float[,] heightMap) {
		int width = heightMap.GetLength (0);
		int height = heightMap.GetLength (1);

		// Texture2D texture = new Texture2D (width, height);

		/**FYI: faster to generate all color and set them
		 	   in a same time
 	    **/
		Color[] colorMap = new Color[width * height];

		// generates color maps once
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colorMap [y * width + x] = Color.Lerp (Color.black, Color.white, heightMap[x, y]);
			}
		}

		return TextureFromColorMap (colorMap, width, height);
	}
}

