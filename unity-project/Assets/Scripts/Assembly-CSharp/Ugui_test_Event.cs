using UnityEngine;
using UnityEngine.EventSystems;

public class Ugui_test_Event : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerClickHandler, IPointerExitHandler
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("Enter ###.");
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Click ###.");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log("Exit ###.");
	}
}
