using UnityEngine;

namespace TileMapEditor
{
	[ExecuteInEditMode]
	public class SnapToGrid : MonoBehaviour
	{
		public float cell_size = 1f;

		private float x;

		private float y;

		private float z;

		[Tooltip("Defines if this function should be applyed or not")]
		public bool lockedOnGrid = true;

		private void Start()
		{
			x = 0f;
			y = 0f;
			z = 0f;
		}

		private void Update()
		{
			if (lockedOnGrid && !Application.isPlaying)
			{
				x = Mathf.Round(base.transform.position.x / cell_size) * cell_size;
				y = Mathf.Round(base.transform.position.y / cell_size) * cell_size;
				z = base.transform.position.z;
				base.transform.position = new Vector3(x, y, z);
			}
		}
	}
}
