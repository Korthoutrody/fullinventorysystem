using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CraftingController : MonoBehaviour {

	public Button craftingButton;
	public Transform craftingButtonStart;
	public Image craftingWindow;

	private string cat;
	private bool isDone;

	InventoryController ic;
	public GameObject player;
	PlayerInventory pi;

	public void ShowCategory(string name)
	{

		//The display order
		int order = 0;

		for (int i = 0; i < this.gameObject.GetComponent<ItemController>().items.Count; i++)
		{
			//Check for a match in name
			if (this.gameObject.GetComponent<ItemController>().items[i].isCraftable && name == this.gameObject.GetComponent<ItemController>().items[i].category)
			{
				Debug.Log(this.gameObject.GetComponent<ItemController>().items[i].name);

				Vector2 pos = craftingButtonStart.transform.position;

				Button button = Instantiate(craftingButton, new Vector2(pos.x, pos.y - (order * 15)), Quaternion.identity) as Button;
				button.transform.SetParent(craftingWindow.transform);
				button.transform.SetAsLastSibling();
				button.transform.localScale = new Vector2(1, 0.85f);
				button.name = gameObject.GetComponent<ItemController>().items[i].name;
				button.transform.GetChild(0).GetComponent<Text>().text = gameObject.GetComponent<ItemController>().items[i].name;
				order += 1;
			}
		}
	}

	public void DestroyPreviousCategory(string name)
	{

		//Destroy all the buttons from the previous category if there are any
		if (craftingWindow.transform.childCount > 1)
			{
				foreach (Transform child in craftingWindow.transform)
				{
					if (child.name != "CraftingButtonStart")
					{
						Destroy(child.gameObject);
						isDone = false;
					}
				isDone = true;
			}

			//Display the clicked category
				if (isDone)
				{
					ShowCategory(name);
					return;
				}

			}
			else
			{
				ShowCategory(name);
				return;
			}
		}

	public void CheckRecipe(string name)
	{
		pi = player.GetComponent<PlayerInventory>();
		ic = GetComponent<InventoryController>();
		ic.TrackPlayerInventory();

		//Check through the existing types
		for (int k = 0; k < this.gameObject.GetComponent<ItemController>().items.Count; k++)
		{
			//Check for a match in type
			if (name == this.gameObject.GetComponent<ItemController>().items[k].name)
			{
				bool recipe1 = false;
				bool recipe2 = false;
				bool recipe3 = false;

				GameObject item = gameObject.GetComponent<ItemController>().items[k].model;

				//The first recipe item
				for (int m = 0; m < pi.playerInvItems.Count; m++)
				{
					if (gameObject.GetComponent<ItemController>().items[k].recipes[0].name == pi.playerInvItems[m].name
						&& gameObject.GetComponent<ItemController>().items[k].recipes[0].amount <= pi.playerInvItems[m].count)
					{
						recipe1 = true;
					}
				}


				//If there is a second recipe item
				for (int m = 0; m < pi.playerInvItems.Count; m++)
				{
					if (gameObject.GetComponent<ItemController>().items[k].recipes.Count >= 2)
					{
						if (gameObject.GetComponent<ItemController>().items[k].recipes[1].name == pi.playerInvItems[m].name
							&& gameObject.GetComponent<ItemController>().items[k].recipes[1].amount <= pi.playerInvItems[m].count
							)
						{
							recipe2 = true;
						}
					}
				}

				//If there is a third recipe item
				for (int m = 0; m < pi.playerInvItems.Count; m++)
				{
					if (gameObject.GetComponent<ItemController>().items[k].recipes.Count >= 3)
					{
						if (gameObject.GetComponent<ItemController>().items[k].recipes[2].name == pi.playerInvItems[m].name
							&& gameObject.GetComponent<ItemController>().items[k].recipes[2].amount <= pi.playerInvItems[m].count
							)
						{
							recipe3 = true;
						}

					}

				}
				//If all recipe items are available
				if (gameObject.GetComponent<ItemController>().items[k].recipes.Count == 1 && recipe1)
				{
					ic.RemoveItems(gameObject.GetComponent<ItemController>().items[k].recipes[0].name,
						gameObject.GetComponent<ItemController>().items[k].recipes[0].amount);

					CraftItem(item);
				}

				else if (gameObject.GetComponent<ItemController>().items[k].recipes.Count == 2 && recipe1 && recipe2)
				{

					ic.RemoveItems(gameObject.GetComponent<ItemController>().items[k].recipes[0].name,
						gameObject.GetComponent<ItemController>().items[k].recipes[0].amount);

					ic.RemoveItems(gameObject.GetComponent<ItemController>().items[k].recipes[1].name,
						gameObject.GetComponent<ItemController>().items[k].recipes[1].amount);

					CraftItem(item);

				}

				else if (gameObject.GetComponent<ItemController>().items[k].recipes.Count == 3 && recipe1 && recipe2 && recipe3)
				{
					ic.RemoveItems(gameObject.GetComponent<ItemController>().items[k].recipes[0].name,
						gameObject.GetComponent<ItemController>().items[k].recipes[0].amount);

					ic.RemoveItems(gameObject.GetComponent<ItemController>().items[k].recipes[1].name,
						gameObject.GetComponent<ItemController>().items[k].recipes[1].amount);

					ic.RemoveItems(gameObject.GetComponent<ItemController>().items[k].recipes[2].name,
						gameObject.GetComponent<ItemController>().items[k].recipes[2].amount);

					CraftItem(item);

				}
			}
			}
		}

	public void CraftItem(GameObject item)
	{

		Debug.Log("Crafting " + name + " [CC: 74)");
		ic.AddItem(item.name, 1);

		if (!InventoryController.sorted)
		{
			//Make a copy of the object of that item in order to drop it to the floor
			GameObject go = Instantiate(item, ic.dropPoint.transform.position + ic.cam.transform.forward * 1, Quaternion.identity);
			go.GetComponent<Rigidbody>().velocity = 5 * ic.cam.transform.forward;
			go.name = item.name;
			go.GetComponent<ItemEntity>().amount = 1;
		}
	}
	
}

