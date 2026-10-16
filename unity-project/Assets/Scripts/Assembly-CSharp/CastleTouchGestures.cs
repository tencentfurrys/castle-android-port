using System.Collections.Generic;
using UnityEngine;

// CastleTouchGestures: real touch support, no buttons.
//   1-finger tap ......... Z (confirm / talk / advance)
//   1-finger swipe+hold .. arrow in swipe direction (held until lift: walk / menu move)
//   2-finger tap ......... X (cancel)
//   3-finger tap ......... Space (start / skip)
//   Phone BACK button .... Escape (native, untouched)
// Feeds CastleTouch, which the patched PlayMaker input actions OR with real keys.
// In the editor, the mouse mirrors one finger so touch is testable on PC.
public class CastleTouchGestures : MonoBehaviour
{
	private class Finger
	{
		public Vector2 start;
		public float beginTime;
		public bool dirPressed;
		public string dirName = "";
	}

	private Dictionary<int, Finger> fingers = new Dictionary<int, Finger>();
	private List<PendingRelease> pending = new List<PendingRelease>();
	private bool twoFingerTap;

	private struct PendingRelease
	{
		public string name;
		public int frame;
	}

	private const float TAP_MAX_TIME = 0.35f;
	private const float SWIPE_MIN_DIST = 60f;

	private float scaledDist(float px)
	{
		return px * 720f / (float)Screen.height;
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init()
	{
		if (Application.platform != RuntimePlatform.Android && !Application.isEditor)
		{
			return;
		}
		GameObject go = new GameObject("CastleTouchGestures");
		Object.DontDestroyOnLoad(go);
		go.AddComponent<CastleTouchGestures>();
	}

	private void Update()
	{
		int now = Time.frameCount;
		for (int i = pending.Count - 1; i >= 0; i--)
		{
			if (pending[i].frame <= now)
			{
				CastleTouch.Release(pending[i].name);
				pending.RemoveAt(i);
			}
		}
#if UNITY_EDITOR
		EditorMouse();
#else
		if (Application.platform == RuntimePlatform.Android)
		{
			foreach (Touch t in Input.touches)
			{
				HandleTouch(t.fingerId, t.position, t.phase);
			}
			if (Input.touchCount == 0 && fingers.Count > 0)
			{
				fingers.Clear();
			}
		}
#endif
	}

	private void TapPress(string name)
	{
		CastleTouch.Press(name);
		pending.Add(new PendingRelease { name = name, frame = Time.frameCount + 2 });
	}

	private void HandleTouch(int id, Vector2 pos, TouchPhase phase)
	{
		if (phase == TouchPhase.Began)
		{
			Finger f = new Finger();
			f.start = pos;
			f.beginTime = Time.realtimeSinceStartup;
			f.dirPressed = false;
			fingers[id] = f;
			if (fingers.Count == 2)
			{
				twoFingerTap = true;
			}
			if (fingers.Count == 3)
			{
				// 3-finger tap: Space on third finger down.
				CastleTouch.Press("Space");
				pending.Add(new PendingRelease { name = "Space", frame = Time.frameCount + 2 });
				twoFingerTap = false;
				fingers.Clear();
			}
		}
		else if (phase == TouchPhase.Moved || phase == TouchPhase.Stationary)
		{
			Finger f;
			if (!fingers.TryGetValue(id, out f) || f.dirPressed)
			{
				return;
			}
			Vector2 d = pos - f.start;
			if (scaledDist(d.magnitude) >= SWIPE_MIN_DIST)
			{
				string dir = (Mathf.Abs(d.x) > Mathf.Abs(d.y))
					? (d.x > 0f ? "RightArrow" : "LeftArrow")
					: (d.y > 0f ? "UpArrow" : "DownArrow");
				f.dirPressed = true;
				f.dirName = dir;
				CastleTouch.Press(dir);
			}
		}
		else if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
		{
			Finger f;
			if (!fingers.TryGetValue(id, out f))
			{
				return;
			}
			fingers.Remove(id);
			if (f.dirPressed)
			{
				CastleTouch.Release(f.dirName);
			}
			else
			{
				float dt = Time.realtimeSinceStartup - f.beginTime;
				float dist = scaledDist((pos - f.start).magnitude);
				if (dt <= TAP_MAX_TIME && dist < SWIPE_MIN_DIST && phase == TouchPhase.Ended)
				{
					if (twoFingerTap || Input.touchCount >= 2)
					{
						TapPress("X");
					}
					else
					{
						TapPress("Z");
					}
				}
			}
			if (fingers.Count == 0)
			{
				twoFingerTap = false;
			}
		}
	}

#if UNITY_EDITOR
	private bool mouseHeld;
	private Vector2 mouseStart;
	private float mouseTime;
	private bool mouseDir;
	private string mouseDirName = "";

	private void EditorMouse()
	{
		if (Input.GetMouseButtonDown(0))
		{
			mouseHeld = true;
			mouseStart = Input.mousePosition;
			mouseTime = Time.realtimeSinceStartup;
			mouseDir = false;
		}
		else if (Input.GetMouseButton(0) && mouseHeld && !mouseDir)
		{
			Vector2 d = (Vector2)Input.mousePosition - mouseStart;
			if (scaledDist(d.magnitude) >= SWIPE_MIN_DIST)
			{
				mouseDir = true;
				mouseDirName = (Mathf.Abs(d.x) > Mathf.Abs(d.y))
					? (d.x > 0f ? "RightArrow" : "LeftArrow")
					: (d.y > 0f ? "UpArrow" : "DownArrow");
				CastleTouch.Press(mouseDirName);
			}
		}
		else if (Input.GetMouseButtonUp(0) && mouseHeld)
		{
			mouseHeld = false;
			if (mouseDir)
			{
				CastleTouch.Release(mouseDirName);
				mouseDir = false;
			}
			else if (Time.realtimeSinceStartup - mouseTime <= TAP_MAX_TIME)
			{
				TapPress("Z");
			}
		}
	}
#endif
}
