using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	[Header("References")]
	public GameObject ui;
	public GameObject controller;
	[Header("Keys")]
	public KeyCode accessInven;

	void Start () {
		ui.SetActive(false);

		//Show the first category
		controller = GameObject.Find("controller");
		controller.GetComponent<CraftingController>().DestroyPreviousCategory("Weaponry");

	}

	void Update () {

		//Accessing Inventory
		if (Input.GetKeyDown(accessInven) && ui.activeInHierarchy == false)
		{
			ui.SetActive(true);
		}

		//Unaccessing Inventory
		else if (Input.GetKeyDown(accessInven) && ui.activeInHierarchy == true)
		{
			if (GameObject.Find("ItemMenu"))
			{
				Destroy(GameObject.Find("ItemMenu"));
			}

			ui.SetActive(false);
		}

	}
}
