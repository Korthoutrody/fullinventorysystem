using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableController : MonoBehaviour {

	[System.Serializable]
	public class Gatherable
	{
		[Header("General Info")]
		public int id;
		public string name;

		[Header("Objects")]
		public GameObject model;
		public GameObject item;

		[Header("Attributes")]
		public float timeToGather;
		public float respawnTimer;
		public int gatherAmount;
		public float experience;


	}

	public List<Gatherable> gatherables = new List<Gatherable>();
}
