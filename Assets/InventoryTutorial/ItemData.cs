using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler  {

	public Item item;
	public int amount;
	public int slotNum;

//	private Transform originalParent;
	private Inventory inventory;
	private Vector2 offset;

	private ToolTip tooltip;

	void Start() {
		inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Inventory> ();
		tooltip = inventory.GetComponent<ToolTip> ();
	}

	public void OnPointerDown(PointerEventData eventData) {
		// Check if item exists
		if (item != null) {
			offset = eventData.position - (Vector2)this.transform.position;
//			originalParent = this.transform.parent; // Store orignal parent
			this.transform.SetParent (this.transform.parent.parent); // set parent to invetory slot so it appears above slots
			this.transform.position = eventData.position - offset; // Set Position
			GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	public void OnDrag (PointerEventData eventData) {
		if (item != null) {
			this.transform.position = eventData.position - offset; // Move item
		}
	}
		
	public void OnEndDrag (PointerEventData eventData) {
		this.transform.SetParent (inventory.slots[slotNum].transform); // Reset parent
		this.transform.position = inventory.slots[slotNum].transform.position; // reset to parent's position
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	public void OnPointerEnter (PointerEventData eventData) {
		tooltip.Activate (item);
	}

	public void OnPointerExit (PointerEventData eventData) {
		tooltip.Deactivate ();
	}
}
