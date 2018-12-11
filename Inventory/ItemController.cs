using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour {

	[System.Serializable]
	public class Item
	{
		[Header("General Info")]
		public int id;
		public string name;
		public string desc;
		public bool canHotbar;

		[Header("Objects")]
		public GameObject model;
		public Image icon;

		[Header("Stackables")]
		public bool isStackable;
		public int maxStack;

		[Header("Using")]
		public bool isUseable;
		public bool isDepletable;
		public float useTime;

		[Header("Swinging")]
		public bool isAction;
		public float swingRange;
		public float harvestEfficiency;
		public float killingEfficiency;
		public float swingSpeed;

		[Header("Consumption Values")]
		public float thirstValue;
		public float hungerValue;

		[Header("Crafting")]
		public bool isCraftable;
		public string category;
		public List<Recipes> recipes = new List<Recipes>();

		[Header("Equipment")]
		public bool isEquippable;
		public string equipmentType;
		public List<EquipmentEffects> equipmentEffects = new List<EquipmentEffects>();

	}

	//Equipment Effects
	public List<Item> items = new List<Item>();

	[System.Serializable]
	public class EquipmentEffects
	{
		public string name;
		public float value;
	}

	[System.Serializable]
	public class Recipes
	{
		public string name;
		public int amount;
	}

}
