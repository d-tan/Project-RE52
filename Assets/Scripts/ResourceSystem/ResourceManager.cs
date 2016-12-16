using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		List<Resource> itemsResources = new List<Resource>();
		foreach (ResourceType key in item.ResourcesGiven.Keys) {
			itemsResources.Add (resourceDatabase.FetchResourceByID ((int)key));
		}

		for (int i = 0; i < itemsResources.Count; i++) {
			if (itemsResources [i].RubbishType == rubbishType) {
				// Add to the quantity
				itemsResources [i].Quantity += 
					sign * item.ResourcesGiven [(ResourceType)itemsResources [i].ID] * multiplier;
				// Resource ID should match ResourceType enum
			}
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
