using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	[System.Serializable]
	public class AI
	{
		[Header("General Info")]
		public int id;
		public string name;
		public GameObject model;

		[Header("Attributes")]
		public float health;
	}

	public List<AI> ai = new List<AI>();
}
