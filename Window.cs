using UnityEngine;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour, IPointerClickHandler {

	PointerEventData pointerEventData;

	//Detect current clicks on the GameObject (the one with the script attached)
	public void OnPointerClick(PointerEventData eventData)
	{
		pointerEventData = eventData as PointerEventData;

		if (pointerEventData.button == PointerEventData.InputButton.Right && GameObject.Find("RightClickMenu")
			|| pointerEventData.button == PointerEventData.InputButton.Left && GameObject.Find("RightClickMenu"))
		{
			Destroy(GameObject.Find("RightClickMenu"));
		}

	}
}

