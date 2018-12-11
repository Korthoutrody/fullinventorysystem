using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesController : MonoBehaviour {

	[System.Serializable]
	public class Attribute
	{
		[Header("General Info")]
		public int id;
		public string name;

		[Header("Attributes")]
		public float value;
		public float charge;
	}

	public List<Attribute> attributes = new List<Attribute>();
}
