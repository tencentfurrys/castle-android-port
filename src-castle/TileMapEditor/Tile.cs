using System;
using UnityEngine;

namespace TileMapEditor;

[Serializable]
public class Tile
{
	public Vector2 position;

	public GameObject tile;

	public Tile(Vector2 pos, GameObject til)
	{
		position = pos;
		tile = til;
	}

	public void Update(Vector2 pos, GameObject til)
	{
		position = pos;
		tile = til;
	}
}
