using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

public class ItemDatabase : MonoBehaviour {
	
	private List<Item> database = new List<Item>();
	private JsonData itemData;

	void Start() {

		itemData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssests/Items.json"));
		ConstructItemDatabase ();
	}
		
	void ConstructItemDatabase() {
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
				(int)itemData[i]["rarity"]

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
}
