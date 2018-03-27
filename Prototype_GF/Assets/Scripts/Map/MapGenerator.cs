using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script to generate map
public class MapGenerator : MonoBehaviour {

	public int mapWidth;
	public int mapHeight;
	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public bool autoUpdate;

	public void GenerateMap(){
		
		// fetch 2d array by Noise Class
//		float[,] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, noiseScale);

//		float[,] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, noiseScale, octaves, persistance, lacunarity);

		float[,] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

		// drawing
		MapDisplay display = FindObjectOfType<MapDisplay> ();

		display.DrawNoiseMap (noiseMap);
	}


	// check the script value changed on editor
	void OnValidate() {

		if (mapWidth < 1) {
			mapWidth = 1;
		}

		if (mapHeight < 1) {
			mapHeight = 1;
		}

		if (lacunarity < 1) {
			lacunarity = 1;
		}

		if (octaves < 0) {
			octaves = 0;
		}


	}
			
}
