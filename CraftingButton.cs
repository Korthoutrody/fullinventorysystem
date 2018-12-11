using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public Button button;
	public GameObject controller;

	public void OnClicked(Button button)
	{
		controller = GameObject.Find("controller");
		controller.GetComponent<CraftingController>().CheckRecipe(button.name);
	}

	#region Tooltips With Pointers
	public void OnPointerEnter(PointerEventData eventData)
	{
		controller = GameObject.Find("controller");
		controller.GetComponent<TooltipController>().ShowTooltip(gameObject);
		
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		controller = GameObject.Find("controller");
		controller.GetComponent<TooltipController>().HideTooltip();
		
	}
	#endregion
}
