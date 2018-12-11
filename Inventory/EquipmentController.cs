using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
	//PUBLIC VARIABLES
	[Header("References")]
	public GameObject player;

	//PRIVATE VARIABLES
	PlayerAttributes pat;

	#region Adding Equipment Effects to the Player
	public void AddEquipEffect(string name)
	{
		player = GameObject.Find("Character");
		pat = player.GetComponent<PlayerAttributes>();

		//Check through the existing types
		for (int k = 0; k < this.gameObject.GetComponent<ItemController>().items.Count; k++)
		{
			//Check for a match in type
			if (name == this.gameObject.GetComponent<ItemController>().items[k].name)
			{
				Debug.Log("Added Equipment Effects [EC: 28]");
				//Check for effects
				for (int i = 0; i < this.gameObject.GetComponent<ItemController>().items[k].equipmentEffects.Count; i++)
				{

					for (int s = 0; s < pat.equippedEffects.Count; s++)
					{
						if (pat.equippedEffects[s].name == this.gameObject.GetComponent<ItemController>().items[k].equipmentEffects[i].name)
						{
							pat.equippedEffects[s].value = player.GetComponent<PlayerAttributes>().equippedEffects[s].value 
								+ this.gameObject.GetComponent<ItemController>().items[k].equipmentEffects[i].value;

						}
					}


				}
			}
		}
	}
	#endregion


	#region Removing Equipment Effects from the Player
	public void RemoveEquipEffect(GameObject item)
	{
		player = GameObject.Find("Character");
		pat = player.GetComponent<PlayerAttributes>();

		//Check through the existing types
		for (int k = 0; k < this.gameObject.GetComponent<ItemController>().items.Count; k++)
		{
			//Check for a match in type
				if (item.name == this.gameObject.GetComponent<ItemController>().items[k].name)
				{
					//Remove the effects from the sorted item
					for (int i = 0; i < this.gameObject.GetComponent<ItemController>().items[k].equipmentEffects.Count; i++)
					{

						for (int s = 0; s < pat.equippedEffects.Count; s++)
						{
							if (pat.equippedEffects[s].name == this.gameObject.GetComponent<ItemController>().items[k].equipmentEffects[i].name)
							{
								pat.equippedEffects[s].value = player.GetComponent<PlayerAttributes>().equippedEffects[s].value
								- gameObject.GetComponent<ItemController>().items[k].equipmentEffects[i].value;
							}


						}

				}
			}
		}
	}
	#endregion
}