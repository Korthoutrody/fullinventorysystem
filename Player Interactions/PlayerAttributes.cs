using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerAttributes : MonoBehaviour
{

	//PUBLIC VARIABLES
	[Title("Current Attributes")]
	public float vHunger;
	public float vThirst;
	public float vStamina;

	[Title("Equipment Effects")]
	[System.Serializable]
	public class EquippedEffect
	{
		public string name;
		public float value;
	}
	public List<EquippedEffect> equippedEffects = new List<EquippedEffect>();

	[Title("Charge & Drain")]
	public float hungerDrain;
	public float thirstDrain;
	public float staminaCharge;

	[Title("States")]
	public bool isFatigued;
	public bool isExhausted;
	public bool isResting;

	[Title("Maximum values")]
	public float maxThirst;
	public float maxHunger;
	public float maxStamina;

	[FoldoutGroup("State Variables")]
	public float fatigueTimer;
	[FoldoutGroup("State Variables")]
	public float exhaustionTimer;
	[FoldoutGroup("State Variables")]
	public float maxFatigue;
	[FoldoutGroup("State Variables")]
	public float startFatigue;
	[FoldoutGroup("State Variables")]
	public float maxExhaustion;
	[FoldoutGroup("State Variables")]
	public float fatiguedResult;

	[FoldoutGroup("References")]
	public GameObject controller;

	//PRIVATE VARIABLES
	AttributesController ac;

	void Start()
	{
		//Script reference
		ac = controller.GetComponent<AttributesController>();

		#region Starting Values For Variables And Attributes
		//Starting Values for timers
		fatigueTimer = startFatigue;
		exhaustionTimer = maxExhaustion;

		//Start with the correct values of the attributes
		for (int i = 0; i < ac.attributes.Count; i++)
		{
			if (ac.attributes[i].name == "hunger")
			{
				hungerDrain = ac.attributes[i].charge;
				maxHunger = ac.attributes[i].value;
				vHunger = maxHunger;
			}

			if (ac.attributes[i].name == "thirst")
			{
				thirstDrain = ac.attributes[i].charge;
				maxThirst = ac.attributes[i].value;
				vThirst = maxThirst;
			}

			if (ac.attributes[i].name == "stamina")
			{
			    staminaCharge = ac.attributes[i].charge;
				maxStamina = ac.attributes[i].value;
				vStamina = maxStamina;
			}
		}
		#endregion
	}

	void Update()
	{
		#region Changing, Draining and Charging Attributes
		//Becoming hungry
		if (vHunger > 0)
		{
			HungerDrain();
		}

		//Becoming thirsty
		if (vThirst > 0)
		{
			ThirstDrain();
		}

		//Recharging stamina
		if (vStamina < 100 && vStamina > 0 && isResting && !isFatigued && !isExhausted)
		{
			StaminaCharge();
		}

		//Becoming fatigued
		if (vStamina <= 0 && !isFatigued)
		{
			vStamina = 0;
			fatigueTimer = startFatigue;
			isFatigued = true;
		}
		#endregion


		#region Handle Being Fatigued
		if (isFatigued)
		{
			//If the player does not rest, the player will become exhausted
			if (!isResting)
			{
				fatigueTimer += Time.deltaTime;

				//Becoming exhausted
				if (fatigueTimer >= maxFatigue)
				{
					isExhausted = true;
					isFatigued = false;
					return;
				}

			}
			//If the player is resting, the player will become unfatigued
			else
			{
				fatigueTimer -= Time.deltaTime;

				//Becoming unfatigued
				if (fatigueTimer <= 0)
				{
					vStamina = 0.01f; //Make sure the fatigue timer does not loop with the isFatigued bool at 0
					fatigueTimer = 0;
					isFatigued = false;
					return;
				}

			}
		}
		#endregion

		#region Handle Being Exhausted
		if (isExhausted)
		{
			fatigueTimer = startFatigue;
			exhaustionTimer -= Time.deltaTime;

			//Stop being exhausted after a set amount of time
			if (exhaustionTimer <= 0)
			{
				Debug.Log("No longer exhausted");
				isExhausted = false;
				isFatigued = true;
				exhaustionTimer = maxExhaustion;
				return;
			}
		}
		#endregion

	}


	#region Handling Useage of Items
	//Handle any item Useage
	public void HandleUseage(float thirstValue, float hungerValue)
	{
		//No need to call the function if there is no increase or decrease in value
		if (thirstValue != maxThirst)
		{
			Drinking(thirstValue);
		}

		if (hungerValue != maxHunger)
		{
			Eating(hungerValue);
		}
	}
	#endregion

	#region Eating
	//Eating to restore hunger
	private void Eating(float hungerValue)
	{
		//If the used item has a negative value
		if (hungerValue < 0)
		{
			//Set to 0 if it reaches below the limits
			if ((vHunger + hungerValue) <= 0)
			{
				vHunger = 0;
			}

			else
			{
				//Add to the hunger value of the player
				vHunger += hungerValue;
			}
		}

		//If the used item is a positive value
		else
		{
			//Set to the maximum value if it reaches above the limits
			if ((vHunger + hungerValue) >= maxHunger)
			{
				vHunger = maxHunger;
			}

			else
			{
				//Add to the hunger value of the player
				vHunger += hungerValue;
			}
		}
	}
	#endregion

	#region Drinking
	//Drinking to restore thirst
	private void Drinking(float thirstValue)
	{
		//If the used item has a negative value
		if (thirstValue < 0)
		{
			//Set to 0 if it reaches below the limits
			if ((vThirst + thirstValue) < 0)
			{
				vThirst = 0;
			}

			else
			{
				//Add to the thirst value of the player
				vThirst += thirstValue;
			}
		}
		else
		{
			//Set to the maximum value if it reaches above the limits
			if ((vThirst + thirstValue) >= maxThirst)
			{
				vThirst = maxThirst;
			}

			else
			{
				//Add to the thirst value of the player
				vThirst += thirstValue;
			}
		}
	}
	#endregion


	#region Draining and Charging Attributes
	//Drain hunger
	private void HungerDrain()
	{
		//Draining 
		vHunger = vHunger - (hungerDrain * Time.deltaTime);

		//Make sure it only runs till hunger is 0
		if (vHunger <= 0)
			{
				vHunger = 0;
				return;
			}


	}

	//Drain thirst
	private void ThirstDrain()
	{
		//Draining 
		vThirst = vThirst - (thirstDrain * Time.deltaTime);

		//Make sure it only runs till thirst is 0
		if (vThirst <= 0)
		{
			vThirst = 0;
			return;
		}

	}

	//Handle the recharge of stamina
	public void StaminaCharge()
	{
		//Draining 
		vStamina = vStamina + (staminaCharge * Time.deltaTime);

		//Make sure it only runs till hunger is 0
		if (vStamina >= 100)
		{
			vStamina = 100;
			return;
		}
		//Handling the recharge of stamina while idle or walking
	}



	public void HandleStamina(float loss)
	{
		if ((vStamina - loss) <= 0)
		{
			vStamina = 0;
		}
		else
		{
			vStamina -= loss;
		}
		return;
		//Handling the loss of stamina through actions and movement
	}
	#endregion
}
