using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildablesList : MonoBehaviour {

	[System.Serializable]
	public class Buildable
	{
		[Header("General Info")]
		public int id;
		public string name;
		public int category;
		public int posInCat;

		[Header("Objects")]
		public Image icon;
		public GameObject model;

	}

	public List<Buildable> buildables = new List<Buildable>();
}
