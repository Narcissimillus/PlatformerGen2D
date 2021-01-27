using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Tilemaps;
using System.Runtime.InteropServices;
using System.Collections.Specialized;

public class GenerateMap : MonoBehaviour
{
	[Tooltip("The Tilemap to draw onto")]
	public Tilemap tilemap;
	public Tilemap gemTilemap;
	public Tilemap envTilemap;
	[Tooltip("The Tile to draw (use a RuleTile for best results)")]
	public TileBase tile;
	public TileBase gemTile;
	public TileBase envRandTile;
	public int width, minHeight = 0, minWidth = 0;
	public float heightVal, smoothness;
	public GameObject ken;
	public GameObject dragon;
	public int enemySpawnChanceProcentage = 3;
	float seed;

	// Start is called before the first frame update
	void Start()
	{
		seed = UnityEngine.Random.Range(-1000, 1000);
		Generation();
		tilemap.transform.Translate(0, -heightVal/2, 0);
		envTilemap.transform.Translate(0, -heightVal / 2 - 0.05f, 0);
	}

	// Update is called once per frame
	void Update()
	{

	}

	void Generation()
	{
		for (int x = 0; x < minWidth; x++)
        {
			for (int y = 0; y <= minHeight; y++)
			{
				tilemap.SetTile(new Vector3Int(x, y, 0), tile);
			}
		}
		for (int x = minWidth; x < width; x++)
		{
			int height = Mathf.RoundToInt(heightVal * Mathf.PerlinNoise(x / smoothness, seed));
			for (int y = 0; y < minHeight; y++)
            {
				tilemap.SetTile(new Vector3Int(x, y, 0), tile);
			}
			if (minHeight <= height)
			{
				for (int y = minHeight; y < height; y++)
				{
					tilemap.SetTile(new Vector3Int(x, y, 0), tile);
				}
			}
			int randVal = UnityEngine.Random.Range(0, 10);
            if ((randVal == 5 || randVal == 6) && tilemap.GetTile(new Vector3Int(x, height, 0)) == null)
            {
				int addHeight = UnityEngine.Random.Range(0, 3);
                gemTilemap.SetTile(new Vector3Int(x, height + addHeight, 0), gemTile);
            }
            if (randVal < 5 && tilemap.GetTile(new Vector3Int(x, height, 0)) == null)
			{
				envTilemap.SetTile(new Vector3Int(x, height, 0), envRandTile);
			}
			randVal = UnityEngine.Random.Range(1, 100);
			if (randVal <= enemySpawnChanceProcentage && x % 5 == 0 && tilemap.GetTile(new Vector3Int(x, height, 0)) == null )
            {
				GameObject kenClone = Instantiate(ken, new Vector3(x, height + 1, 0), Quaternion.identity);
				kenClone.SetActive(true);
			}
            else
            {
				randVal = UnityEngine.Random.Range(1, 400);
				if (randVal <= enemySpawnChanceProcentage && x % 3 == 0 && tilemap.GetTile(new Vector3Int(x, height, 0)) == null)
				{
					GameObject dragonClone = Instantiate(dragon, new Vector3(x, height + 1, 0), Quaternion.identity);
					dragonClone.SetActive(true);
				}
			}
		}
	}
}