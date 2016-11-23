using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

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

		itemData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssests/Items.json"));
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
				(RubbishType)(int)itemData[i]["rubbishType"] ["type1"],
				(RubbishType)(int)itemData[i]["rubbishType"] ["type2"],
				(int)itemData[i]["rubbishType"]["quantity1"],
				(int)itemData[i]["rubbishType"]["quantity2"],
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

		return new Item ();;
	}

	// Constructs lists of item IDs by the rarity
	private void ConstructItemRarityList() {
		for (int i = 0; i < database.Count; i++) {
			switch (database [i].Rarity) {
			case ItemRarity.Common:
				// common
				commonItemsID.Add (database [i].ID);
				break;

			case ItemRarity.Uncommon:
				// uncommon
				uncommonItemsID.Add (database [i].ID);
				break;

			case ItemRarity.Rare:
				// rare
				rareItemsID.Add (database [i].ID);
				break;

			case ItemRarity.SuperRare:
				// super-rare
				superItemsID.Add (database [i].ID);
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
			chosenItemID = Mathf.FloorToInt (Random.Range (0, commonItemsID.Count));
			break;
		case ItemRarity.Uncommon:
			chosenItemID = Mathf.FloorToInt (Random.Range (0, uncommonItemsID.Count));
			break;
		case ItemRarity.Rare:
			chosenItemID = Mathf.FloorToInt (Random.Range (0, rareItemsID.Count));
			break;
		case ItemRarity.SuperRare:
			chosenItemID = Mathf.FloorToInt (Random.Range (0, superItemsID.Count));
			break;
		default:
			Debug.Log ("Unknown item rarity given");
			break;
		}

		return chosenItemID;
	}
}
