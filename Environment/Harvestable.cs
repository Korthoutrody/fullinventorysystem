using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour {

	//PUBLIC VARIABLES
	[Header("References")]
	public GameObject controller;
	public GameObject player;

	[Header("Attributes")]
	public float density;
	public float maxDensity;

	//PRIVATE VARIABLES
	HarvestableController hc;
	PlayerProfs pp;
	ProfsTables pt;


	#region Start Values From List
	void Start()
	{
		hc = controller.GetComponent<HarvestableController>();
		pt = controller.GetComponent<ProfsTables>();
		pp = player.GetComponent<PlayerProfs>();

		//Check through the existing harvestables
		for (int i = 0; i < hc.harvestables.Count; i++)
		{
			//Check for a match with this harvestable in the controller
			if (this.gameObject.name == hc.harvestables[i].name)
			{
				maxDensity = hc.harvestables[i].density;
				density = maxDensity;


			}
		}
	}
	#endregion


	#region Handle Swing Taken
	public void SwingTaken(float efficiency, string name)
	{
		//Check through the existing harvestables
		for (int i = 0; i < hc.harvestables.Count; i++)
		{
			//Check for a match with this harvestable in the controller
			if (name == hc.harvestables[i].name)
			{
				//Check the level of the profficiency
				string prof = hc.harvestables[i].prof;
				for (int k = 0; k < pp.profs.Count; k++)
				{

					if (prof == pp.profs[k].name)
					{
						//Find out the speed multiplier of the player's level
						int level = pp.profs[k].level;
						float multi = pt.multiplier[level];

						if (density - (efficiency * multi) > 0)
						{
							density -= (efficiency * multi);
							Debug.Log(density);

						}

						else
						{
							CalculateExperience();
							FallOver();
						}
					}
				}
			}


		}
	}
	#endregion

	public void FallOver()
	{
		Debug.Log("The tree falls");
	}

	#region Calculating The Gained Experience
	public void CalculateExperience()
	{
		//Check through the existing items
		for (int i = 0; i < hc.harvestables.Count; i++)
		{
			//Check for a match with this harvastable in the controller
			if (this.gameObject.name == hc.harvestables[i].name)
			{
				float exp = hc.harvestables[i].experience;
				string prof = hc.harvestables[i].prof;
				string name = this.gameObject.name;

				pp.GainExperience(name, exp, prof);

			}

		}

	}
	#endregion
}
