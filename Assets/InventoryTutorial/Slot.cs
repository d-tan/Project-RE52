using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

	public int slotID;

	private Inventory inventory;


	void Start() {
		inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Inventory>();
	}

	public void OnDrop (PointerEventData eventData) {
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> ();

		if (inventory.items [slotID].ID == -1) {
			
			inventory.items [droppedItem.slotNum] = new Item ();
			inventory.items [slotID] = droppedItem.item;
			droppedItem.slotNum = this.slotID;


		} else if (droppedItem.slotNum != slotID) {
			
			Transform item = this.transform.GetChild (0);
			item.GetComponent<ItemData> ().slotNum = droppedItem.slotNum;
			item.transform.SetParent(inventory.slots[droppedItem.slotNum].transform);
			item.transform.position = inventory.slots [droppedItem.slotNum].transform.position;

			droppedItem.slotNum = slotID;
			droppedItem.transform.SetParent (this.transform);
			droppedItem.transform.position = this.transform.position;

			inventory.items [droppedItem.slotNum] = item.GetComponent<ItemData> ().item;
			inventory.items [slotID] = droppedItem.item;
		}
	}
}
