using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour
{
	//PUBLIC VARIABLES
	[Header("References")]
	public Transform tooltip;
	public new Text name;
	public Text desc;
	public Image icon;

	public Image recipe1;
	public Image recipe2;
	public Image recipe3;

	public GameObject player;

	//PRIVATE VARIABLES
	ItemAttributes ia;
	PlayerInventory pi;
	InventoryController ic;
	private string recipe1Req, recipe2Req, recipe3Req, recipe1Inv, recipe2Inv, recipe3Inv;
	void Start()
	{

		//Start with the tooltip inactive
		tooltip.gameObject.SetActive(false);

	}

	//Show Tooltip
	public void ShowTooltip(GameObject item)
	{
		ia = item.GetComponent<ItemAttributes>();
		pi = player.GetComponent<PlayerInventory>();
		ic = gameObject.GetComponent<InventoryController>();
		ic.TrackPlayerInventory();

		//Populate the tooltip with the information of the specific item
		name.text = ia.gameObject.name;
		desc.text = ia.desc;
		icon.GetComponent<Image>().sprite = ia.icon.GetComponent<Image>().sprite;

		//Default values
		recipe1Inv = "0";
		recipe2Inv = "0";
		recipe3Inv = "0";

		//Populate the tooltip's recipe
		for (int k = 0; k < this.gameObject.GetComponent<ItemController>().items.Count; k++)
		{
			//Find the correct item
			if (gameObject.GetComponent<ItemController>().items[k].name == item.name)
			{


				//If the item has atleast one recipe item |RECIPE ITEM 1|
				if (this.gameObject.GetComponent<ItemController>().items[k].recipes.Count > 0)
				{
					//The amount required to make the requirede recipe item
					recipe1Req = this.gameObject.GetComponent<ItemController>().items[k].recipes[0].amount.ToString();


					if (pi.playerInvItems.Count > 0)
					{
						for (int m = 0; m < pi.playerInvItems.Count; m++)
						{
							Debug.Log(gameObject.GetComponent<ItemController>().items[k].recipes[0].name);
							//Look for the required recipe item in the player inventory
							if (pi.playerInvItems[m].name == gameObject.GetComponent<ItemController>().items[k].recipes[0].name)
							{

								//The amount of the required recipe item that the player has
								recipe1Inv = pi.playerInvItems[m].count.ToString();
								ShowRecipe(1, this.gameObject.GetComponent<ItemController>().items[k].recipes[0].name);
								break;

							}
						}
					}
					else
					{
						ShowRecipe(1, this.gameObject.GetComponent<ItemController>().items[k].recipes[0].name);
					}
				}

				//There is no recipe for this item
				else
				{
					recipe1.gameObject.SetActive(false);
					recipe2.gameObject.SetActive(false);
					recipe3.gameObject.SetActive(false);
				}


				//If the item has atleast two recipe items | RECIPE ITEM 2|
				if (this.gameObject.GetComponent<ItemController>().items[k].recipes.Count > 1)
				{

					Debug.Log("2");
					//The amount required to make the requirede recipe item
					recipe2Req = this.gameObject.GetComponent<ItemController>().items[k].recipes[1].amount.ToString();

					if (pi.playerInvItems.Count > 0)
					{
						Debug.Log("2b");

						for (int m = 0; m < pi.playerInvItems.Count; m++)
						{
							//Look for the required recipe item in the player inventory
							if (pi.playerInvItems[m].name == gameObject.GetComponent<ItemController>().items[k].recipes[1].name)
							{

								//The amount of the required recipe item that the player has
								recipe2Inv = pi.playerInvItems[m].count.ToString();
								ShowRecipe(2, this.gameObject.GetComponent<ItemController>().items[k].recipes[1].name);
								break;

							}

						}
					}

					else
					{
						Debug.Log("2c");

						ShowRecipe(2, this.gameObject.GetComponent<ItemController>().items[k].recipes[1].name);
					}
				}

				else
				{
					recipe2.gameObject.SetActive(false);
					recipe3.gameObject.SetActive(false);
				}


				//If the item has atleast three recipe items | RECIPE ITEM 3|
				if (this.gameObject.GetComponent<ItemController>().items[k].recipes.Count > 2)
				{

					//The amount required to make the requirede recipe item
					recipe3Req = this.gameObject.GetComponent<ItemController>().items[k].recipes[2].amount.ToString();

					if (pi.playerInvItems.Count > 0)
					{
						for (int m = 0; m < pi.playerInvItems.Count; m++)
						{
							//Look for the required recipe item in the player inventory
							if (pi.playerInvItems[m].name == gameObject.GetComponent<ItemController>().items[k].recipes[2].name)
							{

								//The amount of the required recipe item that the player has
								recipe3Inv = pi.playerInvItems[m].count.ToString();
								ShowRecipe(3, this.gameObject.GetComponent<ItemController>().items[k].recipes[2].name);
								break;
							}
						}
					}
					else
					{
						ShowRecipe(3, this.gameObject.GetComponent<ItemController>().items[k].recipes[2].name);
					}
				}

				else
				{
					recipe3.gameObject.SetActive(false);
				}
			}

		}

		//Show the tooltip
		if (tooltip.gameObject.activeInHierarchy == false)
		{
			tooltip.gameObject.SetActive(true);
		}
	}

	//Hide Tooltip
	public void HideTooltip()
	{
		if (tooltip.gameObject.activeInHierarchy == true)
		{
			tooltip.gameObject.SetActive(false);
		}
	}

	public void ShowRecipe(int recipe, string name)
	{
		if (recipe == 1)
		{
			//Setting the UI text to the full formula
			Debug.Log(recipe1Inv + " / " + recipe1Req);
			recipe1.transform.GetChild(1).GetComponent<Text>().text = recipe1Inv + " / " + recipe1Req;

			recipe1.gameObject.SetActive(true);

			//Show the icon of the recipe item
			for (int k = 0; k < this.gameObject.GetComponent<ItemController>().items.Count; k++)
			{
				//Find the correct item
				if (gameObject.GetComponent<ItemController>().items[k].name == name)
				{
					recipe1.transform.GetChild(0).GetComponent<Image>().sprite = gameObject.GetComponent<ItemController>().items[k].icon.sprite;
				}
			}
		}

			if (recipe == 2)
			{
				//Setting the UI text to the full formula
				Debug.Log(recipe2Inv + " / " + recipe2Req);
				recipe2.transform.GetChild(1).GetComponent<Text>().text = recipe2Inv + " / " + recipe2Req;

				recipe2.gameObject.SetActive(true);

				//Show the icon of the recipe item
				for (int k = 0; k < this.gameObject.GetComponent<ItemController>().items.Count; k++)
				{
					//Find the correct item
					if (gameObject.GetComponent<ItemController>().items[k].name == name)
					{
						recipe2.transform.GetChild(0).GetComponent<Image>().sprite = gameObject.GetComponent<ItemController>().items[k].icon.sprite;
					}
				}

			}

			if (recipe == 3)
			{
				//Setting the UI text to the full formula
				Debug.Log(recipe3Inv + " / " + recipe3Req);
				recipe3.transform.GetChild(1).GetComponent<Text>().text = recipe3Inv + " / " + recipe3Req;

				recipe3.gameObject.SetActive(true);

				//Show the icon of the recipe item
				for (int k = 0; k < this.gameObject.GetComponent<ItemController>().items.Count; k++)
				{
					//Find the correct item
					if (gameObject.GetComponent<ItemController>().items[k].name == name)
					{
						recipe3.transform.GetChild(0).GetComponent<Image>().sprite = gameObject.GetComponent<ItemController>().items[k].icon.sprite;
					}
				}

			}
		}
	}

	

