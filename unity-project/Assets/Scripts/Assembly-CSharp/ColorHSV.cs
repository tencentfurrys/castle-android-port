using UnityEngine;

public struct ColorHSV
{
	private float _h;

	private float _s;

	private float _v;

	private float _a;

	public float h
	{
		get
		{
			return _h;
		}
		set
		{
			_h = value % 360f;
		}
	}

	public float s
	{
		get
		{
			return _s;
		}
		set
		{
			if (value < 0f)
			{
				_s = 0f;
			}
			else if (value > 100f)
			{
				_s = 100f;
			}
			else
			{
				_s = value;
			}
		}
	}

	public float v
	{
		get
		{
			return _v;
		}
		set
		{
			if (value < 0f)
			{
				_v = 0f;
			}
			else if (value > 100f)
			{
				_v = 100f;
			}
			else
			{
				_v = value;
			}
		}
	}

	public float a
	{
		get
		{
			return _a;
		}
		set
		{
			if (value < 0f)
			{
				_a = 0f;
			}
			else if (value > 100f)
			{
				_a = 100f;
			}
			else
			{
				_a = value;
			}
		}
	}

	public ColorHSV(float h, float s, float v)
	{
		this.h = h;
		this.s = s;
		this.v = v;
		a = 100f;
	}

	public ColorHSV(float h, float s, float v, float a)
	{
		this.h = h;
		this.s = s;
		this.v = v;
		this.a = a;
	}

	public ColorHSV(Color color)
	{
		_h = 0f;
		_s = 0f;
		_v = 0f;
		_a = 0f;
		Color32 colorHSV = color;
		SetColorHSV(colorHSV);
	}

	public ColorHSV(Color32 color)
	{
		_h = 0f;
		_s = 0f;
		_v = 0f;
		_a = 0f;
		SetColorHSV(color);
	}

	private void SetColorHSV(Color32 color)
	{
		float num = 100f / 255f;
		_a = (float)(int)color.a * num;
		_h = 0f;
		float num2 = Mathf.Min(Mathf.Min(color.r, color.g), color.b);
		float num3 = Mathf.Max(Mathf.Max(color.r, color.g), color.b);
		float num4 = num3 - num2;
		_v = num3;
		if (num3 != 0f)
		{
			_s = 255f * num4 / num3;
		}
		else
		{
			_s = 0f;
		}
		if (_s != 0f)
		{
			if ((float)(int)color.r == num3)
			{
				_h = (float)(color.g - color.b) / num4;
			}
			else if ((float)(int)color.g == num3)
			{
				_h = 2f + (float)(color.b - color.r) / num4;
			}
			else if ((float)(int)color.b == num3)
			{
				_h = 4f + (float)(color.r - color.g) / num4;
			}
		}
		else
		{
			_h = -1f;
		}
		_h *= 60f;
		if (_h < 0f)
		{
			_h += 360f;
		}
		_s *= num;
		_v *= num;
	}

	public Color ToColor()
	{
		return ToColor32();
	}

	public Color32 ToColor32()
	{
		float num = _s * 0.01f;
		float num2 = _v * 2.55f;
		int num3 = (int)Mathf.Floor(_h / 60f) % 6;
		float num4 = _h / 60f - Mathf.Floor(_h / 60f);
		byte b = (byte)Mathf.Round(num2);
		byte b2 = (byte)Mathf.Round(num2 * (1f - num));
		byte b3 = (byte)Mathf.Round(num2 * (1f - num4 * num));
		byte b4 = (byte)Mathf.Round(num2 * (1f - (1f - num4) * num));
		byte b5 = (byte)Mathf.Round(_a * 2.55f);
		switch (num3)
		{
		case 0:
			return new Color32(b, b4, b2, b5);
		case 1:
			return new Color32(b3, b, b2, b5);
		case 2:
			return new Color32(b2, b, b4, b5);
		case 3:
			return new Color32(b2, b3, b, b5);
		case 4:
			return new Color32(b4, b2, b, b5);
		default:
			return new Color32(b, b2, b3, b5);
		}
	}

	public override string ToString()
	{
		return "HSVA(" + h.ToString("f2") + ", " + s.ToString("f2") + ", " + v.ToString("f2") + ", " + a.ToString("f2") + ")";
	}
}
