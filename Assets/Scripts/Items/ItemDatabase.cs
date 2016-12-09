using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public enum ItemRarity {
	Common,
	Uncommon,
	Rare,
	SuperRare
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


		ConstructItemDatabase ();
		ConstructItemRarityList ();
	}
		
	private void ConstructItemDatabase() {
		for (int i = 0; i < itemData.Count; i++) {
			database.Add (new Item (
				(int)itemData [i] ["id"], 
				itemData [i] ["title"].ToString(), 
				itemData[i] ["slug"].ToString(),
				itemData[i] ["description"].ToString(),
				(ResourceType)(int)itemData[i]["resource"] ["type1"],
				(ResourceType)(int)itemData[i]["resource"] ["type2"],
				(int)itemData[i]["resource"]["quantity1"],
				(int)itemData[i]["resource"]["quantity2"],
				(bool)itemData[i]["craftingItem"],
				(ItemRarity)(int)itemData[i]["rarity"]
			));
		}
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
			case ItemRarity.Common:
				// common
				commonItemsID.Add (database [i].ID);
//				Debug.Log ("name: " + database [i].Title + " added to common.");
				break;

			case ItemRarity.Uncommon:
				// uncommon
				uncommonItemsID.Add (database [i].ID);
//				Debug.Log ("name: " + database [i].Title + " added to uncommon.");
				break;

			case ItemRarity.Rare:
				// rare
				rareItemsID.Add (database [i].ID);
//				Debug.Log ("name: " + database [i].Title + " added to rare.");
				break;

			case ItemRarity.SuperRare:
				// super-rare
				superItemsID.Add (database [i].ID);
//				Debug.Log ("name: " + database [i].Title + " added to super rare.");
				break;

			default:
				Debug.Log ("Unknown rarity detected in items database.");
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
		case ItemRarity.Common:
			chosenItemID = commonItemsID[Mathf.FloorToInt (Random.Range (0, commonItemsID.Count))];
			break;
		case ItemRarity.Uncommon:
			chosenItemID = uncommonItemsID[Mathf.FloorToInt (Random.Range (0, uncommonItemsID.Count))];
			break;
		case ItemRarity.Rare:
			chosenItemID = rareItemsID[Mathf.FloorToInt (Random.Range (0, rareItemsID.Count))];
			break;
		case ItemRarity.SuperRare:
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
