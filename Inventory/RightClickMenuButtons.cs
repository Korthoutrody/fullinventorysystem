using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RightClickMenuButtons : MonoBehaviour {

	[Header("References")]
	public static Button button;					    // Template button
	public List<RightClickMenuButton> rcmb;     // The list of all relevant buttons

	InventoryController ic;

	#region Finding and Adding the Relevant Menu Buttons
	//Draw the buttons for the Right Click Menu based on certain criteria
	public void DrawRightClickMenu()
	{
		//The list to add to
		rcmb = new List<RightClickMenuButton>();

		ic = GameObject.Find("controller").GetComponent<InventoryController>();
		button = GameObject.Find("RightClickMenuButton").GetComponent<Button>();

		//Using the slot to attain the criteria for the buttons shown
		if (Slot.slot != null)
		{
			if (Slot.slot.childCount > 0)
			{
				//Check through the existing items
				for (int i = 0; i < this.gameObject.GetComponent<ItemController>().items.Count; i++)
				{
					//Check for a match in name
					if (Slot.slot.GetChild(0).gameObject.name == this.gameObject.GetComponent<ItemController>().items[i].name)
					{

						Debug.Log("Created Right Click Menu [RCMB: 33]");

						//Carry
						if (Slot.slot.parent.name == "Bag" && this.gameObject.GetComponent<ItemController>().items[i].canHotbar || Slot.slot.parent.name == "Hotbar"
							&& this.gameObject.GetComponent<ItemController>().items[i].canHotbar)
						{
							Action<Image> carry = new Action<Image>(CarryAction);
							rcmb.Add(new RightClickMenuButton("Carry", button, carry));
						}

						//Store
						if (Slot.slot.parent.name == "Handheld" || Slot.slot.parent.name == "Hotbar")
						{
							Action<Image> store = new Action<Image>(StoreAction);
							rcmb.Add(new RightClickMenuButton("Store", button, store));
						}
					
						//Equip
						if (Slot.slot.parent.name != "Equipment" && this.gameObject.GetComponent<ItemController>().items[i].isEquippable)
						{
							Action<Image> equip = new Action<Image>(EquipAction);
							rcmb.Add(new RightClickMenuButton("Equip", button, equip));
						}
						//Unequip
						else if (Slot.slot.parent.name == "Equipment")
						{
							Action<Image> unEquip = new Action<Image>(UnEquipAction);
							rcmb.Add(new RightClickMenuButton("UnEquip", button, unEquip));
						}

						//Use
						else if (this.gameObject.GetComponent<ItemController>().items[i].isUseable)
						{
							Action<Image> use = new Action<Image>(UseAction);
							rcmb.Add(new RightClickMenuButton("Use", button, use));
						}

						//Split Single
						if (Slot.slot.GetChild(0).gameObject.GetComponent<ItemAttributes>().amount > 1 && ic.bagIsFull == false)
						{
							Action<Image> splitSingle = new Action<Image>(SplitSingleAction);
							rcmb.Add(new RightClickMenuButton("Split-1", button, splitSingle));
						}

						//Split By Half
						if (Slot.slot.GetChild(0).gameObject.GetComponent<ItemAttributes>().amount > 3 && ic.bagIsFull == false)
						{
							Action<Image> splitHalf = new Action<Image>(SplitHalfAction);
							rcmb.Add(new RightClickMenuButton("Split", button, splitHalf));
						}

						//Drop
						Action<Image> drop = new Action<Image>(DropAction);
						rcmb.Add(new RightClickMenuButton("Drop", button, drop));
					}
				}
			}
		}
	}
	#endregion


	#region The Button Action References
	void CarryAction(Image contextPanel)
	{
		Destroy(contextPanel.gameObject);

		if (Slot.slot.childCount > 0)
		{
			this.gameObject.GetComponent<InventoryController>().CarryItem(Slot.slot.GetChild(0).gameObject);
		}
	}
	void StoreAction(Image contextPanel)
	{
		Destroy(contextPanel.gameObject);

		if (Slot.slot.childCount > 0)
		{
			this.gameObject.GetComponent<InventoryController>().StoreItem(Slot.slot.GetChild(0).gameObject);
		}
	}
	void EquipAction(Image contextPanel)
	{
		Destroy(contextPanel.gameObject);

		if (Slot.slot.childCount > 0)
		{
			this.gameObject.GetComponent<InventoryController>().UseItem(Slot.slot.GetChild(0).gameObject);
		}
	}

	void UnEquipAction(Image contextPanel)
	{
		Destroy(contextPanel.gameObject);

		if (Slot.slot.childCount > 0)
		{
			this.gameObject.GetComponent<InventoryController>().UnequipItem(Slot.slot.GetChild(0).gameObject);
		}
	}

	void UseAction(Image contextPanel)
	{
		Destroy(contextPanel.gameObject);

		if (Slot.slot.childCount > 0)
		{
			this.gameObject.GetComponent<InventoryController>().UseItem(Slot.slot.GetChild(0).gameObject);
		}

	}

	void SplitHalfAction(Image contextPanel)
	{
		Destroy(contextPanel.gameObject);

		if (Slot.slot.childCount > 0)
		{
			this.gameObject.GetComponent<InventoryController>().SplitItemHalf(Slot.slot.GetChild(0).gameObject);
		}
	}

	void SplitSingleAction(Image contextPanel)
	{
		Destroy(contextPanel.gameObject);

		if (Slot.slot.childCount > 0)
		{
			this.gameObject.GetComponent<InventoryController>().SplitItemSingle(Slot.slot.GetChild(0).gameObject);
		}
	}

	void DropAction(Image contextPanel)
	{
		Destroy(contextPanel.gameObject);

		if (Slot.slot.childCount > 0)
		{
			this.gameObject.GetComponent<InventoryController>().DropItem(Slot.slot.GetChild(0).gameObject);
		}
	}
	#endregion
}
