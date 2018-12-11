using UnityEngine;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
	//PUBLIC VARIABLES
	[Header("References")]
	public GameObject controller;

	[Header("This Slot")]
	public static Transform slot;   //This slot so it can be referenced from other scripts

	//PRIVATE VARIABLES
	private Transform returnSlot;   //Slot to be parented when switching items
	private RightClickMenuButtons rcmb;
	private ItemAttributes ia;
	private ItemDragging id;
	private InventoryController ic;
	private EquipmentController ec;


	void Start()
	{
		controller = GameObject.Find("controller");
		ic = controller.GetComponent<InventoryController>();
		ec = controller.GetComponent<EquipmentController>();

	}

	readonly PointerEventData p;

	#region Pointer Click Events

	//Detect current clicks on the GameObject (the one with the script attached)
	public void OnPointerClick(PointerEventData p)
	{

		if (GameObject.Find("RightClickMenu"))
		{
			Destroy(GameObject.Find("RightClickMenu"));
		}

		//Clicking with the Right Mouse Button
		if (p.button == PointerEventData.InputButton.Right && this.transform.childCount > 0)
		{
			//Making the current slot, the static slot that is required for outside script referencing
			slot = transform;
			CreateMenu();
			}

		//Clicking with the Left Mouse Button
		else if (GameObject.Find("RightClickMenu"))
		{
			Destroy(GameObject.Find("RightClickMenu"));
		}

	}

	public void CreateMenu()
	{
		rcmb = controller.GetComponent<RightClickMenuButtons>();

		//Setting the position of - and calling the Right Click Menu
		rcmb.DrawRightClickMenu();
		Vector2 pos = this.transform.position;

		RightClickMenu.Instance.CreateRightClickMenu(rcmb.rcmb, new Vector2(pos.x + 50, pos.y));
	}
	#endregion


	#region Slot Limitations
	//When a item has been dropped in this slot
	public void OnDrop(PointerEventData p)
	{
		ia = ItemDragging.itemBeingDragged.GetComponent<ItemAttributes>();
		id = ItemDragging.itemBeingDragged.GetComponent<ItemDragging>();
		returnSlot = id.startParent.transform;

		int maxStack = ia.maxStack;

		//Equipment limitations
		if (transform.parent.name == "Equipment")
		{
			if (ia.equipmentType == transform.name && ia.isEquippable == true)
			{
				Debug.Log("Equipping Item on Drop[Slot: 79]");

				GameObject item = ItemDragging.itemBeingDragged.gameObject;
				item.transform.SetParent(returnSlot);
				ic.EquipItem(item, ia.equipmentType);

			}
		}

		//Handheld and Hotbar limitations
		else if (transform.parent.name == "Hotbar" || transform.parent.name == "Handheld")
		{

			//If the targeted slot is a equipped item mand the dragged item came from the equipment slot
			if (transform.childCount > 0)
			{
				//Don't switch if the targeted slot it's equipment type is not the same as the dragged item
				if (transform.GetChild(0).GetComponent<ItemAttributes>().isEquippable && transform.GetChild(0).GetComponent<ItemAttributes>().equipmentType != ia.equipmentType)
				{
					id.transform.SetParent(returnSlot);
					id.transform.localPosition = new Vector2(0, 0);

				}

				//Handheld limitations
				else if (ia.amount == 1 && transform.parent.name == "Handheld")
				{
					//Removing Equipment Effects
					if (ItemDragging.hasBeenEquipped)
					{
						ec.RemoveEquipEffect(ItemDragging.itemBeingDragged.gameObject);
						ItemDragging.hasBeenEquipped = false;

					}

					DropToActive(maxStack);
				}

				//Hotbar limitations
				else if (ia.canHotbar && transform.parent.name == "Hotbar")
				{
					//Removing Equipment Effects
					if (ItemDragging.hasBeenEquipped)
					{
						ec.RemoveEquipEffect(ItemDragging.itemBeingDragged.gameObject);
						ItemDragging.hasBeenEquipped = false;

					}

					DropToActive(maxStack);
				}
			}

			//If the slot is empty
			else
			{
				//Handheld limitations
				if (ia.amount == 1 && transform.parent.name == "Handheld")
				{
					//Removing Equipment Effects
					if (ItemDragging.hasBeenEquipped)
					{
						ec.RemoveEquipEffect(ItemDragging.itemBeingDragged.gameObject);
						ItemDragging.hasBeenEquipped = false;

					}

					DropToActive(maxStack);
				}

				//Hotbar limitations
				else if (ia.canHotbar && transform.parent.name == "Hotbar")
				{
					//Removing Equipment Effects
					if (ItemDragging.hasBeenEquipped)
					{
						ec.RemoveEquipEffect(ItemDragging.itemBeingDragged.gameObject);
						ItemDragging.hasBeenEquipped = false;

					}

					DropToActive(maxStack);
				}
			}
		}
		//Bag
		else
		{

			//If the targeted slot is a equipped item and the dragged item came from the equipment slot
			if (transform.childCount > 0)
			{
				//Don't switch if the targeted slot it's equipment type is not the same as the dragged item
				if (returnSlot.parent.name == "Equipment" && transform.GetChild(0).GetComponent<ItemAttributes>().isEquippable && ia.isEquippable &&
					transform.GetChild(0).GetComponent<ItemAttributes>().equipmentType != ia.equipmentType)
				{
					id.transform.SetParent(returnSlot);
					id.transform.localPosition = new Vector2(0, 0);
					
				}

				else
				{
					//Removing Equipment Effects
					if (ItemDragging.hasBeenEquipped)
					{
						ec.RemoveEquipEffect(ItemDragging.itemBeingDragged.gameObject);
						ItemDragging.hasBeenEquipped = false;
					}

						DropToBag(maxStack);
					
				}

			}
			else
			{
				//Removing Equipment Effects
				if (ItemDragging.hasBeenEquipped)
				{
					ec.RemoveEquipEffect(ItemDragging.itemBeingDragged.gameObject);
					ItemDragging.hasBeenEquipped = false;
				}

				DropToBag(maxStack);
			}
		}
	}

	#endregion

	private void DropToActive(int maxStack)
	{
		ia = ItemDragging.itemBeingDragged.GetComponent<ItemAttributes>();
		id = ItemDragging.itemBeingDragged.GetComponent<ItemDragging>();


		if (ItemDragging.itemBeingDragged.GetComponent<ItemAttributes>().amount == 1)
		{
			//The slot is empty
			if (transform.childCount == 0)
			{
				//Drop the item in the slot`
				ItemDragging.itemBeingDragged.transform.SetParent(transform);
				ItemDragging.itemBeingDragged.transform.localPosition = new Vector2(0, 0);
				Debug.Log("Sorted Item to" + transform + "Slot: 206]");
				return;
			}

			//The slot is full
			else if (transform.childCount == 1)
			{
				//Get the slot for switching
				returnSlot = id.startParent.transform;

				//Drop the item in the slot`
				ItemDragging.itemBeingDragged.transform.SetParent(transform);
				ItemDragging.itemBeingDragged.transform.localPosition = new Vector2(0, 0);


				//Add the other item to the other slot
				transform.GetChild(0).transform.SetParent(returnSlot);
				returnSlot.GetChild(0).transform.localPosition = new Vector2(0, 0);

				Debug.Log("Switched item to" + transform + "from" + returnSlot + "Slot: 206]");
				return;
			}

		}

	}
	#region Putting Items in Slots and Stacking
	private void DropToBag(int maxStack)
	{
		ia = ItemDragging.itemBeingDragged.GetComponent<ItemAttributes>();
		id = ItemDragging.itemBeingDragged.GetComponent<ItemDragging>();

		//The slot is empty
		if (transform.childCount == 0)
		{

			ItemDragging.itemBeingDragged.transform.SetParent(transform);
			ItemDragging.itemBeingDragged.transform.localPosition = new Vector2(0, 0);
			Debug.Log("Sorted Item to Empty Slot [Slot: 244]");


		}

		//The slot is full
		else if (transform.childCount == 1)
		{

			int dragAmount = ItemDragging.itemBeingDragged.GetComponent<ItemAttributes>().amount;
			int slotAmount = transform.GetChild(0).GetComponent<ItemAttributes>().amount;

			//If the draggable item comes from the hotbar or handheld and switching happens with a stacked item, then don't switch at all
			if (returnSlot.parent.name == "Hotbar" && transform.GetChild(0).GetComponent<ItemAttributes>().amount > 1
				|| returnSlot.parent.name == "Handheld" && transform.GetChild(0).GetComponent<ItemAttributes>().amount > 1)
			{

				if (transform.GetChild(0).GetComponent<ItemAttributes>().amount + ItemDragging.itemBeingDragged.GetComponent<ItemAttributes>().amount <= maxStack)
				{
					Debug.Log("Stacked Item from the handhelds/hotbar to the bag [Slot: 262]");

					//Stack from hotbar/handheld to bag by dragging
					transform.GetChild(0).GetComponent<ItemAttributes>().amount += dragAmount;
					Destroy(ItemDragging.itemBeingDragged);

					//Change visible amount
					transform.GetChild(0).GetComponent<ItemStacks>().ChangeAmount(transform.GetChild(0).GetComponent<ItemAttributes>().amount);
					return;
				}

				else
				{
					Debug.Log("Cannot switch a stackable item to the hotbar or handhelds [Slot: 256]");
					return;
				}
			}

			//The items are identical
			else if (ItemDragging.itemBeingDragged.GetComponent<ItemAttributes>().name == transform.GetChild(0).GetComponent<ItemAttributes>().name && ItemDragging.itemBeingDragged.GetComponent<ItemAttributes>().isStackable)
			{

				if (transform.GetChild(0).GetComponent<ItemAttributes>().amount != maxStack && ItemDragging.itemBeingDragged.GetComponent<ItemAttributes>().amount != maxStack)
				{

					//Can be stacked
					if (dragAmount + slotAmount <= maxStack)
					{
						Debug.Log("Stacking on Drop[Slot: 139]");
						transform.GetChild(0).GetComponent<ItemAttributes>().amount += dragAmount;
						Destroy(ItemDragging.itemBeingDragged);

						//Change visible amount
						transform.GetChild(0).GetComponent<ItemStacks>().ChangeAmount(transform.GetChild(0).GetComponent<ItemAttributes>().amount);
						return;
					}

					//Have to be split stacked
					if (dragAmount + slotAmount > maxStack)
					{
						Debug.Log("Split-Stacking on Drop[Slot: 152]");
						//Add the stackable amount
						int leftOver = dragAmount -= (maxStack - slotAmount);
						transform.GetChild(0).GetComponent<ItemAttributes>().amount += maxStack - slotAmount;

						//Deal with the remaining item and add the left over amount to go before sending it back to be added
						ItemDragging.itemBeingDragged.GetComponent<ItemAttributes>().amount = leftOver;

						//Change visible amount
						transform.GetChild(0).GetComponent<ItemStacks>().ChangeAmount(transform.GetChild(0).GetComponent<ItemAttributes>().amount);
						ItemDragging.itemBeingDragged.GetComponent<ItemStacks>().ChangeAmount(leftOver);

						return;
					}
				}

				else
				{
				PutItemInSlot();
				}


			}

			else
			{
				PutItemInSlot();
			}
		}
	}
	#endregion

	#region Switching Items
	private void PutItemInSlot()
	{
		Debug.Log("Switching Item Slots[Slot: 182]");

		//Get the slot for switching
		returnSlot = id.startParent.transform;

		//Drop the item in the slot`
		ItemDragging.itemBeingDragged.transform.SetParent(transform);
		ItemDragging.itemBeingDragged.transform.localPosition = new Vector2(0, 0);


		//Add the other item to the other slot
		transform.GetChild(0).transform.SetParent(returnSlot);
		returnSlot.GetChild(0).transform.localPosition = new Vector2(0, 0);
	}
	#endregion
}
