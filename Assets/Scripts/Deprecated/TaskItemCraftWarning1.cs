using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItemCraftWarning1 : MonoBehaviour {

//	// ------ ITEM WARNING ------
//
//	public void CheckTaskItemCraftingAvailable(int taskItemID) {
//		//		taskItemCraftWarning.ToggleGroup (true);
//
//		TaskItem taskItem = itemDatabase.FetchTaskItemByID (taskItemID);
//		Debug.Assert (taskItem.ID != -1, "Unknown TaskItem ID: " + taskItemID + "; Should not happen.");
//
//		bool hasUncraftable = false;
//		bool insufficientResources = false;
//		string description = "";
//		string title = "";
//
//		// Clear itemsPanel
//		ClearDisplay(taskItemCraftWarning.ItemsPanel);
//
//		// Check if player meets Item Requirements (i.e. player has missing items that cannot be crafted)
//		Dictionary<CraftingItem, int> missingItems = CheckItemRequirements(taskItem);
//		foreach (CraftingItem key in missingItems.Keys) {
//
//			// Check if there is a key signifying that there are uncraftable items
//			if (key.ID == -1 && missingItems [key] == -1) {
//				hasUncraftable = true;
//				description += "There are items missing that are uncraftable. We cannot complete this without them.";
//				break;
//			}
//		}
//
//		// check if player has insufficient resources, including the missing items
//		Dictionary<ResourceType, int> totalResources = CheckResourceRequirements(taskItem, missingItems);
//
//		// Check if there is a key signifying that there are insufficient resources
//		if (totalResources.ContainsKey (ResourceType.Unknown)) {
//			insufficientResources = true;
//			description += "We do not have enough resources to complete this action. We will need to gather more.";
//		}
//
//		// Check if play has missing items (that are craftable)
//		if (missingItems.Count > 0) {
//			taskItemCraftWarning.ToggleGroup (true);
//			if (!hasUncraftable && !insufficientResources) {
//				description = "We are missing some items, but we have enough resources to make them.";
//				title = "Warning: Items Missing.";
//				taskItemCraftWarning.SetBreakdownText (taskItemID, totalResources, description, title);
//
//
//			} else {
//
//				if (insufficientResources) {
//					title = "Error: Insufficient Resources!";
//				} else if (hasUncraftable) {
//					title = "Error: Items Unavailable!";
//				}
//				taskItemCraftWarning.SetBreakdownText (taskItemID, totalResources, description, title, false);
//
//
//			}
//			taskItemCraftWarning.CraftButton.onClick.RemoveAllListeners ();
//			taskItemCraftWarning.CraftButton.onClick.AddListener (() => {
//				taskItemCraftWarning.CraftTaskItem(taskItem, missingItems, totalResources);
//				taskItemUIGroup.RefreshResources(taskItem);
//			});
//
//
//			// Player has no missing items (and sufficient resources)
//		} else {
//			Debug.Log ("All pass. Task Item is crafted");
//			taskItemUIGroup.CraftTaskItem(taskItem);
//			taskItemUIGroup.RefreshResources(taskItem);
//			ClearDisplay (taskItemUIGroup.ItemsPanel);
//			CreateItemList (taskItem);
//			taskItemCraftWarning.ToggleGroup (false);
//			Debug.Log ("Toggling Warning Off");
//		}
//	}
//
//	// Check if the player has enough resources to craft the item, including missin craftable items
//	private Dictionary<ResourceType, int> CheckResourceRequirements(TaskItem taskItem, Dictionary<CraftingItem, int> craftingItems) {
//		// Calculate total resources required to craft the TaskITem
//		Dictionary<ResourceType, int> totalResources = new Dictionary<ResourceType, int>();
//		foreach (ResourceType key in taskItem.RequiredResources.Keys) {
//			totalResources.Add (key, taskItem.RequiredResources [key]);
//		}
//
//		// Loop through each missing item
//		foreach (CraftingItem key in craftingItems.Keys) {
//			if (key.ID != -1 && key.Craftable) {
//				Dictionary<ResourceType, int> requiredResources = key.RequiredResources;
//				// loop through each resource required to craft this item
//				foreach (ResourceType resourceType in requiredResources.Keys) {
//					// Check if total resources list has the current resource type
//					if (totalResources.ContainsKey (resourceType)) {
//						totalResources [resourceType] += craftingItems [key] * requiredResources [resourceType];
//
//						// if not then add a new resource type
//					} else {
//						totalResources.Add (resourceType, craftingItems [key] * requiredResources [resourceType]);
//					}
//				}
//			}
//		}
//
//		foreach (ResourceType key in totalResources.Keys) {
//			Resource resource = resourceDatabase.FetchResourceByID ((int)key);
//			// Check if player has enough resources
//			if (resource.Quantity < totalResources [key]) {
//
//				// Add a key to the total resources list to signify that there are insufficient resources
//				Debug.Log("Adding unknown resource");
//				totalResources.Add (ResourceType.Unknown, -1);
//				break;
//			}
//		}
//
//		return totalResources;
//	}
//
//	// Checks if the player has the required items, and if some are missing, then check if they are craftable
//	private Dictionary<CraftingItem, int> CheckItemRequirements(TaskItem taskItem) {
//		// Check if player is missing crafting items
//		Dictionary<CraftingItem, int> missingItems = new Dictionary<CraftingItem, int>();
//		Dictionary<CraftingItem, int> uncraftables = new Dictionary<CraftingItem, int>();
//		Dictionary<CraftingItem, int> requiredItems = taskItem.CraftingItems;
//		bool cannotCraft = false;
//
//		// loop through each item required for crafting
//		foreach (CraftingItem key in requiredItems.Keys) {
//
//			bool itemMissing = false;
//			KeyValuePair<Item, int> craftingItem = inventoryDatabase.FetchItemWithQuantityByID (key.ID);
//
//			// if item to look for is craftable
//			if (key.Craftable) {
//				// check if TaskItem cannot be crafted
//				if (!cannotCraft) {
//					// Check if item exists in inventory
//					if (craftingItem.Key.ID == -1) {
//						missingItems.Add (key, requiredItems [key]);
//						itemMissing = true;
//
//						// Check if there is enough items in player's inventory
//					} else if (craftingItem.Value < requiredItems [key]) {
//						missingItems.Add (key, requiredItems [key] - craftingItem.Value);
//						itemMissing = true;
//					}
//				}
//
//				// if item to look for is NOT craftable
//			} else {
//
//				// check if inventory has the item OR if the player has enough of the item
//				if (craftingItem.Key.ID == -1) {
//					itemMissing = true;
//					uncraftables.Add (key, requiredItems [key]);
//
//				} else if (craftingItem.Value < requiredItems [key]) {
//					itemMissing = true;
//					uncraftables.Add (key, requiredItems [key] - craftingItem.Value);
//				}
//
//				if (itemMissing) {
//					if (!cannotCraft) {
//						// Clear itemsPanel to only show uncraftable items
//						ClearDisplay (taskItemCraftWarning.ItemsPanel);
//						cannotCraft = true;
//					}
//				}
//			}
//
//			// if item is doesn't exist in player's inventory OR player does not have enough 
//			// instantiate a slot to display the item
//			if (cannotCraft) {
//				InstantiateSlot (
//					taskItemCraftWarning.Slot, 
//					taskItemCraftWarning.ItemsPanel.transform, 
//					key.Sprite, 
//					SetStringColor (uncraftables [key].ToString (), "red")
//				);
//			} else {
//				if (itemMissing) {
//					InstantiateSlot (
//						taskItemCraftWarning.Slot, 
//						taskItemCraftWarning.ItemsPanel.transform, 
//						key.Sprite, 
//						SetStringColor (missingItems [key].ToString (), "red")
//					);
//				}
//			}
//		}
//
//		if (cannotCraft) {
//			uncraftables.Add (new CraftingItem (), -1);
//			return uncraftables;
//		} else {
//			return missingItems;
//		}
//	}
}

//
//[System.Serializable]
//public class TaskItemCraftWarning : TaskItemUIGroup {
//
//	protected Button cancelButton;
//	protected Text title;
//
//	public override void GetChildren ()
//	{
//		base.GetChildren ();
//
//		title = groupObject.transform.GetChild (1).transform.GetChild(5).GetComponent<Text>();
//
//		cancelButton = groupObject.transform.GetChild (4).GetComponent<Button> ();
//
//		cancelButton.onClick.AddListener (() => CancelButton ());
//	}
//
//	public void SetBreakdownText (int taskItemID, Dictionary<ResourceType, int> resourcesRequired, string description, string titleText, bool canCraft = true) {
//
//		TaskItem taskItem = itemDatabase.FetchTaskItemByID (taskItemID);
//		Debug.Assert (taskItem.ID != -1, "Item id: " + taskItemID + " is not a TaskItem.");
//
//		string resourceList = GenerateResourceList(resourcesRequired);
//
//		// Displays the texts in their respective Text; is in a specific order based on the hierarchy 
//		textList [0].text = taskItem.Title; // Name
//		textList [1].text = description; // Description
//		textList [2].text = resourceList;
//
//		title.text = titleText; // Change title
//
//		craftButton.enabled = canCraft;
//	}
//
//	public void CancelButton() {
//		this.ToggleGroup (false);
//	}
//
//	public void CraftTaskItem(TaskItem taskItem, Dictionary<CraftingItem, int> missingItems, Dictionary<ResourceType, int> totalResources) {
//		Dictionary<CraftingItem, int> itemsToRemove = new Dictionary<CraftingItem, int> ();
//
//		ToggleGroup (false);
//
//		foreach (CraftingItem key in missingItems.Keys) {
//			itemsToRemove.Add (key, taskItem.CraftingItems [key] - missingItems [key]);
//			Debug.Assert (itemsToRemove [key] >= 0, "This should not happen.");
//		}
//
//		taskManager.AddActiveTaskItem (taskItem, 1);
//		taskManager.RemoveItems (itemsToRemove);
//		taskManager.RemoveResources (totalResources);
//
//		RefreshResources (taskItem);
//	}
//}