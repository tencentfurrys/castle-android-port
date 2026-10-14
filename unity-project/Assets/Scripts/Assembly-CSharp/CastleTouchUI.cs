using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// CastleTouchUI: runtime-built touch overlay (no scene edits).
// Android only. Left diamond d-pad (arrows), right A/B buttons (Z confirm / X
// cancel), small START (Space). Feeds CastleTouch, which the patched PlayMaker
// input actions OR with real input. Phone BACK button already sends Escape.
public class CastleTouchUI : MonoBehaviour
{
	private const string A_KEY = "Z";
	private const string B_KEY = "X";
	private const string START_KEY = "Space";

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		GameObject go = new GameObject("CastleTouch");
		Object.DontDestroyOnLoad(go);
		go.AddComponent<CastleTouchUI>();
	}

	private void Start()
	{
		BuildUI();
	}

	private void LateUpdate()
	{
		CastleTouch.RefreshAxes();
	}

	private void BuildUI()
	{
		GameObject canvasGo = new GameObject("CastleTouchCanvas");
		canvasGo.transform.SetParent(base.transform);
		Canvas canvas = canvasGo.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = 9999;
		CanvasScaler scaler = canvasGo.AddComponent<CanvasScaler>();
		scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		scaler.referenceResolution = new Vector2(1280f, 720f);
		canvasGo.AddComponent<GraphicRaycaster>();
		if (Object.FindObjectOfType<EventSystem>() == null)
		{
			GameObject es = new GameObject("CastleTouchEventSystem");
			es.transform.SetParent(base.transform);
			es.AddComponent<EventSystem>();
			es.AddComponent<StandaloneInputModule>();
		}
		// Left d-pad.
		MakeHoldButton(canvasGo.transform, "Up", new Vector2(-560f, 120f), "UpArrow");
		MakeHoldButton(canvasGo.transform, "Down", new Vector2(-560f, -120f), "DownArrow");
		MakeHoldButton(canvasGo.transform, "Left", new Vector2(-680f, 0f), "LeftArrow");
		MakeHoldButton(canvasGo.transform, "Right", new Vector2(-440f, 0f), "RightArrow");
		// Right action buttons.
		MakeHoldButton(canvasGo.transform, "A", new Vector2(560f, 0f), A_KEY);
		MakeHoldButton(canvasGo.transform, "B", new Vector2(440f, -100f), B_KEY);
		MakeHoldButton(canvasGo.transform, "ST", new Vector2(560f, 140f), START_KEY);
	}

	private void MakeHoldButton(Transform parent, string label, Vector2 anchoredPos, string keyName)
	{
		GameObject go = new GameObject("CT_" + label);
		go.transform.SetParent(parent);
		RectTransform rt = go.AddComponent<RectTransform>();
		rt.anchorMin = new Vector2(0.5f, 0.5f);
		rt.anchorMax = new Vector2(0.5f, 0.5f);
		rt.pivot = new Vector2(0.5f, 0.5f);
		rt.sizeDelta = new Vector2(110f, 110f);
		rt.anchoredPosition = anchoredPos;
		Image img = go.AddComponent<Image>();
		img.color = new Color(1f, 1f, 1f, 0.28f);
		GameObject textGo = new GameObject("Label");
		textGo.transform.SetParent(go.transform);
		RectTransform trt = textGo.AddComponent<RectTransform>();
		trt.anchorMin = Vector2.zero;
		trt.anchorMax = Vector2.one;
		trt.pivot = new Vector2(0.5f, 0.5f);
		trt.offsetMin = Vector2.zero;
		trt.offsetMax = Vector2.zero;
		Text text = textGo.AddComponent<Text>();
		text.text = label;
		text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		text.fontSize = 40;
		text.alignment = TextAnchor.MiddleCenter;
		text.color = new Color(1f, 1f, 1f, 0.85f);
		CastleTouchButton btn = go.AddComponent<CastleTouchButton>();
		btn.keyName = keyName;
	}
}

// Per-button press/release forwarder (multi-touch safe: each button tracks itself).
public class CastleTouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	public string keyName;

	public void OnPointerDown(PointerEventData eventData)
	{
		CastleTouch.Press(keyName);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		CastleTouch.Release(keyName);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		CastleTouch.Release(keyName);
	}
}
