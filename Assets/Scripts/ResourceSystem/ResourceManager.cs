using UnityEngine;
using System.Collections;

public class ResourceManager : MonoBehaviour {

	ResourceDatabase resourceDatabase;
	ItemDatabase itemDatabase;
	GameUIManager UIManager;

	void Start() {
		GameObject gameController = GameObject.FindGameObjectWithTag ("Databases");
		resourceDatabase = gameController.GetComponent<ResourceDatabase> ();
		itemDatabase = gameController.GetComponent<ItemDatabase>();
		UIManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameUIManager> ();
	}

	public void AddResourceValue(int itemID, RubbishType rubbishType, int multiplier, int sign = 1) {
		Item item = itemDatabase.FetchItemByID (itemID);
		Resource resourceToAdd = resourceDatabase.FetchResourceByID ((int)item.Resource1);
		bool first = true; // boolean to determine if the resource to add to is the first or second.

		if (item.Resource1 != item.Resource2) {
			if (resourceToAdd.RubbishType != rubbishType) {
				resourceToAdd = resourceDatabase.FetchResourceByID ((int)item.Resource2);
				first = false;
			}
		}
		if (first) {
			resourceToAdd.Quantity += sign * item.Quantity1 * multiplier;
		} else {
			resourceToAdd.Quantity += sign * item.Quantity2 * multiplier;
		}

	}

	// for displaying in UI
	public void DisplayResources(RubbishType type, bool checkIfAvaliable = false) {

		string textDisplay = "";
		int databaseLength = resourceDatabase.DatabaseLength;
		Resource resource;

		for (int i = 0; i < databaseLength; i++) {
			resource = resourceDatabase.FetchResourceByID (i);
			if (resource.RubbishType == type) {
				textDisplay += "ID: " + resource.ID + " | " + resource.Title + " | Amount: " + resource.Quantity + "\n";
			}
		}

		UIManager.DisplayResource (textDisplay, checkIfAvaliable);

	}

	public void UpdateResourceDisplay(RubbishType type) {
		DisplayResources (type, true);
	}
}
