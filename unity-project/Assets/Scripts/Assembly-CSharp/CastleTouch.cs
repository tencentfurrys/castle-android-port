using System.Collections.Generic;
using UnityEngine;

// CastleTouch: virtual key/axis state fed by the on-screen overlay (CastleTouchUI).
// Patched PlayMaker input actions OR this state with real Unity input, so the
// keyboard-driven FSM menus and gameplay work with touch on Android.
// Key names match KeyCode.ToString() ("UpArrow", "Z", "Space", ...) which is also
// what the FSM Key_* string variables resolve to.
public static class CastleTouch
{
	private static Dictionary<string, bool> held = new Dictionary<string, bool>();
	private static Dictionary<string, int> downFrame = new Dictionary<string, int>();
	private static Dictionary<string, int> upFrame = new Dictionary<string, int>();
	private static Dictionary<string, float> axes = new Dictionary<string, float>();

	public static void Press(string name)
	{
		held[name] = true;
		downFrame[name] = Time.frameCount;
		CastleDiag.NoteKey(name);
	}

	public static void Release(string name)
	{
		held[name] = false;
		upFrame[name] = Time.frameCount;
	}

	public static bool Down(string name)
	{
		int f;
		return downFrame.TryGetValue(name, out f) && f == Time.frameCount;
	}

	public static bool Down(KeyCode key)
	{
		return Down(key.ToString());
	}

	public static bool Up(string name)
	{
		int f;
		return upFrame.TryGetValue(name, out f) && f == Time.frameCount;
	}

	public static bool Up(KeyCode key)
	{
		return Up(key.ToString());
	}

	public static bool Held(string name)
	{
		bool h;
		return held.TryGetValue(name, out h) && h;
	}

	public static bool Held(KeyCode key)
	{
		return Held(key.ToString());
	}

	public static void SetAxis(string name, float value)
	{
		axes[name] = value;
	}

	public static float Axis(string name)
	{
		float v;
		return axes.TryGetValue(name, out v) ? v : 0f;
	}

	// Called by the d-pad: keeps Horizontal/Vertical axes in sync with arrow holds.
	public static void RefreshAxes()
	{
		float h = 0f;
		if (Held("LeftArrow")) h -= 1f;
		if (Held("RightArrow")) h += 1f;
		float v = 0f;
		if (Held("DownArrow")) v -= 1f;
		if (Held("UpArrow")) v += 1f;
		axes["Horizontal"] = h;
		axes["Vertical"] = v;
	}
}
