using UnityEngine.UI;
using UnityEngine;

public class ItemStacks : MonoBehaviour {

	void Start()
	{
		if (GetComponent<ItemAttributes>().amount == 1)
		{
			transform.Find("Amount").GetComponent<Text>().text = "";
		}
	}
	void Update()
	{

	}
	public void ChangeAmount(int amount)
	{
		transform.Find("Amount").GetComponent<Text>().text = amount.ToString();
	}
}
