﻿using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

public enum ItemRarity {
	Not_Spawnable = -1,
	Common_ = 0,
	Uncommon_,
	Rare_,
	Super_Rare
}

public enum ItemIDBoundaries {
	Items_ = 0,
	Task_Items = 200
}

public class ItemDatabase : MonoBehaviour {
	
	private List<Item> database = new List<Item>();
	private JsonData itemData;

	private List<int> commonItemsID = new List<int> (); // commons are 0
	private List<int> uncommonItemsID = new List<int> (); // uncommons are 1
	private List<int> rareItemsID = new List<int>(); // rares are 2
	private List<int> superItemsID = new List<int>(); //  super-rares are 3

	void Start() {
//		itemData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssests/Items.json"));
//		itemData = JsonMapper.ToObject (File.ReadAllText("jar:file://" + Application.dataPath + "!/assets/" + "/Items.json"));

		TextAsset file = Resources.Load("Json/Items") as TextAsset;
		itemData = JsonMapper.ToObject (file.text);

//		database.Add (new CraftingItem (10, "New Crafting Item", "new_crafting_item", "A new Crafting Item", ResourceType.Building_Materials, ResourceType.Building_Materials, 10, 10, true, 0, "Special Propert"));
//		Debug.Log (((CraftingItem)database [0]).Title);

		ConstructItemDatabase ();
		ConstructItemRarityList ();
	}
		
	private void ConstructItemDatabase() {
		for (int i = 0; i < itemData.Count; i++) {
			// Check if item is a crafting item
			if ((bool)itemData [i] ["craftingItem"]) {
				
				// Check if item itself is craftable
				if ((bool)itemData [i] ["craftable"]) {

					// Check if item is a task item
					if ((bool)itemData [i] ["taskItem"]) {
						AddTaskItem (itemData [i]);

					} else {
						AddCaftingItem (true, itemData [i]);
					}
				// Not Craftable
				} else {
					AddCaftingItem (false, itemData [i]);
				}
			
			// Not a crafting item
			} else {
				AddItem (itemData [i]);
			}
		}
	}

	private void AddTaskItem(JsonData data) {
		database.Add (new TaskItem (
			(int)data ["id"], 
			data ["title"].ToString (), 
			data ["slug"].ToString (),
			data ["description"].ToString (),
			ConstructResourceDictionary (data ["resources"] ["types"], data ["resources"] ["quantities"]),
			(bool)data ["craftingItem"],
			(ItemRarity)(int)data ["rarity"],
			(bool)data ["craftable"],
			ConstructResourceDictionary (data ["requiredResources"] ["types"], data ["requiredResources"] ["quantities"]),
			(bool)data["taskItem"],
			ConstructCraftingItemDictionary(data["requiredItems"]["IDs"], data["requiredItems"]["quantities"])
		));
	}

	private void AddCaftingItem(bool craftable, JsonData data) {
		if (craftable) {
			database.Add (new CraftingItem (
				(int)data ["id"], 
				data ["title"].ToString (), 
				data ["slug"].ToString (),
				data ["description"].ToString (),
				ConstructResourceDictionary (data ["resources"] ["types"], data ["resources"] ["quantities"]),
				(bool)data ["craftingItem"],
				(ItemRarity)(int)data ["rarity"],
				(bool)data ["craftable"],
				ConstructResourceDictionary (data ["requiredResources"] ["types"], data ["requiredResources"] ["quantities"]),
				(bool)data["taskItem"]
			));
		} else {
			database.Add (new CraftingItem (
				(int)data ["id"], 
				data ["title"].ToString (), 
				data ["slug"].ToString (),
				data ["description"].ToString (),
				ConstructResourceDictionary (data ["resources"] ["types"], data ["resources"] ["quantities"]),
				(bool)data ["craftingItem"],
				(ItemRarity)(int)data ["rarity"],
				(bool)data ["craftable"]
			));
		}
	}

	private void AddItem(JsonData data) {
		database.Add (new Item (
			(int)data ["id"], 
			data ["title"].ToString (), 
			data ["slug"].ToString (),
			data ["description"].ToString (),
			ConstructResourceDictionary(data["resources"]["types"], data["resources"]["quantities"]),
			(bool)data ["craftingItem"],
			(ItemRarity)(int)data ["rarity"]
		));
	}

	private Dictionary<ResourceType, int> ConstructResourceDictionary(JsonData resourceList, JsonData quantities) {
		Dictionary<ResourceType, int> resourcesNquantities = new Dictionary<ResourceType, int> ();
		for (int i = 0; i < resourceList.Count; i++) {
			resourcesNquantities.Add ((ResourceType)(int)resourceList [i.ToString ()], (int)quantities [i.ToString ()]);
		}

		return resourcesNquantities;
	}

	private Dictionary<CraftingItem, int> ConstructCraftingItemDictionary(JsonData itemIDList, JsonData quantities) {
		Dictionary<CraftingItem, int> itemsNquantities = new Dictionary<CraftingItem, int>();
		for (int i = 0; i < itemIDList.Count; i++) {
			Item itemToAdd = FetchItemByID ((int)itemIDList [i.ToString()]);
			Debug.Assert (itemToAdd.ID != -1, "Invalid Item parsed");
			Debug.Assert (itemToAdd.IsCraftingItem, "Not a crafting Item");
			itemsNquantities.Add ((CraftingItem)itemToAdd, (int)quantities [i.ToString()]);
		}

		return itemsNquantities;
	}

	public Item FetchItemByID(int id) {
		for (int i = 0; i < database.Count; i++) {
			if (database [i].ID == id) {
				return database [i];
			}
		}

		return new Item ();
	}

	// Constructs lists of item IDs by the rarity
	private void ConstructItemRarityList() {
		for (int i = 0; i < database.Count; i++) {
			switch (database [i].Rarity) {
			case ItemRarity.Common_:
			// common
				commonItemsID.Add (database [i].ID);
//				Debug.Log ("name: " + database [i].Title + " added to common.");
				break;

			case ItemRarity.Uncommon_:
			// uncommon
				uncommonItemsID.Add (database [i].ID);
//				Debug.Log ("name: " + database [i].Title + " added to uncommon.");
				break;

			case ItemRarity.Rare_:
			// rare
				rareItemsID.Add (database [i].ID);
//				Debug.Log ("name: " + database [i].Title + " added to rare.");
				break;

			case ItemRarity.Super_Rare:
			// super-rare
				superItemsID.Add (database [i].ID);
//				Debug.Log ("name: " + database [i].Title + " added to super rare.");
				break;
			}
		}
	}

	/// <summary>
	/// Picks a random item from a list based on given rarity.
	/// </summary>
	/// <returns>The random item.</returns>
	/// <param name="rarity">Rarity.</param>
	public int PickRandomItem(ItemRarity rarity) {

		int chosenItemID = 0;
		
		switch (rarity) {
		case ItemRarity.Common_:
			chosenItemID = commonItemsID[Mathf.FloorToInt (Random.Range (0, commonItemsID.Count))];
			break;
		case ItemRarity.Uncommon_:
			chosenItemID = uncommonItemsID[Mathf.FloorToInt (Random.Range (0, uncommonItemsID.Count))];
			break;
		case ItemRarity.Rare_:
			chosenItemID = rareItemsID[Mathf.FloorToInt (Random.Range (0, rareItemsID.Count))];
			break;
		case ItemRarity.Super_Rare:
			chosenItemID = superItemsID[Mathf.FloorToInt (Random.Range (0, superItemsID.Count))];
			Debug.Log ("Super rare item ID: " + chosenItemID + " name: " + database[chosenItemID].Title);
			break;
		default:
			Debug.Log ("Unknown item rarity given");
			break;
		}

		return chosenItemID;
	}
}
