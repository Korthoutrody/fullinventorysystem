using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction: MonoBehaviour {

	//PUBLIC VARIABLES
	[Header("References")]
	public GameObject controller;
	public Camera cam;

	[Header("Attributes")]
	public float intRange = 8.0f;

	[Header("Keys")]
	public KeyCode interact;
	public KeyCode swing;

	//PRIVATE VARIABLES
	private float maxIntRange = 15.0f; //Check a raycast that's far enough to detect all collider tags

	InventoryController ic;
	PlayerAttributes pat;
	PlayerActions pac;
	
	void Start () {

		//SCRIPT COMPONENTS
		ic = controller.GetComponent<InventoryController>();
		pat = gameObject.GetComponent<PlayerAttributes>();
		pac = gameObject.GetComponent<PlayerActions>();

	}

	void Update () {
		
		//The raycast with the camera at the middle point of the screen
		Ray rayCast = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
		RaycastHit rayHit;


		//Check with a further raycasthit to find out the collider tag that's required to know the raycasthit of the equipped item
		if (Physics.Raycast(rayCast, out rayHit, maxIntRange))
		{

			#region Raycast Interaction with Items and Gatherables
			//Checking the collision tags for interaction
			if (rayHit.collider.tag == "Item" || rayHit.collider.tag == "Gatherable")
			{
				//Interaction range with items and gatherables
				if (Physics.Raycast(rayCast, out rayHit, intRange))
				{
					//Interacting with items
					if (Input.GetKeyDown(interact) && rayHit.collider.tag == "Item")
					{
						string hitName = rayHit.collider.gameObject.name;
						int amount = rayHit.collider.gameObject.GetComponent<ItemEntity>().amount;


						Debug.Log("Interacting with" + hitName + "[PlayerInteraction: 58]");
						ic.AddItem(hitName, amount);

						if (InventoryController.sorted)
						{
							Destroy(rayHit.collider.gameObject);
						}

						else
						{
							rayHit.collider.gameObject.GetComponent<ItemEntity>().amount = InventoryController.sortedValue;
						}
								
					}

					//Interacting with gatherables
					else if (Input.GetKeyDown(interact) && rayHit.collider.tag == "Gatherable")
					{
						GameObject hit = rayHit.collider.gameObject;
						Debug.Log("Interacting with gatherable [PlayerInteraction: 67]");
						pac.GatherByInteract(hit);
					}
				}
			}
			#endregion

			#region Raycast Interaction with Harvestables and AI
			//Checking the collision tags for swinging
			else if (rayHit.collider.tag == "Harvestable" || rayHit.collider.tag == "AI")
			{
				//Check if slot has an equipped item
				if (ic.selectedSlot != null && ic.selectedSlot.childCount > 0)
				{
					//Script referencing
					GameObject item = ic.selectedSlot.GetChild(0).gameObject;
					ItemAttributes ia = ic.selectedSlot.GetChild(0).gameObject.GetComponent<ItemAttributes>();

					if (ia.isAction)  //If the item has it's own raycast range

					{
						float swingRange = ia.swingRange; //Use the Swing Range for the Raycast range

								if (Physics.Raycast(rayCast, out rayHit, swingRange))
								{

									//Interacting with depletables
									if (Input.GetKeyDown(swing) && rayHit.collider.tag == "Harvestable" && pat.isExhausted == false)
									{
										//Set resting to false (For Stamina/Fatigue)
										pat.isResting = false;

										Debug.Log("Interacting with harvestable [PlayerInteraction: 100]");
										//The variables to bring to next function in the player actions script
										GameObject hit = rayHit.collider.gameObject;
										pac.HarvestBySwing(hit, item);
									}

									//Interacting with AI
									if (Input.GetKeyDown(swing) && rayHit.collider.tag == "AI" && pat.isExhausted == false)
									{
										//Set resting to false (Later it will be set false before the animation starts till it hits)
										pat.isResting = false;
										Debug.Log("Interacting with AI [PlayerInteraction: 112]");

										//The variables to bring to next function in the player actions script
										GameObject hit = rayHit.collider.gameObject;
										pac.HitBySwing(hit, item);
									}
								}
							}
						}
					}
				}
			}
			#endregion
		}

