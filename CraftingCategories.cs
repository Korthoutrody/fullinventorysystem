using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftingCategories : MonoBehaviour {

	public Button button;
	public GameObject controller;

	public void OnClicked(Button button)
	{
		controller.GetComponent<CraftingController>().DestroyPreviousCategory(button.name);
	}
}
