using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragging : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	public static bool hasBeenEquipped;		//To ensure that equipped item effects are removed when the item is dragged out of the equipment slot
	public static GameObject itemBeingDragged;
	public Vector3 startPosition;			//The position the dragging starts
	public Transform startParent;			//The parent when the dragging starts
	public static Transform sortSlot;		//Anti-UI glitches

	private GameObject controller;

	void Start()
	{
		controller = GameObject.Find("controller");

	}

	#region Dragging Items
	public void OnBeginDrag(PointerEventData p)
	{
		//Show tooltip
		controller.GetComponent<TooltipController>().ShowTooltip(gameObject);

		//Set the starting position, parent and dragged item
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;


		//To ensure that equipped item effects are removed when the item is dragged out of the equipment slot
		if (startParent.parent.name == "Equipment")
		{
			hasBeenEquipped = true;
		}

		else
		{
			hasBeenEquipped = false;
		}

		if (GameObject.Find("RightClickMenu"))
		{
			Destroy(GameObject.Find("RightClickMenu"));
		}

		//Anti-UI glitching
		GetComponent<CanvasGroup>().blocksRaycasts = false;
		sortSlot = GameObject.Find("SortSlot").transform;
		transform.SetParent(sortSlot);

	}


	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("On End of Drag");
		//Resets
		itemBeingDragged = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;

		//Return the item if not dropped in a slot

		if (transform.parent == sortSlot)
		{
			transform.position = startPosition;
			transform.SetParent(startParent);

		}

	}
	#endregion

	#region Tooltips With Pointers
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (itemBeingDragged == null)
		{
			controller.GetComponent<TooltipController>().ShowTooltip(gameObject);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (itemBeingDragged == null)
		{
			controller.GetComponent<TooltipController>().HideTooltip();
		}
	}
	#endregion
}
