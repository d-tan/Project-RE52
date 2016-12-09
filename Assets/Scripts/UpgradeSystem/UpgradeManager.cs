using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeSection {
	Head,
	Torso,
	Arms,
	Legs,
	RubbishCollector
}

public class UpgradeManager : MonoBehaviour {

	GameUIManager UIManager;

	UpgradeDatabase upgradesDatabase;
	ItemDatabase itemDatabase;
	ResourceDatabase resourceDatabase;

	private string[] upgradeDescriptions;

	void Start () {
		UIManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameUIManager> ();

		GameObject databases = GameObject.FindGameObjectWithTag ("Databases");
		upgradesDatabase = databases.GetComponent<UpgradeDatabase> ();
		itemDatabase = databases.GetComponent<ItemDatabase> ();
		resourceDatabase = databases.GetComponent<ResourceDatabase> ();

		upgradeDescriptions = new string[upgradesDatabase.DatabaseCount];
		GenerateUpgradesStrings ();
		UIManager.DisplayUpgradeText (upgradeDescriptions);
		AddFunctionToUIButtons ();
	}

	void ImprovedAnalysisUpgrade() {
		Debug.Log ("Improved Analysis upgrade");
	}

	void WiderGripUpgrade() {
		Debug.Log ("Wider Grip Upgrade");
	}

	void Efficiency() {
		Debug.Log ("Efficiency Upgrade");
	}

	void MaxThroughput() {
		Debug.Log ("Max Throughput Upgrade");
	}




	private void AddFunctionToUIButtons() {
		List<Button> buttons = new List<Button> ();
		buttons = UIManager.GetUpgradeButtons ();

		buttons [0].onClick.AddListener (delegate {
			ImprovedAnalysisUpgrade ();
		});
		buttons [1].onClick.AddListener (delegate {
			WiderGripUpgrade ();
		});
		buttons [2].onClick.AddListener (delegate {
			Efficiency ();
		});
		buttons [3].onClick.AddListener (delegate {
			MaxThroughput ();
		});
	}
	
	private void GenerateUpgradesStrings() {
		for (int i = 0; i < upgradesDatabase.DatabaseCount; i++) {
			Upgrade upgrade = upgradesDatabase.FetchUpgradeByID (i);

			string itemsNresources = "";

			itemsNresources = GenerateItemsNResourcesString (upgrade);

//			Debug.Log(itemsNresources);
			upgradeDescriptions [i] = "<b>" + upgrade.Title + "</b>\n" + upgrade.Description + "\n" + itemsNresources;
		}
	}

	// Generates items and resources list as a string, given an upgrade, for display
	private string GenerateItemsNResourcesString(Upgrade upgrade) {
		string[] items = new string[upgrade.Items.Count];
		string[] resources = new string[upgrade.Resources.Count];
		bool itemsLonger = true; // determines if the items list is longer or the resource list
		string combinedString = "";

		int j = 0; // foreach loop indexer
		// populate items string array to hold each string formulated
		foreach (int key in upgrade.Items.Keys) {
			items[j] = itemDatabase.FetchItemByID (key).Title + ": " + upgrade.Items [key];
			j++;
		}
		j = 0;

		// populate resources string array to hold each string formulated
		foreach (int key in upgrade.Resources.Keys) {
			resources[j] = resourceDatabase.FetchResourceByID (key).Title + ": " + upgrade.Resources [key];
			j++;
		}
		j = 0;

		// Check which string array is longer
		if (items.Length > resources.Length) {
			combinedString = CombineItemsNResourcesStrings (items, resources);
		} else {
			combinedString = CombineItemsNResourcesStrings (resources, items);
		}

		return combinedString;
	}

	// Combines the 2 given string arrays assuming the longer array is first
	private string CombineItemsNResourcesStrings(string[] longer, string[] shorter) {
		string combinedString = "";
		for (int i = 0; i < longer.Length; i++) {
			if (i - (shorter.Length - 1) <= 0) {
				combinedString += longer [i] + "\t\t" + shorter [i] + "\n";
			} else {
				combinedString += longer [i] + "\t\t\n";
			}
		}

		return combinedString;
	}
}
