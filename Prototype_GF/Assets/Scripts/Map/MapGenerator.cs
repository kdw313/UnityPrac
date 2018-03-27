using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script to generate map
public class MapGenerator : MonoBehaviour {

	public enum DrawMode
	{
		NoiseMap,
		ColorMap,
		MeshMap
	};

	public DrawMode drawMode;

	// Limit: <= 255^2 
	// The actual limit is 65535 vertices (256^2 -1), 
	// but 255^2 is the most we can have if our map is square.
	public int mapChunkSize = 241;
	// 1 for no simplification, < 1 to simplification
	[Range(1,6)]
	public int levelOfDetail;

	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public TerrainType[] regions;

	public bool autoUpdate;

	public void GenerateMap(){
		
		// fetch 2d array by Noise Class
//		float[,] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, noiseScale);

//		float[,] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, noiseScale, octaves, persistance, lacunarity);

		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

		Color[] colorMap = new Color[mapChunkSize * mapChunkSize];

		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {
				
				float currentHeight = noiseMap [x, y];

				for (int i = 0; i < regions.Length; i++) {
					if (currentHeight <= regions [i].height) {
						colorMap [y * mapChunkSize + x] = regions [i].color;
						break;
					}
				}

			}
		}

		// drawing
		MapDisplay display = FindObjectOfType<MapDisplay> ();

		if (drawMode == DrawMode.NoiseMap)
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (noiseMap));
		else if (drawMode == DrawMode.ColorMap)
			display.DrawTexture (TextureGenerator.TextureFromColorMap (colorMap, mapChunkSize, mapChunkSize));
		else if (drawMode == DrawMode.MeshMap)
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap (colorMap, mapChunkSize, mapChunkSize));
	}


	// check the script value changed on editor
	void OnValidate() {
		if (lacunarity < 1) {
			lacunarity = 1;
		}

		if (octaves < 0) {
			octaves = 0;
		}
	}
			
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color color;
}