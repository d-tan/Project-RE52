using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeSection {
	Head,
	Torso,
	Left_Arm,
	Right_Arm,
	Tracks,
	Rubbish_Collector
}

public enum UpgradeNames {
	Improved_Analysis, // Increases rarity finds; players can find rarer items
	Wider_Grip, // Increases the number of items picked up in one go; players can pick up near by items that are the same as the item tapped on
	Efficiency, // Increases time taken for multipliers to count down or time value per item; easier for players to reach higher streaks
	Max_Throughput // Increases number of items spawned by reducing spawn time
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
//		GenerateUpgradesStrings ();
//		UIManager.DisplayUpgradeText (upgradeDescriptions);
//		AddFunctionToUIButtons ();
	}

	void ImprovedAnalysisUpgrade() {
		Debug.Log ("Analysis pressed");
		if (CheckUpgradeIsAvaliable (UpgradeNames.Improved_Analysis)) {
			rubbishSpawner.RarityThresholds = upgradedRarityThresholds;
		}
	}

	void WiderGripUpgrade() {
		Debug.Log ("Wider Grip pressed");
		if (CheckUpgradeIsAvaliable (UpgradeNames.Wider_Grip)) {
			Debug.Log ("Wider Grip Upgrade");
		}
	}

	void Efficiency() {
		Debug.Log ("Efficiency pressed");
		if (CheckUpgradeIsAvaliable (UpgradeNames.Efficiency)) {
			Debug.Log ("Efficiency Upgrade");
		}
	}

	void MaxThroughput() {
		Debug.Log ("Max Throughput pressed");
		if (CheckUpgradeIsAvaliable (UpgradeNames.Max_Throughput)) {
			Debug.Log ("Max Throughput Upgrade");
		}
	}

	private bool CheckUpgradeIsAvaliable(UpgradeNames upgradeName) {
		bool itemsAvaliable = true;
		bool resourcesAvaliable = true;
		Upgrade upgradeToCheck = upgradesDatabase.FetchUpgradeByID ((int)upgradeName);

		// Check Inventory if item requirements are met
		Dictionary<int, int> itemsRequired = upgradeToCheck.Items;
		itemsAvaliable = CheckInventoryOrResources (itemsRequired, true);

		// check resources if resources requirements are met
		Dictionary<int, int> resourcesRequired = upgradeToCheck.RequiredResources;
		resourcesAvaliable = CheckInventoryOrResources (resourcesRequired, false);

		// Take from inventory + resources
		if (itemsAvaliable && resourcesAvaliable) {
//			UIManager.DisableUpgradeButton (upgradeName);
			TakeItemsOrResources (itemsRequired, true);
			TakeItemsOrResources (resourcesRequired, false);

			return true;
		} else {
//			StopCoroutine (UIManager.UpgradeUnavaliableFlash (upgradeName));
//			StartCoroutine (UIManager.UpgradeUnavaliableFlash (upgradeName));
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

			return false;
		}
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


//	private void AddFunctionToUIButtons() {
//		List<Button> buttons = new List<Button> ();
//		buttons = UIManager.GetUpgradeButtons ();
//
//		buttons [0].onClick.AddListener (delegate {
//			ImprovedAnalysisUpgrade ();
//		});
//		buttons [1].onClick.AddListener (delegate {
//			WiderGripUpgrade ();
//		});
//		buttons [2].onClick.AddListener (delegate {
//			Efficiency ();
//		});
//		buttons [3].onClick.AddListener (delegate {
//			MaxThroughput ();
//		});
//	}

	public void AddFunctionToUIButton(Button button, UpgradeSection section) {
		button.onClick.RemoveAllListeners ();
		switch (section) {
		case UpgradeSection.Head:
			button.onClick.AddListener (() => ImprovedAnalysisUpgrade ());
			break;

		case UpgradeSection.Left_Arm:
			button.onClick.AddListener (() => WiderGripUpgrade ());
			break;

		case UpgradeSection.Right_Arm:
			button.onClick.AddListener (() => Efficiency ());
			break;

		case UpgradeSection.Torso:
			break;

		case UpgradeSection.Tracks:
			button.onClick.AddListener (() => MaxThroughput ());
			break;

		case UpgradeSection.Rubbish_Collector:
			break;
		}
	}
	
//	private void GenerateUpgradesStrings() {
//		for (int i = 0; i < upgradesDatabase.DatabaseCount; i++) {
//			Upgrade upgrade = upgradesDatabase.FetchUpgradeByID (i);
//
//			string itemsNresources = "";
//
//			itemsNresources = GenerateItemsNResourcesString (upgrade);
//
////			Debug.Log(itemsNresources);
//			upgradeDescriptions [i] = "<b>" + upgrade.Title + "</b>\n" + upgrade.Description + "\n" + itemsNresources;
//		}
//	}

//	// Generates items and resources list as a string, given an upgrade, for display
//	private string GenerateItemsNResourcesString(Upgrade upgrade) {
//		string[] items = new string[upgrade.Items.Count];
//		string[] resources = new string[upgrade.RequiredResources.Count];
////		bool itemsLonger = true; // determines if the items list is longer or the resource list
//		string combinedString = "";
//
//		int j = 0; // foreach loop indexer
//		// populate items string array to hold each string formulated
//		foreach (int key in upgrade.Items.Keys) {
//			items[j] = itemDatabase.FetchItemByID (key).Title + ": " + upgrade.Items [key];
//			j++;
//		}
//		j = 0;
//
//		// populate resources string array to hold each string formulated
//		foreach (int key in upgrade.RequiredResources.Keys) {
//			resources[j] = resourceDatabase.FetchResourceByID (key).Title + ": " + upgrade.RequiredResources [key];
//			j++;
//		}
//		j = 0;
//
//		// Check which string array is longer
//		if (items.Length > resources.Length) {
//			combinedString = CombineItemsNResourcesStrings (items, resources);
//		} else {
//			combinedString = CombineItemsNResourcesStrings (resources, items);
//		}
//
//		return combinedString;
//	}

//	// Combines the 2 given string arrays assuming the longer array is first
//	private string CombineItemsNResourcesStrings(string[] longer, string[] shorter) {
//		string combinedString = "";
//		for (int i = 0; i < longer.Length; i++) {
//			if (i - (shorter.Length - 1) <= 0) {
//				combinedString += longer [i] + "\t\t" + shorter [i] + "\n";
//			} else {
//				combinedString += longer [i] + "\t\t\n";
//			}
//		}
//
//		return combinedString;
//	}
}
