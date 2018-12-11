using UnityEngine;
using UnityEngine.UI;

public class ItemEntity : MonoBehaviour {

	public int amount;
	public GameObject controller;
	ItemController ic;

	#region Item Variables from ItemsController For Entities
	//----< THE ITEMS DB <-------
	//Objects
	public Image icon;

	//Stackables
	public bool isStackable;
	public int maxStack;
	//----< THE ITEMS DB <-------
	#endregion

	void Start()
	{
		//Script references
		controller = GameObject.Find("controller");
		ic = controller.GetComponent<ItemController>();

		for (int i = 0; i < ic.items.Count; i++)
		{
			//Checking through the whole of the items list for relevant variables
			if (ic.items[i].name == name)
			{
				//Base information
				icon = ic.items[i].icon;

				//Stack information
				if (ic.items[i].isStackable)
				{
					isStackable = ic.items[i].isStackable;
					maxStack = ic.items[i].maxStack;
				}
			}
		}
	}

}
