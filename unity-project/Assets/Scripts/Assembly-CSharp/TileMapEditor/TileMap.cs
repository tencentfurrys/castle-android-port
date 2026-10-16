using System.Collections.Generic;
using UnityEngine;

namespace TileMapEditor
{
	public class TileMap : MonoBehaviour
	{
		public Texture2D texture2D;

		public Vector2 mapSize = new Vector2(20f, 10f);

		public Vector2 tileSize = default(Vector2);

		public Vector2 tilePadding = default(Vector2);

		public Vector2 gridSize = default(Vector2);

		public Object[] spriteReferences;

		public int pixelsToUnits = 100;

		public int tileID;

		public int spriteLayer;

		public int collisionLayer;

		public GameObject tileContainer;

		public string mapName = "Tilemap";

		public bool randomTile;

		public List<Tile> tiles;

		public Sprite currentTileBrush
		{
			get
			{
				return spriteReferences[tileID] as Sprite;
			}
		}

		private void OnDrawGizmosSelected()
		{
			Vector3 position = base.transform.position;
			if (!(texture2D != null))
			{
				return;
			}
			Gizmos.color = Color.gray;
			int num = 0;
			float x = mapSize.x;
			float num2 = mapSize.x * mapSize.y;
			Vector3 size = new Vector3(tileSize.x / (float)pixelsToUnits, tileSize.y / (float)pixelsToUnits);
			Vector2 vector = new Vector2(size.x / 2f, size.y / 2f);
			for (int i = 0; (float)i < num2; i++)
			{
				float num3 = (float)i % x;
				float x2 = num3 * size.x + vector.x + position.x;
				float y = 0f - (float)num * size.y - vector.y + position.y;
				Gizmos.DrawWireCube(new Vector2(x2, y), size);
				if (num3 == x - 1f)
				{
					num++;
				}
			}
			Gizmos.color = Color.white;
			float x3 = position.x + gridSize.x / 2f;
			float y2 = position.y - gridSize.y / 2f;
			Gizmos.DrawWireCube(new Vector2(x3, y2), gridSize);
		}
	}
}
