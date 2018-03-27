using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// noise class
public static class Noise {

	public static int MIN_OCTAVE_OFFSET = 100000;
	public static int MAX_OCTAVE_OFFSET = -100000;

	public static float[,] GenerateNoiseMap (int mapWidth, int mapHeight, float noiseScale) {
		float[,] noiseMap = new float[mapWidth, mapHeight];

		if (noiseScale <= 0)
			noiseScale = 0.00001f;
		

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				
				float sampleX = x / noiseScale;
				float sampleY = y / noiseScale;

				float perlinValue = Mathf.PerlinNoise (sampleX, sampleY);
				noiseMap [x, y] = perlinValue;
			}
		}

		return noiseMap;

	}

	public static float[,] GenerateNoiseMap (int mapWidth, int mapHeight, float noiseScale, int octaves, float persistance, float lacunarity) {
		float[,] noiseMap = new float[mapWidth, mapHeight];

		if (noiseScale <= 0)
			noiseScale = 0.00001f;

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {
					float sampleX = x / noiseScale * frequency;
					float sampleY = y / noiseScale * frequency;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
				

				}

				if (noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;

				} else if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}

				noiseMap [x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
			}
		}

		return noiseMap;
	}

	public static float[,] GenerateNoiseMap (int mapWidth, int mapHeight, int seed, float noiseScale, int octaves, float persistance, float lacunarity, Vector2 offset) {
		float[,] noiseMap = new float[mapWidth, mapHeight];

		System.Random prng = new System.Random (seed);

		Vector2[] octaveOffsets = new Vector2[octaves];

		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next (-100000, 100000) + offset.x;
			float offsetY = prng.Next (-100000, 100000) + offset.y;

			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
		}

		if (noiseScale <= 0)
			noiseScale = 0.00001f;

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {
					float sampleX = (x - halfWidth) / noiseScale * frequency + octaveOffsets[i].x;
					float sampleY = (y - halfHeight) / noiseScale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;


				}

				if (noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;

				} else if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}

				noiseMap [x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
			}
		}

		return noiseMap;
	}


}
