using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


#region Button Class Format
//The format for the list of buttons that populate the Right Click Menu
[System.Serializable]
public class RightClickMenuButton
{
	public string text;             // Button Text
	public Button button;           // Template Button
	public Action<Image> action;    // Delegate to method that needs to be executed when button is clicked


	//Required to populate the Right Click Menu with buttons
	public RightClickMenuButton(string text, Button button, Action<Image> action)
	{
		this.text = text;
		this.button = button;
		this.action = action;
	}
}
#endregion

public class RightClickMenu : MonoBehaviour, IPointerClickHandler
{
	//PUBLIC VARIABLES
	[Header("References")]
	public Image menu;              // The menu itself in UI-format
	public Canvas canvas;           // The parent for the menu above

	#region Instancing the Right Click Menu
	private static RightClickMenu instance;    // Private Singleton for creating new Right Click Menu's
	public static RightClickMenu Instance	   // The Publicly referenced instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType(typeof(RightClickMenu)) as RightClickMenu;
				if (instance == null)
				{
					instance = new RightClickMenu();
				}
			}
			return instance;
		}
	}
	#endregion
	

	#region Pointer Click To Destroy Menu
	readonly PointerEventData p;             // Tracking the data for Pointer Events

	//Destroying the Right Click Menu when having clicked on one of it's buttons
	public void OnPointerClick(PointerEventData p)
	{
		//Either by Right or Left clicking
		if (p.button == PointerEventData.InputButton.Right && GameObject.Find("RightClickMenu")
			|| p.button == PointerEventData.InputButton.Left && GameObject.Find("RightClickMenu"))
		{
			Destroy(GameObject.Find("RightClickMenu"));
		}

	}
	#endregion


	#region Creating The Right Click Menu
	public void CreateRightClickMenu(List<RightClickMenuButton> items, Vector2 position)
	{

			//Create a Right Click Menu if one isn't currently displayed
			if (menu != null)
			{
				Image panel = Instantiate(menu, new Vector2(position.x, position.y), Quaternion.identity) as Image;
				panel.transform.SetParent(canvas.transform);
				panel.transform.SetAsLastSibling();
				panel.name = "RightClickMenu";

				//Format and create all the relevant buttons for the Right Click Menu
				foreach (var item in items)
			{
				//Temporary listener

				RightClickMenuButton temp = item;

				//Instantiate
				Button button = Instantiate(item.button) as Button;

				//Handle the text
				Text buttonText = button.GetComponentInChildren(typeof(Text)) as Text;
				buttonText.text = item.text;

				//The listener for clicking the button
				button.onClick.AddListener(delegate { temp.action(panel); });

				//Formatting
				button.transform.SetParent(panel.transform);
				button.image.rectTransform.sizeDelta = new Vector2(30, 15);
				}
			}
		}
	}
	#endregion
