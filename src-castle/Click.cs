using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Click : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	public UnityEvent PointerDown;

	public UnityEvent PointerUp;

	public UnityEvent PointerEnter;

	public UnityEvent PointerExit;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (PointerDown != null)
		{
			PointerDown.Invoke();
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (PointerUp != null)
		{
			PointerUp.Invoke();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (PointerEnter != null)
		{
			PointerEnter.Invoke();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (PointerExit != null)
		{
			PointerExit.Invoke();
		}
	}
}
