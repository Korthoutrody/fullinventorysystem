using UnityEngine;
using System.Collections.Generic;

public class BuildablesController : MonoBehaviour {

	public GameObject controller;
	InventoryController ic;
	BuildablesList bl;
	public Transform buildLocation;
	public GameObject player;

	private int selector;
	private int categoryPos;
	private int categoryAmount;
	private int categoryChilds;

    void Start () {
		ic = controller.GetComponent<InventoryController>();
		bl = controller.GetComponent<BuildablesList>();

		//Starters
		selector = 1; categoryPos = 1;
		GetCategoryAmount();
		GetCategoryChilds();
	}

	void Update()
	{
		//Starting and ending build mode
		if (Input.GetKeyDown(ic.selectBuild))
		{
			if (buildLocation.childCount > 0)
			{
				Destroy(buildLocation.GetChild(0).gameObject);
			}

			else if (buildLocation.childCount == 0)
			{
				ShowBuildable();
			}
		}

		//Mouse Options
		if (ic.isBuilding)
		{
			MouseScrolling();

			//Category selecting by Middle Mouse Button
			if (Input.GetKeyDown(KeyCode.Mouse2))
			{
				ChangeCategory();
			}
		
		}
	}

	//Selecting buildables with the mouse scrollwheel
	public void MouseScrolling()
	{
		//Scrolling upwards
		if (Input.GetAxis("Mouse ScrollWheel") > 0f && categoryChilds > 1) // forward
		{
			//Handeling upwards scrolling
			if (selector < categoryChilds)
			{
				selector += 1;
				ShowBuildable();
				Debug.Log(selector + "Up");
			}

			//Handeling upwards scrolling to flip
			else if (selector == categoryChilds)
			{
				selector = 1;
				ShowBuildable();
				Debug.Log(selector + "Flip");
			}
		}

		//Scrolling downards
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f && categoryChilds > 1) // backwards
		{

			//Handeling downwards scrolling
			if (selector > 1)
			{
				selector -= 1;
				ShowBuildable();
				Debug.Log(selector + "Down");
			}

			//Handeling downwards scrolling to flip
			else if (selector == 1)
			{
				selector = categoryChilds;
				ShowBuildable();
				Debug.Log(selector + "Flip");
			}
		}
	}

	//Showing the buildable that can be placed
	private void ShowBuildable()
	{
		for (int i = 0; i < bl.buildables.Count; i++)
		{
			if (bl.buildables[i].category == categoryPos)
			{
				if (buildLocation.childCount > 0)
				{
					Destroy(buildLocation.GetChild(0).gameObject);
				}

				GameObject buildable = Instantiate(bl.buildables[  1].model, buildLocation.position, Quaternion.identity);
				buildable.name = bl.buildables[selector - 1].name;
				buildable.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0f);
				buildable.transform.SetParent(buildLocation);
				Debug.Log("Showing Buildable: " + bl.buildables[selector - 1].name + " " + (selector - 1));
				break;


			}
		}
	}

	//Changing the categories by upping the integer
	private void ChangeCategory()
	{
		if (categoryPos == categoryAmount)
		{
			categoryPos = 1;
			ShowBuildable();
			GetCategoryChilds();

		}

		else if (categoryPos < categoryAmount)
		{
			categoryPos += 1;
			ShowBuildable();
			GetCategoryChilds();
		}
	}

	//Get the amount of buildables in the selected category
	private void GetCategoryChilds()
	{
		for (int i = 0; i < category.Count; i++)
		{
			if (categoryPos == category[i].id)
			{
				categoryChilds = category[i].amount;
				Debug.Log("Moved to category: " + category[i].id + " " + category[i].name + " " + categoryChilds);
				break;
			}
		}
	}

	//Get the amount of overall categories that exist
	private void GetCategoryAmount()
	{
		categoryAmount = category.Count;
	}
			

	//The categories
	[System.Serializable]
	public class Category
	{
		public int id;
		public string name;
		public int amount;
	}

	public List<Category> category = new List<Category>();
}


