using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableController : MonoBehaviour {

	[System.Serializable]
	public class Harvestable
	{
		[Header("General Info")]
		public int id;
		public string name;
		public string item;

		[Header("Objects")]
		public GameObject model;

		[Header("Attributes")]
		public float experience;
		public float density;
		public string prof;
	}

	public List<Harvestable> harvestables = new List<Harvestable>();
}
