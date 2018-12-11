using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : MonoBehaviour {


	//PUBLIC VARIABLES
	[Header("References")]
	public GameObject controller;

	[Header("Objects")]
	public GameObject item;

	[Header("Attributes")]
	public int maxGatherAmount;
	public int gatherAmount;
	public float replenishTimer;

	//PRIVATE VARIABLES
	InventoryController ic;
	GatherableController gc;

	#region Starting Values From List
	void Start () {
		ic = controller.GetComponent<InventoryController>();
		gc = controller.GetComponent<GatherableController>();
		
		//Check through the existing gatherables
		for (int i = 0; i < gc.gatherables.Count; i++)
		{
			//Check for a match with this gatherable in the controller
			if (this.gameObject.name == gc.gatherables[i].name)
		{
			//Take the variables from the controller
			maxGatherAmount = gc.gatherables[i].gatherAmount;
			replenishTimer = gc.gatherables[i].respawnTimer;
			item = gc.gatherables[i].item;

			//Set the gatherable amount to it's max
				gatherAmount = maxGatherAmount;
			}
		}	
	}
	#endregion


	#region Gathering Items
	public void GatherItem()
	{
		//Gather an item
		if (gatherAmount > 0)
		{
	
			//Check through the existing items
			for (int i = 0; i < gc.gatherables.Count; i++)
			{
				//Check for a match with this gatherable in the controller
				if (this.gameObject.name == gc.gatherables[i].name)
				{

					string name = item.name;
					//If there is an empty slot available, gather the item

						gatherAmount -= 1;
						int amount = 1;
						ic.AddItem(name, amount);

						//Start replenishing the gatherable up till it's maximum amount
						StartCoroutine(ReplenishTimer());
				}
			}
		}
	}
	#endregion



	#region Replenish Timer
	private IEnumerator ReplenishTimer()
	{
		float timeToWait = replenishTimer;
		yield return new WaitForSeconds(timeToWait);

		while (timeToWait > 0)
		{
			//Make sure it only runs once and reference the correct method
			if (gatherAmount < maxGatherAmount)
			{
				gatherAmount += 1;
				yield break;
			}

			
		}
	}
	#endregion
}
