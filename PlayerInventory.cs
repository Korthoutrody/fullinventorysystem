using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

	[System.Serializable]
	public class PlayerInvItems
	{
		[Header("General Info")]
		public string name;
		public int count;

		public PlayerInvItems(string newName, int newCount)
		{
			name = newName;
			count = newCount;
			
		}

	}

	public List<PlayerInvItems> playerInvItems = new List<PlayerInvItems>();
}
