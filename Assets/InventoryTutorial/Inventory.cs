using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	GameObject inventoryPanel;
	GameObject slotPanel;
	ItemDatabase database;
	public GameObject inventorySlot;
	public GameObject inventoryItem;

	public List<Item> items = new List<Item>();
	public List<GameObject> slots = new List<GameObject>();

	int slotAmount;

	void Start() {
		slotAmount = 30;
		inventoryPanel = GameObject.FindGameObjectWithTag ("InventoryPanel");
		slotPanel = inventoryPanel.transform.FindChild ("SlotPanel").gameObject;
		database = GetComponent<ItemDatabase> ();

		for (int i = 0; i < slotAmount; i++) {
			items.Add (new Item ());
			slots.Add (Instantiate (inventorySlot));
			slots [i].GetComponent<Slot> ().slotID = i;
			slots [i].transform.SetParent(slotPanel.transform);
		}

		AddItem (0);
		AddItem (0);
		AddItem (0);
		AddItem (0);
		AddItem (0);
		AddItem (1);
		AddItem (2);
	}


	// Add Item to inventory
	public void AddItem(int id) {
		Item itemToAdd = database.FetchItemByID(id);

		if (itemToAdd.Stackable && CheckItemInInventory (itemToAdd)) {
			for (int i = 0; i < items.Count; i++) {
				if (items [i].ID == id) {
					ItemData data = slots [i].transform.GetChild (0).GetComponent<ItemData> ();

					data.amount++;
					data.transform.GetChild (0).GetComponent<Text> ().text = data.amount.ToString ();
					break;
				}
			}
		} else {

			// Loop through each slot of inventory
			for (int i = 0; i < items.Count; i++) {
				// Find empty slot
				if (items [i].ID == -1) {
					// Add Item to slot
					items [i] = itemToAdd;
					GameObject itemObject = Instantiate (inventoryItem);
					itemObject.GetComponent<ItemData> ().item = itemToAdd;
					itemObject.GetComponent<ItemData> ().amount = 1;
					itemObject.GetComponent<ItemData> ().slotNum = i;
					itemObject.transform.SetParent (slots [i].transform); // Set parent
					itemObject.transform.position = Vector2.zero; // Set to centre
					itemObject.GetComponent<Image> ().sprite = itemToAdd.Sprite; 
					itemObject.name = itemToAdd.Title;
					break;
				}
			}
		}
	}

	bool CheckItemInInventory(Item item) {
		for (int i = 0; i < items.Count; i++) {
			if (items [i].ID == item.ID) {
				return true;
			}
		}
		return false;
	}

}
