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

public enum UpgradeNames {
	Improved_Analysis,
	Wider_Grip,
	Efficiency,
	Max_Throughput
}

public class UpgradeManager : MonoBehaviour {

	GameUIManager UIManager;

	UpgradeDatabase upgradesDatabase;
	ItemDatabase itemDatabase;
	ResourceDatabase resourceDatabase;
	InventoryDatabase inventoryDatabase;

	RubbishItemSpawner rubbishSpawner;

	private string[] upgradeDescriptions;

	// Improved Analysis variables
	private Vector4 upgradedRarityThresholds = new Vector4 (1.0f, 0.18f, 0.045f, 0.015f); // (common, uncommon, rare, super-rare)

	void Start () {
		UIManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameUIManager> ();

		GameObject databases = GameObject.FindGameObjectWithTag ("Databases");
		upgradesDatabase = databases.GetComponent<UpgradeDatabase> ();
		itemDatabase = databases.GetComponent<ItemDatabase> ();
		resourceDatabase = databases.GetComponent<ResourceDatabase> ();
		inventoryDatabase = databases.GetComponent<InventoryDatabase> ();

		rubbishSpawner = GameObject.FindGameObjectWithTag ("ConveyorBelt").GetComponent<RubbishItemSpawner> ();

		upgradeDescriptions = new string[upgradesDatabase.DatabaseCount];
		GenerateUpgradesStrings ();
		UIManager.DisplayUpgradeText (upgradeDescriptions);
		AddFunctionToUIButtons ();
	}

	void ImprovedAnalysisUpgrade() {
		bool itemsAvaliable = true;
		bool resourcesAvaliable = true;
		Debug.Log ("Improved Analysis upgrade");

		// Check Inventory if item requirements are met
		Dictionary<int, int> itemsRequired = upgradesDatabase.FetchUpgradeByID(0).Items;
		itemsAvaliable = CheckInventoryOrResources (itemsRequired, true);

		// check resources if resources requirements are met
		Dictionary<int, int> resourcesRequired = upgradesDatabase.FetchUpgradeByID (0).Resources;
		resourcesAvaliable = CheckInventoryOrResources (resourcesRequired, false);

		// Take from inventory + resources
		if (itemsAvaliable && resourcesAvaliable) {
			UIManager.DisableUpgradeButton (UpgradeNames.Improved_Analysis);
			TakeItemsOrResources (itemsRequired, true);
			TakeItemsOrResources (resourcesRequired, false);

			rubbishSpawner.RarityThresholds = upgradedRarityThresholds;
			Debug.Log ("Imrpoved Analysis upgrade Successful");

		} else {
			StopCoroutine (UIManager.UpgradeUnavaliableFlash (UpgradeNames.Improved_Analysis));
			StartCoroutine (UIManager.UpgradeUnavaliableFlash (UpgradeNames.Improved_Analysis));
			string message = "Unable to upgrade: \n";
			string itemsMessage = "Insufficient items avaliable.\n";
			string resourcesMessage = "Insufficient resources avaliable.\n";

			if (!itemsAvaliable) {
				message += itemsMessage;
			}

			if (!resourcesAvaliable) {
				message += resourcesMessage;
			}

			Debug.Log (message);
		}
		// implement upgrade
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

	private bool CheckInventoryOrResources(Dictionary<int, int> listToCheck, bool checkItems) {
		bool avaliable = true;
		int[] IDs = new int[listToCheck.Count];
		int[] quantities = new int[listToCheck.Count];

		int index = 0;
		foreach (int key in listToCheck.Keys) {
			IDs [index] = key;
			quantities [index] = listToCheck [key];
			index++;
		}

		for (int i = 0; i < IDs.Length; i++) {
			if (checkItems) {
				// For Checking Items
				KeyValuePair<Item, int> itemToCheck = inventoryDatabase.FetchItemWithQuantityByID(IDs[i]);
				if (itemToCheck.Value < quantities [i]) {
					avaliable = false;
					break;
				}
			} else {
				// For Checking Resources;
				Resource resourceToCheck = resourceDatabase.FetchResourceByID (IDs [i]);
				if (resourceToCheck.Quantity < quantities [i]) {
					avaliable = false;
					break;
				}
			}
		}

		return avaliable;
	}

	private void TakeItemsOrResources(Dictionary<int, int> listToTake, bool takeItems) {
		int[] IDs = new int[listToTake.Count];
		int[] quantities = new int[listToTake.Count];

		int index = 0;
		foreach (int key in listToTake.Keys) {
			IDs [index] = key;
			quantities [index] = listToTake [key];
			index++;
		}

		for (int i = 0; i < IDs.Length; i++) {
			if (takeItems) {
				// For Taking Items
				inventoryDatabase.RemoveItemByID (IDs [i], quantities[i]);
			} else {
				// For Taking Resources;
				Resource resourceToTake = resourceDatabase.FetchResourceByID (IDs [i]);
				resourceToTake.Quantity -= quantities [i];
			}
		}
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
//		bool itemsLonger = true; // determines if the items list is longer or the resource list
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
