using UnityEngine;

public static class CastleSavePath
{
	public static string Abs(string f)
	{
		if (System.IO.Path.IsPathRooted(f))
		{
			return f;
		}
		return System.IO.Path.Combine(Application.persistentDataPath, f);
	}
}
