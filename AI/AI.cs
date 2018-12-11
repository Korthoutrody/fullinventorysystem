using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

	//PUBLIC VARIABLES
	[Header("References")]
	public GameObject controller;

	[Header("Attributes")]
	public float health;


	//PRIVATE VARIABLES
	private float maxHealth;

	void Start () {
		//Check through the existing harvestables
		for (int i = 0; i < controller.GetComponent<AIController>().ai.Count; i++)
		{
			//Check for a match with this harvestable in the controller
			if (this.gameObject.name == controller.GetComponent<AIController>().ai[i].name)
			{
				maxHealth = controller.GetComponent<AIController>().ai[i].health;
				health = maxHealth;

			}
		}
	}
	
	void Update () {
		
	}

	public void DamageTaken(float efficiency)
	{
		if (health - efficiency > 0)
		{
			health -= efficiency;
			Debug.Log(health);

		}

		else
		{
			Death();
		}
	}
	public void Death()
	{
		Destroy(this.gameObject);
	}
}
