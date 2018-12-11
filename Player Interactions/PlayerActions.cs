using System.Collections;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{

	//PUBLIC VARIABLES
	[Header("References")]
	public GameObject controller;
	public Camera cam;

	[Header("References")]
	private bool stillSwinging;
	private bool actionAborted;

	//PRIVATE VARIABLES
	InventoryController ic;
	GatherableController gc;
	PlayerInteraction pi;
	PlayerAttributes pat;
	PlayerMovement pm;

	#region Start Values
	void Start()
	{
		//Start Values
		stillSwinging = false;
		actionAborted = false;

		ic = controller.GetComponent<InventoryController>();
		gc = controller.GetComponent<GatherableController>();

		pi = gameObject.GetComponent<PlayerInteraction>();
		pat = gameObject.GetComponent<PlayerAttributes>();
		pm = gameObject.GetComponent<PlayerMovement>();
	}
	#endregion

	void Update()
	{

		#region Swing Abortion Criteria
		//Aborting the action when moving or looking away
		Ray rayCast = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
		RaycastHit rayHit;

		//Check with a further raycasthit to find out the collider tag
		if (Physics.Raycast(rayCast, out rayHit, pi.intRange))
		{
			//Checking if the player is still looking at one of the interactive objects
			if (rayHit.collider.tag != "Gatherable" || rayHit.collider.tag != "Harvestable" || rayHit.collider.tag != "AI")
			{
				actionAborted = true;
			}
		}

		//Checking if the player has started moving
		if (pm.playerIsMoving == true)
		{
			actionAborted = true;
		}
	}
	#endregion

	#region Using Items For Consumption
	public void UseItem(GameObject item)
	{
		//Script Reference
		ItemAttributes ia = item.GetComponent<ItemAttributes>();

				float thirstValue = ia.thirstValue;
				float hungerValue = ia.hungerValue;

				pat.HandleUseage(thirstValue, hungerValue);
	}
	#endregion


	#region Hitting AI By Swinging
	public void HitBySwing(GameObject hit, GameObject item)
	{
		//Script Reference
		ItemAttributes ia = item.GetComponent<ItemAttributes>();

		if (ic.selectedSlot != null)
		{
			if (ic.selectedSlot.childCount > 0)
			{
				string weapon = ic.selectedSlot.GetChild(0).gameObject.name; //The equipped weapon
				float swingSpeed;											 //The weapon's swing speed
				float efficiency = ia.killingEfficiency;					 //The weapon's strength
				string actionType = "Combat";                                //To let the coroutine know the next function to call

				//If player is fatigued slow down the swing speed
				if (pat.isFatigued)
				{
					swingSpeed = ia.swingSpeed * pat.fatiguedResult;
				}
				else //otherwise, take the regular swing speed
				{
					swingSpeed = ia.swingSpeed;
				}

				if (stillSwinging == false) // The previous swing time must have been past
				{
					Debug.Log("Swinging at AI [PlayerActions: 113]");
					StartCoroutine(SwingTimer(swingSpeed, actionType, efficiency, hit)); //Start the Timer
				}
			}
		}
	}
	#endregion

	#region Harvesting By Swinging
	public void HarvestBySwing(GameObject hit, GameObject item)
	{
		//Script Reference
		ItemAttributes ia = item.GetComponent<ItemAttributes>();

		if (ic.selectedSlot != null)
		{
			if (ic.selectedSlot.childCount > 0)
			{

				string tool = ic.selectedSlot.GetChild(0).gameObject.name;	//The equipped tool
				float swingSpeed;                                           //The weapon's swing speed
				float efficiency = ia.harvestEfficiency;					//The tool's efficiency
				string actionType = "Harvesting";                           //To let the coroutine know the next function to call


				//If player is fatigued slow down the swing speed
				if (pat.isFatigued)
				{
					swingSpeed = ia.swingSpeed * pat.fatiguedResult;
				}
				else //otherwise, take the regular swing speed
				{
					swingSpeed = ia.swingSpeed;
				}

				if (stillSwinging == false) // The previous swing time must have been past
				{
					Debug.Log("Swinging at Harvestable [PlayerActions: 150]");
					StartCoroutine(SwingTimer(swingSpeed, actionType, efficiency, hit)); //Start the Timer
				}
			}
		}
	}
	#endregion


	#region Gathering By Interacting
	public void GatherByInteract(GameObject hit)
	{
		//Check through the existing gatherables
		for (int i = 0; i < gc.gatherables.Count; i++)
		{
			//Check for a match with interacted gatherable
			if (hit.name == gc.gatherables[i].name)
			{
				bool ranOnce = false;
				string actionType = "Gathering";
				float timeToGather = gc.gatherables[i].timeToGather;

				//Start the gather timer when the gatherable object is not depleted
				if (hit.GetComponent<Gatherable>().gatherAmount > 0)
				{
					if (ic.slotLeft.childCount == 0 ||
						ic.slotRight.childCount == 0)
					{
						StartCoroutine(ActionTimer(ranOnce, timeToGather, actionType, hit));
					}
				}

			}
		}
	}
	public void GatherItem(GameObject hit)
	{
		hit.GetComponent<Gatherable>().GatherItem();
	}
	#endregion


	#region Timer For Swinging and Stamina Loss
	private IEnumerator SwingTimer(float timeToWait, string actionType, float efficiency, GameObject hit)
	{
		stillSwinging = true;							//Can no longer swing
		yield return new WaitForSeconds(timeToWait);	//Return after this time, simulating the swing speed

		while (timeToWait > 0)
		{
			//Make sure it only runs once and reference the correct method
			{
				float loss = 50;			//Stamina loss from swinging
				pat.HandleStamina(loss);	//Handle the stamina at player attributes

				if (actionType == "Harvesting") //If harvesting, refer to the Harvestable
				{
					Debug.Log("Completed Swinging at Harvestable [PlayerActions: 207]");
					hit.GetComponent<Harvestable>().SwingTaken(efficiency, hit.name);
				}

				else if (actionType == "Combat") //If in combat, refer to the AI
				{
					Debug.Log("Completed Swinging at AI [PlayerActions: 213]");
					hit.GetComponent<AI>().DamageTaken(efficiency);
				}

				pat.isResting = true; //(For Stamina/Fatigue)
				stillSwinging = false; //Can Swing again
				yield break;

			}


		}
	}
	#endregion


	#region Action Timer for Gathering
	private IEnumerator ActionTimer(bool ranOnce, float timeToWait, string actionType, GameObject hit)
	{
		actionAborted = false; 
		yield return new WaitForSeconds(timeToWait);

		while (timeToWait > 0)
		{
			//Stop the action when it has been aborted by movement or looking away
			if (actionAborted == true)
			{
				yield break;
			}

			//Make sure it only runs once and reference the correct method
			if (actionType == "Gathering" && !ranOnce)
			{
				ranOnce = true;
				GatherItem(hit);
				yield break;
			}
		}
	}
	#endregion
}

