using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
	//PUBLIC VARIABLES
	[Title("Using Items")]
	public bool canUse = true;
	public float useCooldown;
	public float curUseCooldown;
	public bool useAborted;

	[Title("Switching Items")]
	public bool canSwitch = true;
	public float switchCooldown;
	public float curSwitchCooldown;

	[Title("Size of Inventories")]
	public int bagSize;
	public int hotbarSize;

	[Title("Room Detection")]
	public bool hotbarIsFull;
	public bool bagIsFull;
	public bool handsAreFull;

	[Title("Building")]
	public bool isBuilding;
	public ScrollRect buildScroll;

	[FoldoutGroup("Keycodes")]
	public KeyCode selectRight;
	[FoldoutGroup("Keycodes")]
	public KeyCode selectLeft;
	[FoldoutGroup("Keycodes")]
	public KeyCode selectBuild;
	[FoldoutGroup("Keycodes")]
	public KeyCode drop;
	[FoldoutGroup("Keycodes")]
	public KeyCode use;
	[FoldoutGroup("Keycodes")]
	public KeyCode hotSelect1;
	[FoldoutGroup("Keycodes")]
	public KeyCode hotSelect2;
	[FoldoutGroup("Keycodes")]
	public KeyCode hotSelect3;
	[FoldoutGroup("Keycodes")]
	public KeyCode hotSelect4;

	[FoldoutGroup("References")]
	public GameObject player;
	[FoldoutGroup("References")]
	public GameObject dropPoint;
	[FoldoutGroup("References")]
	public Camera cam;
	[FoldoutGroup("References")]
	public GameObject bag;
	[FoldoutGroup("References")]
	public GameObject hotbar;
	[FoldoutGroup("References")]
	public Transform equipment;

	[FoldoutGroup("Fake Transforms")]
	public Transform selectedSlot;
	[FoldoutGroup("Fake Transforms")]
	public Transform selectedHot;
	[FoldoutGroup("Fake Transforms")]
	public Transform sortSlot;

	[FoldoutGroup("Generating Slots")]
	public GameObject slot;
	[FoldoutGroup("Generating Slots")]
	public Vector2 bagSlotsGenStart;
	[FoldoutGroup("Generating Slots")]
	public Vector2 slotSize;
	[FoldoutGroup("Generating Slots")]
	public Vector2 bagSlotSpacing;
	[FoldoutGroup("Generating Slots")]
	public Vector2 hotSlotsGenStart;
	[FoldoutGroup("Generating Slots")]
	public Vector2 hotSlotSpacing;

	[FoldoutGroup("Inventory")]
	public Transform slotLeft;
	[FoldoutGroup("Inventory")]
	public Transform slotRight;
	[FoldoutGroup("Inventory")]
	public Transform[] equipmentSlots;

	//To be able to destroy Game Objects after sorting and change amount values in other scripts
	public static bool sorted;
	public static int sortedValue;


	//PRIVATE VARIABLES
	private GameObject itemToStack;
	EquipmentController ec;
	ItemController items;
	PlayerInventory pi;
	CraftingController cc;

	void Start()
	{
		//Set the first cooldown for using and switching items
		curUseCooldown = useCooldown;
		curSwitchCooldown = switchCooldown;

		//Automatically generate all the required slots for the bag
		for (int i = 0; i < bagSize; i++)
		{
			GameObject newSlot = Instantiate(slot, transform.position, Quaternion.identity);
			newSlot.transform.SetParent(bag.transform);
			newSlot.name = "Slot" + (i);
			newSlot.transform.localScale = new Vector3(1, 1, 1);

			if (i < 4) //Upper Row
			{
				newSlot.transform.localPosition = new Vector2(bagSlotsGenStart.x + ((slotSize.x + bagSlotSpacing.x) * i), bagSlotsGenStart.y);
			}

			else if (i > 3 && i < 8) //Middle Row
			{
				newSlot.transform.localPosition = new Vector2(bagSlotsGenStart.x + ((slotSize.x + bagSlotSpacing.x) * (i - 4)), bagSlotsGenStart.y - (slotSize.y + bagSlotSpacing.y));

			}

			else // Lower Row
			{
				newSlot.transform.localPosition = new Vector2(bagSlotsGenStart.x + ((slotSize.x + bagSlotSpacing.x) * (i - 8)), bagSlotsGenStart.y - ((slotSize.y + bagSlotSpacing.y) * 2));

			}



		}

		//Automatically generate all the required slots for the hotbar
		for (int i = 0; i < hotbarSize; i++)
		{
			GameObject newSlot = Instantiate(slot, transform.position, Quaternion.identity);
			newSlot.transform.SetParent(hotbar.transform);
			newSlot.name = "Slot" + (i);
			newSlot.transform.localScale = new Vector3(1, 1, 1);

			newSlot.transform.localPosition = new Vector2(hotSlotsGenStart.x + ((slotSize.x + hotSlotSpacing.x) * i), hotSlotsGenStart.y);

		}
	}

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.K))
		{
			TrackPlayerInventory();
		}
		//Detect if slots are full or empty
		CheckSizeHotbar();
		CheckSizeBag();
		CheckSizeHandhelds();

		#region Hotbar Selecting
		//Toolbar selecting <--------------- NEEDS TO BE AUTOMATED -----------------------------------------------
		if (Input.GetKeyDown(hotSelect1) && canSwitch && !isBuilding)
		{
			selectedHot = hotbar.transform.GetChild(1);
			canSwitch = false;
			SwitchItem();
		}

		if (Input.GetKeyDown(hotSelect2) && canSwitch && !isBuilding)
		{
			selectedHot = hotbar.transform.GetChild(2);
			canSwitch = false;
			SwitchItem();
		}

		if (Input.GetKeyDown(hotSelect3) && canSwitch && !isBuilding)
		{
			selectedHot = hotbar.transform.GetChild(3);
			canSwitch = false;
			SwitchItem();
		}

		if (Input.GetKeyDown(hotSelect4) && canSwitch && !isBuilding)
		{
			selectedHot = hotbar.transform.GetChild(4);
			canSwitch = false;
			SwitchItem();
		}

		#endregion

		//Checking if the player has started moving
		if (player.GetComponent<PlayerMovement>().playerIsMoving == true)
		{
			useAborted = true;
		}

		#region Selecting a Hand
		//Selecting the used hand for controlling the item(s) in that hand
		if (Input.GetKeyDown(selectLeft) && selectedSlot != slotLeft)
		{
			selectedSlot = slotLeft;
			isBuilding = false;
			Debug.Log(selectedSlot);
		}
		if (Input.GetKeyDown(selectRight) && selectedSlot != slotRight)
		{
			selectedSlot = slotRight;
			isBuilding = false;
			Debug.Log(selectedSlot);
		}

		//Building
		if (Input.GetKeyDown(selectBuild))
		{
			selectedSlot = null;
			Debug.Log(selectedSlot);
			isBuilding = true;
		}

		#endregion


		#region Keys for Dropping and Using items
		//Dropping items
		if (Input.GetKeyDown(drop) && selectedSlot != null)
		{
			DropItem(selectedSlot.GetChild(0).gameObject);
		}

		//Using items
		if (Input.GetKeyDown(use) && selectedSlot != null && canUse && selectedSlot.childCount > 0)
		{
			UseItem(selectedSlot.GetChild(0).gameObject);
		}

		#endregion


		#region Cooldown For The Useage of Items
		//Cooldown for using items
		if (!canUse)
		{
			curUseCooldown = curUseCooldown - (1 * Time.deltaTime);

			if (curUseCooldown <= 0)
			{
				curUseCooldown = useCooldown;
				canUse = true;
				return;
			}
		}
		#endregion

		#region Cooldown for Switching Items
		//Cooldown for switching items
		if (!canSwitch)
		{
			curSwitchCooldown = curSwitchCooldown - (1 * Time.deltaTime);

			if (curSwitchCooldown <= 0)
			{
				curSwitchCooldown = switchCooldown;
				canSwitch = true;
				return;
			}
		}
		#endregion
	}


	#region Checking Sizes of Inventories
	//Check the size of the handhelds
	private void CheckSizeHandhelds()
	{
		if (slotLeft.childCount > 0 && slotRight.childCount > 0)
		{
			handsAreFull = true;
		}

		else
		{
			handsAreFull = false;
		}
	}
	//Check the size of the hotbar
	private void CheckSizeHotbar()
	{
		bool ready = true;
		for (int i = 1; i < hotbar.transform.childCount; i++)
		{
			if (hotbar.transform.GetChild(i).childCount == 0)
			{
				ready = false;
				hotbarIsFull = false;
			}
		}
		if (ready)
		{
			hotbarIsFull = true;
		}
	}

	//Check the size of the bag
	private void CheckSizeBag()
	{
		bool ready = true;
		for (int i = 1; i < bag.transform.childCount; i++)
		{
			if (bag.transform.GetChild(i).childCount == 0)
			{
				ready = false;
				bagIsFull = false;
			}
		}
		if (ready)
		{
			bagIsFull = true;
		}

	}
	#endregion


	#region Adding Items to Inventory
	public void AddItem(string name, int amount)
	{
		//Check through the existing types
		for (int k = 0; k < this.gameObject.GetComponent<ItemController>().items.Count; k++)
		{
			//Check for a match in type
			if (name == this.gameObject.GetComponent<ItemController>().items[k].name)
			{

				//Check the amount of the item in the inventory and the item to be added and the max stackable amount for that item
				int maxAmount = gameObject.GetComponent<ItemController>().items[k].maxStack;
				Image icon = gameObject.GetComponent<ItemController>().items[k].icon;
				bool isStackable = gameObject.GetComponent<ItemController>().items[k].isStackable;
				sorted = false;
				sortedValue = amount;

				if (handsAreFull)
				{

					//The sort method that changes depending on the selected stacking order
					bool stackSort = false;
					bool splitSort = false;
					bool emptySort = false;

					//Have a seperate for-loop for every stacking method with 
					for (int m = 1; m < bag.transform.childCount; m++)
					{

						if (bag.transform.GetChild(m).childCount == 1 && !stackSort && !splitSort && !emptySort)
						{
							if (bag.transform.GetChild(m).GetChild(0).gameObject.name == name && gameObject.GetComponent<ItemController>().items[k].isStackable)
							{
								if (bag.transform.GetChild(m).GetChild(0).gameObject.GetComponent<ItemAttributes>().amount + amount <= maxAmount)
								{
									stackSort = true;
									itemToStack = bag.transform.GetChild(m).GetChild(0).gameObject;
									break;
								}

								else if (bag.transform.GetChild(m).GetChild(0).gameObject.GetComponent<ItemAttributes>().amount
								+ amount > maxAmount && bag.transform.GetChild(m).GetChild(0).gameObject.GetComponent<ItemAttributes>().amount != maxAmount)
								{
									splitSort = true;
									itemToStack = bag.transform.GetChild(m).GetChild(0).gameObject;
									break;
								}
							}
						}
					}

					for (int m = 1; m < bag.transform.childCount; m++)
					{
						if (bag.transform.GetChild(m).childCount == 0)
						{
							emptySort = true;
							sortSlot = bag.transform.GetChild(m).transform;
							break;
						}
					}

					if (stackSort && itemToStack != null)
					{
						Debug.Log("Picking Up Item To Stack [IC: 313]");
						itemToStack.GetComponent<ItemAttributes>().amount += amount;
						itemToStack.GetComponent<ItemStacks>().ChangeAmount(itemToStack.GetComponent<ItemAttributes>().amount);                 //Change the visible amount integer
						sorted = true;
						return;
					}
					else if (splitSort && itemToStack != null)
					{
						Debug.Log("Picking Up Item To Split-Stack [IC: 324]");

						//Add the stackable amount
						int leftOver = amount -= (maxAmount - itemToStack.GetComponent<ItemAttributes>().amount);
						itemToStack.GetComponent<ItemAttributes>().amount += maxAmount - itemToStack.GetComponent<ItemAttributes>().amount;
						itemToStack.GetComponent<ItemStacks>().ChangeAmount(itemToStack.GetComponent<ItemAttributes>().amount);    //Change the visible amount integer

						//Deal with the remaining item and add the left over amount to go before sending it back to be added
						amount = leftOver;
						AddItem(name, amount);

						//Changing the value for the Item Entity
						sortedValue = amount;
						return;
					}
					else if (emptySort && sortSlot != null)
					{
						Debug.Log("Picking Up Item To Empty Stack [IC: 340]");

						Image ins = Instantiate(icon, transform.position, Quaternion.identity);
						ins.name = name;
						ins.GetComponent<ItemAttributes>().amount = amount;
						ins.GetComponent<ItemStacks>().ChangeAmount(amount);
						ins.transform.SetParent(sortSlot);

						ins.transform.localPosition = new Vector2(0, 0);
						ins.transform.localScale = new Vector2(1, 1);
						sorted = true;
						return;
					}
				}

				//Add to hands or hotbar if stacking in the bag is not possible
				else if (!handsAreFull)
				{
					//Check if there are empty hands available
					if (slotLeft.childCount == 0 || slotRight.childCount == 0)
					{
						Debug.Log("Picking Up" + name + "To Empty Handhelds [IC: 362]");

						//Make a copy of the sprite of that item in order to place it in a slot
						Image item = Instantiate(icon, transform.position, Quaternion.identity);
						item.name = name;

						//If there is a stacked amount being taken
						if (sortedValue > 1)
						{
							sortedValue -= 1;
						}

						else
						{
							sorted = true;
						}

						//Put the item in the empty slot
						if (slotRight.childCount == 0)
						{
							item.transform.SetParent(slotRight);
							item.transform.localPosition = new Vector2(0, 0);
							item.transform.localScale = new Vector2(1, 1);
							return;
						}

						else
						{
							item.transform.SetParent(slotLeft);
							item.transform.localPosition = new Vector2(0, 0);
							item.transform.localScale = new Vector2(1, 1);
							return;

						}

					}
					//Check if the toolbelt has slots available and the item can be added to the toolbelt
					else if (!hotbarIsFull && !isStackable) //<---------------------  ONLY NON-STACKABLES / HOLDER ITEMS ------------------------------>
					{
						Debug.Log("Picking Up Item To Empty Hotbar [IC: 390]");

						//Check through the existing items
						for (int m = 1; m < hotbar.transform.childCount; m++)
						{
							//Check for a match in name
							if (hotbar.transform.GetChild(m).childCount == 0)
							{
								Image item = Instantiate(icon, transform.position, Quaternion.identity);
								icon.name = name;

								icon.transform.SetParent(hotbar.transform.GetChild(m));
								icon.transform.localPosition = new Vector2(0, 0);
								icon.transform.localScale = new Vector2(1, 1);
								sorted = true;
								return;

							}
						}
					}

					else
					{
						sorted = false;
						return;
					}

				}
			}
		}
	}
	#endregion

	#region Splitting a Single Item From Stack
	public void SplitItemSingle(GameObject item)
	{
		ItemAttributes ia = item.GetComponent<ItemAttributes>();

		for (int k = 1; k < bag.transform.childCount; k++)
		{
			if (bag.transform.GetChild(k).childCount == 0)
			{

				Debug.Log("Split the Item by 1 [IC: 577]");
				ia.amount -= 1;

				Image icon = Instantiate(ia.icon, transform.position, Quaternion.identity);
				icon.GetComponent<ItemAttributes>().amount = 1;
				icon.name = item.name;

				icon.transform.SetParent(bag.transform.GetChild(k));
				icon.transform.localPosition = new Vector2(0, 0);
				icon.transform.localScale = new Vector2(1, 1);

				//Change the visible amount integer
				item.GetComponent<ItemStacks>().ChangeAmount(ia.amount);
				icon.GetComponent<ItemStacks>().ChangeAmount(1);
				return;
			}
		}
	}
	#endregion


	#region Splitting a Stack in Half
	public void SplitItemHalf(GameObject item)
	{
		ItemAttributes ia = item.GetComponent<ItemAttributes>();

		for (int k = 1; k < bag.transform.childCount; k++)
		{
			if (bag.transform.GetChild(k).childCount == 0)
			{
				int amount = ia.amount;

				//Number has a decimal
				if (amount % 2 != 0)
				{
					Debug.Log("Split the Item by half w/ CeilToInt[IC: 456");
					int newAmount = Mathf.CeilToInt(amount / 2);
					ia.amount -= newAmount;

					Image icon = Instantiate(ia.icon, transform.position, Quaternion.identity);
					icon.GetComponent<ItemAttributes>().amount = newAmount;
					icon.name = item.name;

					icon.transform.SetParent(bag.transform.GetChild(k));
					icon.transform.localPosition = new Vector2(0, 0);
					icon.transform.localScale = new Vector2(1, 1);

					//Change the visible amount integer
					item.GetComponent<ItemStacks>().ChangeAmount(ia.amount);
					icon.GetComponent<ItemStacks>().ChangeAmount(newAmount);

					return;
				}

				//Number has no decimal
				else
				{
					Debug.Log("Split the Item by half [IC: 473]");
					int newAmount = amount / 2;
					ia.amount -= newAmount;

					Image icon = Instantiate(ia.icon, transform.position, Quaternion.identity);
					icon.GetComponent<ItemAttributes>().amount = newAmount;
					icon.name = item.name;

					icon.transform.SetParent(bag.transform.GetChild(k));
					icon.transform.localPosition = new Vector2(0, 0);
					icon.transform.localScale = new Vector2(1, 1);

					//Change the visible amount integer
					item.GetComponent<ItemStacks>().ChangeAmount(newAmount);
					icon.GetComponent<ItemStacks>().ChangeAmount(newAmount);

					return;

				}
			}
		}
	}
	#endregion

	#region Dropping Items
	public void DropItem(GameObject item)
	{
		ItemAttributes ia = item.GetComponent<ItemAttributes>();
		dropPoint = GameObject.Find("Bip01");
		cam = GameObject.Find("PlayerView").GetComponent<Camera>();

		float dropDistance = 1;

		//Make a copy of the object of that item in order to drop it to the floor
		GameObject go = Instantiate(ia.model, dropPoint.transform.position + cam.transform.forward * dropDistance, Quaternion.identity);
		go.GetComponent<Rigidbody>().velocity = 5 * cam.transform.forward;
		go.name = ia.name;
		go.GetComponent<ItemEntity>().amount = ia.amount;

		//Destroy the item from the players hand as it has been dropped
		Destroy(item);
	}
	#endregion

	#region Using Items
	public void UseItem(GameObject item)
	{
		ItemAttributes ia = item.GetComponent<ItemAttributes>();
		canUse = false;

		//If the item is a useable
		if (ia.isUseable)
		{
			//Run the Use Timer
			bool isDepletable = ia.isDepletable;
			bool ranOnce = false;
			float useTime = ia.useTime;

			StartCoroutine(UseTimer(ranOnce, useTime, isDepletable, item));

		}

		//If the item is equippable
		else if (ia.isEquippable)
		{
			string type = ia.equipmentType;
			EquipItem(item, type);
			return;

		}
	}

	#endregion


	#region Consuming Items
	private void ConsumeItem(bool ranOnce, bool isDepletable, GameObject item)
	{
		ItemAttributes ia = item.GetComponent<ItemAttributes>();


		if (ranOnce)
		{
			//If the item is depletable, destroy it
			if (isDepletable)
			{
				if (ia.amount > 1)
				{
					ia.amount -= 1;
					item.GetComponent<ItemStacks>().ChangeAmount(ia.amount);

				}

				else
				{
					Destroy(item);
				}
			}
			//For holder items
			else
			{

			}

			//Go to the Player Actions script to handle the exact usage
			player.GetComponent<PlayerActions>().UseItem(item);

		}
	}
	#endregion

	#region Equipping Items
	public void EquipItem(GameObject item, string type)
	{
		ec = this.gameObject.GetComponent<EquipmentController>();

		equipment = GameObject.Find("Equipment").transform; //Anti-Glitch with no access to Equipment with Public Variable on Inspector
		Transform slotForEquip = equipment.Find(type);
		Debug.Log(slotForEquip);

		//If there is an item equipped in the selected Equipment slot and a switch is in order
		if (slotForEquip.childCount == 1)
		{
			Transform slot = item.transform.parent;
			GameObject equipped = slotForEquip.GetChild(0).gameObject;

			item.transform.SetParent(slotForEquip);
			item.transform.localPosition = new Vector2(0, 0);

			equipped.transform.SetParent(slot);
			equipped.transform.localPosition = new Vector2(0, 0);

			ec.AddEquipEffect(item.name);
			ec.RemoveEquipEffect(equipped);

			Debug.Log("Equipped Item w/ Switch [IC: 577]");


		}
		//When the selected equipment slot is empty
		else if (slotForEquip.childCount == 0)
		{
			item.transform.SetParent(slotForEquip);
			item.transform.localPosition = new Vector2(0, 0);

			ec.AddEquipEffect(item.name);

			Debug.Log("Equipped Item [IC: 589]");

		}

	}
	#endregion


	#region Unequipping Items
	public void UnequipItem(GameObject item)
	{
		for (int i = 1; i < bag.transform.childCount; i++)
		{
			if (bag.transform.GetChild(i).childCount == 0)
			{
				item.transform.SetParent(bag.transform.GetChild(i));
				item.transform.localPosition = new Vector2(0, 0);
				ec.RemoveEquipEffect(item);
				Debug.Log("Unequipped Item [IC: 608]");
				return;
			}
		}

	}

	#endregion


	#region Switching Items from Hotbar to Handhelds or backwards
	private void SwitchItem()
	{

		//	< ---------------NEEDS TO HAVE A SWITCH TIMER TO EQUALIZE ANIMATIONS---------------------------------------------- -
		if (selectedSlot != null && selectedHot != null)
		{
			//Check if there is an item in both the selected hand and hotbar
			if (selectedSlot.childCount > 0 && selectedHot.childCount > 0)
			{
				Debug.Log("Switched Hotbar Item With Handheld [IC: 628]");

				//Switch the items
				GameObject itemInHand = selectedSlot.GetChild(0).gameObject;
				GameObject itemInHot = selectedHot.GetChild(0).gameObject;

				itemInHand.transform.SetParent(selectedHot);
				itemInHot.transform.SetParent(selectedSlot);
				return;
			}
		}
	}
	#endregion

	public void CarryItem(GameObject item)
	{
		if (!handsAreFull)
		{
			if (slotLeft.childCount == 0)
			{
				item.transform.SetParent(slotLeft);
				item.transform.localPosition = new Vector2(0, 0);
				item.transform.localScale = new Vector2(1, 1);
			}

			else if (slotRight.childCount == 0)
			{
				item.transform.SetParent(slotRight);
				item.transform.localPosition = new Vector2(0, 0);
				item.transform.localScale = new Vector2(1, 1);
			}
		}
	}

	public void StoreItem(GameObject item)
	{
		if (!bagIsFull)
		{
			for (int k = 1; k < bag.transform.childCount; k++)
			{
				if (bag.transform.GetChild(k).childCount == 0)
				{
					item.transform.SetParent(bag.transform.GetChild(k));
					item.transform.localPosition = new Vector2(0, 0);
					item.transform.localScale = new Vector2(1, 1);
					break;
				}
			}
		}
	}
	#region Timer for the Useage of Items
	private IEnumerator UseTimer(bool ranOnce, float timeToWait, bool isDepletable, GameObject item)
	{
		useAborted = false;
		yield return new WaitForSeconds(timeToWait);

		while (timeToWait > 0)
		{
			//Stop the action when it has been aborted by movement or looking away
			if (useAborted == true)
			{
				yield break;
			}

			//Make sure it only runs once
			if (!ranOnce)
			{
				ranOnce = true;
				ConsumeItem(ranOnce, isDepletable, item);
				yield break;
			}
		}
	}
	#endregion

	public void TrackPlayerInventory()
	{
		pi = player.GetComponent<PlayerInventory>();

		pi.playerInvItems.Clear();

		foreach (Transform child in bag.transform)
		{
			if (child.childCount > 0)
			{
				//If there is not already an item in the list. This will counter an issue with not being able to compare to a name of a nulled list
				if (pi.playerInvItems.Count == 0)
				{
					pi.playerInvItems.Add(new PlayerInventory.PlayerInvItems(child.GetChild(0).name, 0));
				}

				//If there are items in the player's inventory already and names can be compared
				if (pi.playerInvItems.Count != 0)
				{
					bool isReady = true;

					for (int i = 0; i < pi.playerInvItems.Count; i++)
					{
						//If the item does already exist, figure out the total amount of that item
						if (child.GetChild(0).name == pi.playerInvItems[i].name)
						{
							pi.playerInvItems[i].count += child.GetChild(0).GetComponent<ItemAttributes>().amount;
							isReady = false;
						}

					}

					//If the item does not already exist in the list, create it.
					if (isReady)
					{
						pi.playerInvItems.Add(new PlayerInventory.PlayerInvItems(child.GetChild(0).name, child.GetChild(0).GetComponent<ItemAttributes>().amount));

					}
				}
			}

		}
		return;

	}
	public void RemoveItems(string name, int amount)
	{
		cc = gameObject.GetComponent<CraftingController>();
		int leftover = amount;

		for (int m = 1; m < bag.transform.childCount; m++)
		{
			if (bag.transform.GetChild(m).childCount > 0)
			{
				if (bag.transform.GetChild(m).GetChild(0).name == name)
				{
					//If the amount can be taken by one item stack

					if (leftover > bag.transform.GetChild(m).GetChild(0).GetComponent<ItemAttributes>().amount)
					{

						leftover -= bag.transform.GetChild(m).GetChild(0).GetComponent<ItemAttributes>().amount;

						DestroyImmediate(bag.transform.GetChild(m).GetChild(0).gameObject);
						RemoveItems(name, leftover);
						return;
					}

					if (leftover == bag.transform.GetChild(m).GetChild(0).GetComponent<ItemAttributes>().amount)
					{
						DestroyImmediate(bag.transform.GetChild(m).GetChild(0).gameObject);
						return;
					}

					if (leftover < bag.transform.GetChild(m).GetChild(0).GetComponent<ItemAttributes>().amount)
					{
						bag.transform.GetChild(m).GetChild(0).GetComponent<ItemAttributes>().amount -= amount;
						bag.transform.GetChild(m).GetChild(0).GetComponent<ItemStacks>().ChangeAmount(bag.transform.GetChild(m).GetChild(0).GetComponent<ItemAttributes>().amount);
						return;
					}
				}
			}
		}

			}
		}



