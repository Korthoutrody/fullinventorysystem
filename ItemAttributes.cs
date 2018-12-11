using UnityEngine;
using UnityEngine.UI;

public class ItemAttributes : MonoBehaviour
{
	public int amount;
	private GameObject controller;
	ItemController ic;


	#region Item Variables from ItemsController
	//----< THE ITEMS DB <-------
	[Header("General Info")]
	public string desc;

	[Header("Objects")]
	public GameObject model;
	public Image icon;
	public bool canHotbar;

	[Header("Stacking")]
	public bool isStackable;
	public int maxStack;

	[Header("Using")]
	public bool isUseable;
	public bool isDepletable;
	public float useTime;

	[Header("Swinging")]
	public bool isAction;
	public float swingRange;
	public float harvestEfficiency;
	public float killingEfficiency;
	public float swingSpeed;

	[Header("Useage Values")]
	public float thirstValue;
	public float hungerValue;

	[Header("Crafting")]
	public bool isCraftable;
	public string category;

	[Header("Equipment")]
	public bool isEquippable;
	public string equipmentType;
	//----< THE ITEMS DB <-------
	#endregion

	void Start()
	{
		controller = GameObject.Find("controller");
		ic = controller.GetComponent<ItemController>();

		for (int i = 0; i < ic.items.Count; i++)
		{
			//Checking through the whole of the items list for relevant variables
			if (ic.items[i].name == name)
			{
				//Base information
				desc = ic.items[i].desc;
				model = ic.items[i].model;
				icon = ic.items[i].icon;
				canHotbar = ic.items[i].canHotbar;

				//Stack information
				if (ic.items[i].isStackable)
				{
					isStackable = ic.items[i].isStackable;
					maxStack = ic.items[i].maxStack;
				}

				//Use information
				if (ic.items[i].isUseable)
				{
					isUseable = ic.items[i].isUseable;
					isDepletable = ic.items[i].isDepletable;
					useTime = ic.items[i].useTime;
					thirstValue = ic.items[i].thirstValue;
					hungerValue = ic.items[i].hungerValue;
				}

				//Swing information
				if (ic.items[i].isAction)
				{
					isAction = ic.items[i].isAction;
					swingRange = ic.items[i].swingRange;
					swingSpeed = ic.items[i].swingSpeed;
					harvestEfficiency = ic.items[i].harvestEfficiency;
					killingEfficiency = ic.items[i].killingEfficiency;

				}

				//Crafting information
				if (ic.items[i].isCraftable)
				{
					isCraftable = ic.items[i].isCraftable;
					category = ic.items[i].category;
				}

				//Equipment information
				if (ic.items[i].isEquippable)
				{
					isEquippable = ic.items[i].isEquippable;
					equipmentType = ic.items[i].equipmentType;
				}

			}
		}
	}
}
