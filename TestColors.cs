using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColors : MonoBehaviour {


	// Use this for initialization
	void Start () {
		Renderer rend = this.gameObject.GetComponent<Renderer>();

		//Test colors for unequipped items
		if (this.gameObject.name == "TestWeapon")
		{
			rend.material.SetColor("_Color", Color.red);
		}

		if (this.gameObject.name == "TestTool")
		{
			rend.material.SetColor("_Color", Color.blue);
		}


		if (this.gameObject.name == "TestUse")
		{
			rend.material.SetColor("_Color", Color.green);
		}

		if (this.gameObject.name == "TestHolder")
		{
			rend.material.SetColor("_Color", Color.yellow);
		}

		//Test colors for equipped items
		if (this.gameObject.name == "TestEquipHead")
		{
			rend.material.SetColor("_Color", Color.gray);
		}
		if (this.gameObject.name == "TestEquipHead2")
		{
			rend.material.SetColor("_Color", Color.gray);
		}

		if (this.gameObject.name == "TestEquipChest")
		{
			rend.material.SetColor("_Color", Color.gray);
		}

		if (this.gameObject.name == "TestEquipLegs")
		{
			rend.material.SetColor("_Color", Color.gray);
		}
		if (this.gameObject.name == "TestEquipHands")
		{
			rend.material.SetColor("_Color", Color.gray);
		}

		if (this.gameObject.name == "TestEquipFeet")
		{
			rend.material.SetColor("_Color", Color.gray);
		}

		//Test colors for interacts
		if (this.gameObject.name == "TestGather")
		{
			rend.material.SetColor("_Color", Color.green);
		}

		if (this.gameObject.name == "TestHarvest")
		{
			rend.material.SetColor("_Color", Color.gray);
		}

		if (this.gameObject.name == "TestAI")
		{
			rend.material.SetColor("_Color", Color.red);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
