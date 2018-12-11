using System.Collections.Generic;
using UnityEngine;

public class PlayerProfs: MonoBehaviour {

	//PUBLIC VARIABLES
	[Header("References")]
	public GameObject controller;

	[System.Serializable]
	public class Profs
	{
		public int id;
		public string name;

		public int level;
		public float experience;
	}

	public List<Profs> profs = new List<Profs>();


	//PRIVATE VARIABLES
	ProfsTables pt;

	void Start()
	{
		//Script references
		pt = controller.GetComponent<ProfsTables>();

	}

	#region Levelling Up
	public void LevelUp(string name, string prof)
	{
		//Check through the existing items
		for (int i = 0; i < profs.Count; i++)
		{
			//Check for a match with this gatherable in the playerprofs
			if (prof == profs[i].name)
			{
				//Level-Up
				profs[i].level += 1;
			}
		}

	}
	#endregion

	#region Gaining Experience
	public void GainExperience(string name, float exp, string prof)
	{
		//Check through the existing items
		for (int i = 0; i < profs.Count; i++)
		{
			//Check for a match with this gatherable in the playerprofs
			if (prof == profs[i].name)
			{
				//Add experience to the profficiency
				profs[i].experience += exp;

				//Check experience needed to level-up
				int level = profs[i].level;
				if (profs[i].experience >= pt.experienceTable[level + 1])
				{
					LevelUp(name, prof);
				}
			}

		}
	}
	#endregion
}
