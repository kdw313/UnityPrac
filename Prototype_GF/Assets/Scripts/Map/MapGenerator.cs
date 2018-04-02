using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

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
	public const int mapChunkSize = 241;

	// 1 for no simplification, < 1 to simplification
	[Range(1,6)]
	public int editorPreviewLOD;

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

	Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
	Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();


	public void DrawMapInEditor(){
		MapData mapData = GenerateMapData (Vector2.zero);

		// drawing
		MapDisplay display = FindObjectOfType<MapDisplay> ();

		if (drawMode == DrawMode.NoiseMap)
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (mapData.heightMap));
		else if (drawMode == DrawMode.ColorMap)
			display.DrawTexture (TextureGenerator.TextureFromColorMap (mapData.colorMap, mapChunkSize, mapChunkSize));
		else if (drawMode == DrawMode.MeshMap)
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh (mapData.heightMap, meshHeightMultiplier, meshHeightCurve, editorPreviewLOD), TextureGenerator.TextureFromColorMap (mapData.colorMap, mapChunkSize, mapChunkSize));
	}


	public void RequestMapData(Vector2 centre, Action<MapData> callback){
		ThreadStart threadStart = delegate {
			MapDataThread (centre, callback);
		};

		new Thread(threadStart).Start();
	}


	void MapDataThread(Vector2 centre, Action<MapData> callback){
		MapData mapData = GenerateMapData (centre);

		// no other thread can executed once another other already is using
		lock (mapDataThreadInfoQueue) {
			mapDataThreadInfoQueue.Enqueue (new MapThreadInfo<MapData> (callback, mapData));
		}
	}

	public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback){
		ThreadStart threadStart = delegate {
			MeshDataThread (mapData, lod, callback);
		};

		new Thread(threadStart).Start();
	}

	void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback){
		MeshData meshData = MeshGenerator.GenerateTerrainMesh (mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod);

		lock (meshDataThreadInfoQueue) {
			meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData> (callback, meshData));
		}
	}


	void Update() {
		if (mapDataThreadInfoQueue.Count > 0){
			for(int i = 0; i < mapDataThreadInfoQueue.Count; i++) {
				MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue ();
				threadInfo.callback (threadInfo.parameter);
			}
		}

		if (meshDataThreadInfoQueue.Count > 0){
			for(int i = 0; i < meshDataThreadInfoQueue.Count; i++) {
				MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue ();
				threadInfo.callback (threadInfo.parameter);
			}
		}
	}
			

	MapData GenerateMapData(Vector2 centre){
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, centre + offset);

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

		return new MapData (noiseMap, colorMap);
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
		
	struct MapThreadInfo<T>{

		// make the data immutable
		public readonly Action<T> callback;
		public readonly T parameter;

		public MapThreadInfo (Action<T> callback, T parameter)
		{
			this.callback = callback;
			this.parameter = parameter;
		}
		
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color color;
}


public struct MapData {

	public readonly float[,] heightMap;
	public readonly Color[] colorMap;

	public MapData (float[,] heightMap, Color[] colorMap)
	{
		this.heightMap = heightMap;
		this.colorMap = colorMap;
	}
	
}
