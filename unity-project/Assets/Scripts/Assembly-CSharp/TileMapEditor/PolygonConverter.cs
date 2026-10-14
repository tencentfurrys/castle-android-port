using System;
using System.Collections.Generic;
using ClipperLib;
using UnityEngine;

namespace TileMapEditor
{
	public class PolygonConverter : MonoBehaviour
	{
		private GameObject colliderObj;

		public List<List<Vector2>> UniteCollisionPolygons(List<List<Vector2>> polygons, Transform spawnPoint, int coll)
		{
			List<List<Vector2>> list = new List<List<Vector2>>();
			Clipper clipper = new Clipper();
			string text = spawnPoint.gameObject.name + "Collider";
			if (spawnPoint.FindChild(text) != null)
			{
				colliderObj = spawnPoint.FindChild(text).gameObject;
				MonoBehaviour.print(colliderObj.name);
				UnityEngine.Object.DestroyImmediate(colliderObj.GetComponent<PolygonCollider2D>());
			}
			else
			{
				colliderObj = new GameObject(text);
			}
			int num = 100;
			foreach (Transform item in spawnPoint)
			{
				if (item.GetComponent<SpriteRenderer>() != null)
				{
					Sprite sprite = item.GetComponent<SpriteRenderer>().sprite;
					num = (int)(sprite.rect.width / sprite.bounds.size.x);
					MonoBehaviour.print(num);
					break;
				}
			}
			colliderObj.transform.SetParent(spawnPoint);
			colliderObj.transform.position = spawnPoint.transform.position;
			colliderObj.layer = coll;
			int num2 = 10000;
			for (int i = 0; i < polygons.Count; i++)
			{
				List<IntPoint> list2 = new List<IntPoint>(polygons[i].Count);
				for (int j = 0; j < polygons[i].Count; j++)
				{
					list2.Add(new IntPoint(Mathf.Floor(polygons[i][j].x * (float)num2), Mathf.Floor(polygons[i][j].y * (float)num2)));
				}
				clipper.AddPath(list2, PolyType.ptSubject, true);
			}
			List<List<IntPoint>> solution = new List<List<IntPoint>>();
			clipper.Execute(ClipType.ctUnion, solution);
			ClipperOffset clipperOffset = new ClipperOffset();
			clipperOffset.AddPaths(solution, JoinType.jtMiter, EndType.etClosedPolygon);
			clipperOffset.Execute(ref solution, 5.0);
			foreach (List<IntPoint> item2 in solution)
			{
				List<Vector2> list3 = new List<Vector2>();
				foreach (IntPoint item3 in item2)
				{
					list3.Add(new Vector2((float)item3.X / (float)num2, (float)item3.Y / (float)num2));
				}
				list.Add(list3);
			}
			return RemoveClosePointsInPolygons(list);
		}

		public void CreateLevelCollider(List<List<Vector2>> polygons)
		{
			PolygonCollider2D polygonCollider2D = colliderObj.AddComponent<PolygonCollider2D>();
			polygonCollider2D.pathCount = polygons.Count;
			for (int i = 0; i < polygons.Count; i++)
			{
				Vector2[] points = polygons[i].ToArray();
				polygonCollider2D.SetPath(i, points);
			}
		}

		public List<List<Vector2>> RemoveClosePointsInPolygons(List<List<Vector2>> polygons)
		{
			float num = 0.1f;
			List<List<Vector2>> list = new List<List<Vector2>>();
			foreach (List<Vector2> polygon in polygons)
			{
				List<Vector2> list2 = polygon;
				List<Vector2> pointsToRemove = new List<Vector2>();
				foreach (Vector2 item in list2)
				{
					foreach (Vector2 item2 in polygon)
					{
						if (!(item2 == item) && !pointsToRemove.Contains(item2))
						{
							bool flag = Math.Abs(item2.x - item.x) < num;
							bool flag2 = Math.Abs(item2.y - item.y) < num;
							if (flag && flag2)
							{
								pointsToRemove.Add(item);
								break;
							}
						}
					}
				}
				polygon.RemoveAll((Vector2 x) => pointsToRemove.Contains(x));
				if (polygon.Count > 0)
				{
					list.Add(polygon);
				}
			}
			return list;
		}
	}
}
