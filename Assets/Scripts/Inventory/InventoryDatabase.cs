using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDatabase : MonoBehaviour {

	ItemDatabase itemDatabase;

	private Dictionary<Item, int> inventory = new Dictionary<Item, int>();

	void Start() {
		itemDatabase = GameObject.FindGameObjectWithTag ("Databases").GetComponent<ItemDatabase> ();

	}

	public Dictionary<Item, int> Inventory { 
		get {
			return inventory; 
		}
	}

	public void AddItemByID(int id, int quantity = 1) {
		if (FetchItemByID (id).ID != -1) {
			inventory [FetchItemByID (id)] += quantity;
		} else {
			inventory.Add (itemDatabase.FetchItemByID (id), quantity);
		}
	}

	public void RemoveItemByID(int id, int quantity = 1) {
		Item itemToRemove = FetchItemWithQuantityByID (id).Key;

		// Check if item is in inventory
		Debug.Assert (itemToRemove.ID != -1);

		if (inventory [itemToRemove] > quantity) {
			inventory [itemToRemove] -= quantity;
		} else {
			inventory.Remove (itemToRemove);
		}
	}

	public Item FetchItemByID (int id) {

		foreach (Item key in inventory.Keys) {
			if (key.ID == id) {
				return key;
			}
		}

		return new Item();
	}

	public KeyValuePair<Item, int> FetchItemWithQuantityByID(int id) {
		foreach (Item key in inventory.Keys) {
			if (key.ID == id) {
				return new KeyValuePair<Item, int>(key, inventory[key]);
			}
		}

		KeyValuePair<Item, int> newKeyValuePair = new KeyValuePair<Item, int> (new Item (), 0);
		return newKeyValuePair;
	}
}
