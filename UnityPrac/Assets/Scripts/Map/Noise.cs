using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// noise class
public static class Noise {

	// Global estimating global min max
	public enum NormalizeMode { Local, Global };

	public static int MIN_OCTAVE_OFFSET = 100000;
	public static int MAX_OCTAVE_OFFSET = -100000;

	//noise map 1
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

	//noise map 2
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

	//noise map 3
	public static float[,] GenerateNoiseMap (int mapWidth, int mapHeight, int seed,
											float noiseScale, int octaves, float persistance,
											float lacunarity, Vector2 offset,
											NormalizeMode normalizeMode) {

		float[,] noiseMap = new float[mapWidth, mapHeight];

		System.Random prng = new System.Random (seed);

		Vector2[] octaveOffsets = new Vector2[octaves];

		float maxPossibleHeight = 0;
		float amplitude = 1;
		float frequency = 1;

		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next (-100000, 100000) + offset.x;
			float offsetY = prng.Next (-100000, 100000) - offset.y;

			octaveOffsets [i] = new Vector2 (offsetX, offsetY);

			maxPossibleHeight += amplitude;
			amplitude *= persistance;
		}

		if (noiseScale <= 0)
			noiseScale = 0.00001f;

		float maxLocalNoiseHeight = float.MinValue;
		float minLocalNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

				amplitude = 1;
				frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {
					float sampleX = (x - halfWidth + octaveOffsets[i].x) / noiseScale * frequency;
					float sampleY = (y - halfHeight + octaveOffsets[i].y) / noiseScale * frequency;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
				}

				if (noiseHeight > maxLocalNoiseHeight) {
					maxLocalNoiseHeight = noiseHeight;
				} else if (noiseHeight < minLocalNoiseHeight) {
					minLocalNoiseHeight = noiseHeight;
				}

				noiseMap [x, y] = noiseHeight;
			}
		}

		// normalizing noise map
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				if (normalizeMode == NormalizeMode.Local) {
					noiseMap [x, y] = Mathf.InverseLerp (minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap [x, y]);
				} else {

					float normailzedHeight = (noiseMap [x, y] + 1) / maxPossibleHeight;
					noiseMap [x, y] = Mathf.Clamp(normailzedHeight, 0, int.MaxValue);
				}
			}
		}

		return noiseMap;
	}


}
