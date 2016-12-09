using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

public class UpgradeDatabase : MonoBehaviour {

	private List<Upgrade> database = new List<Upgrade>();
	private JsonData upgradeData;

	void Start () {
		TextAsset file = Resources.Load("Json/Upgrades") as TextAsset;
		upgradeData = JsonMapper.ToObject (file.text);

		ConstructUpgradeDatabase ();
	}
	
	private void ConstructUpgradeDatabase() {
		for (int i = 0; i < upgradeData.Count; i++) {
			Dictionary <int, int> items = new Dictionary<int, int> ();
			Dictionary <int, int> resources = new Dictionary<int, int> ();

			items = ConstructRequirementsList ("items", i);
			resources = ConstructRequirementsList ("resources", i);

			database.Add (new Upgrade (
				(int)upgradeData [i] ["id"],
				upgradeData [i] ["title"].ToString (),
				upgradeData [i] ["slug"].ToString (),
				upgradeData [i] ["description"].ToString (),
				(UpgradeSection)(int)upgradeData [i] ["section"],
				items,
				resources
			));

//			Debug.Log ("ID: " + database [i].ID + " " + database [i].Title + " " + database [i].Section + "\n" + database [i].Items.Count + " " + database [i].Resources.Count);
		}
	}

	/// <summary>
	/// Constructs a list of IDs and Quantities based on a SPECIFIC structure. Please follow format.
	/// </summary>
	/// <returns>The requirements list.</returns>
	/// <param name="requirementName">Name of the requirement as specified in the Json file.</param>
	/// <param name="index">index of the current loop, looping through the Json file.</param>
	private Dictionary<int, int> ConstructRequirementsList(string requirementName, int index) {
		// Temporary dictionary to store a generic list
		Dictionary <int, int> list = new Dictionary<int, int> ();

		// Loop through the IDs of the requirements section with specified requirement name
		// Note: the number of IDs and quantities should match, as the quantity must be specified for each ID
		for (int j = 0; j < upgradeData [index] ["requirements"][requirementName]["IDs"].Count; j++) {
			string idName = "ID" + (j + 1).ToString ();
			string quantityName = "Q" + (j + 1).ToString ();

			list.Add ((int)upgradeData [index] ["requirements"] [requirementName] ["IDs"] [idName],
				(int)upgradeData [index] ["requirements"] [requirementName] ["quantities"] [quantityName]);
		}

		return list;
	}

	public Upgrade FetchUpgradeByID(int id) {
		for (int i = 0; i < database.Count; i++) {
			if (database [i].ID == id) {
				return database [i];
			}
		}

		return database [0];
	}

	public int DatabaseCount {
		get {
			return database.Count;
		}
	}
}
