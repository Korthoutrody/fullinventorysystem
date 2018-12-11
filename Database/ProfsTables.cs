using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfsTables : MonoBehaviour {

	//PUBLIC VARIABLES
	[Header("References")]
	public GameObject controller;

	[Header("Experience Table")]
	public float[] experienceTable;

	[Header("Multiplier Table")]
	public float[] multiplier;


	//PRIVATE VARIABLES
	private float multiIncrease = 0.05f;
	private float expIncrease = 1.25f;

	void Start ()
	{
		//Mathematically make the experience table based on exponential increase
		experienceTable[1] = 75f;
		for (int i = 2 ; i < experienceTable.Length; i++)
		{
			experienceTable[i] = Mathf.Round(experienceTable[i-1] * expIncrease);

		}

		//Mathematically make the multiplier table based on incremental increase
		multiplier[0] = 1.00f;
		for (int i = 1; i < multiplier.Length; i++)
		{
			multiplier[i] = multiplier[i-1] + multiIncrease;
		}
	}
}
