using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;




public class GameUIManager : MonoBehaviour {

//	public Image[] indicators = new Image[2];
//	private Color[] indicatorColors = new Color[4];

	public Text resourceDisplay;

	// Upgrades
	public GameObject upgradesPanel;
	private float upgradeFlashTime = 0.5f;
	List<Text> upgradesTexts = new List<Text>();
	List<Button> upgradeButtons = new List<Button> ();

	private bool upgradesDisplaying = true;

	// Inventory
	public GameObject inventoryGroup;
	public GameObject inventoryPanel;
	public GameObject inventorySlot;
	InventoryDatabase inventoryDatabase;

	private bool inventoryDisplaying = true;

	// Resources
	ResourceDatabase resourceDatabase;

	// Tasks
	public GameObject taskGroup;
	public GameObject taskPanel;
	public GameObject taskSlot;

	public TaskUIGroup taskUIGroup;
	TaskDatabase taskDatabase;
	TaskManager taskManager;

	private bool taskDisplaying = true;

	// TaskItem
	public TaskItemUIGroup taskItemUIGroup;
	ItemDatabase itemDatabase;

	// Item
	public ItemUIGroup itemUIGroup;

	// ItemWarning
	public TaskItemCraftWarning taskItemCraftWarning;

	void Start() {
//		indicatorColors [0] = new Color (1.0f, 103.0f / 255.0f, 103.0f / 255.0f, 160.0f / 255.0f);
//		indicatorColors [1] = new Color (54.0f / 255.0f, 188.0f / 255.0f, 71.0f / 255.0f, 160.0f / 255.0f);
//		indicatorColors [2] = new Color (241.0f / 255.0f, 244.0f / 255.0f, 48.0f / 255.0f, 160.0f / 255.0f);
//		indicatorColors [3] = new Color (184.0f / 255.0f, 184.0f / 255.0f, 184.0f / 255.0f, 160.0f / 255.0f);

		GameObject databases = GameObject.FindGameObjectWithTag ("Databases");

		// Resources
		resourceDisplay.gameObject.SetActive (false);

		// Upgrades
		RetrieveUpgradesUI ();
		ToggleUpgradesDisplay ();

		// Inventory
		ToggleInventoryDisplay();
		inventoryDatabase = databases.GetComponent<InventoryDatabase>();

		// Resources
		resourceDatabase = databases.GetComponent<ResourceDatabase>();

		// TaskItemCraftWarning
		taskItemCraftWarning.GetChildren();
		taskItemCraftWarning.ToggleGroup (false);

		// Items
		itemUIGroup.GetChildren();
		itemUIGroup.ToggleGroup (false);

		// TaskItems
		taskItemUIGroup.GetChildren();
		taskItemUIGroup.ToggleGroup (false);
		itemDatabase = databases.GetComponent<ItemDatabase> ();

		// Tasks
		taskUIGroup.GetChildren();
		taskUIGroup.ToggleGroup (false);
		ToggleTasksDisplay ();
		taskDatabase = databases.GetComponent<TaskDatabase>();
		taskManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TaskManager> ();


	}


//	public void ChangeIndicatorColors(bool active, RubbishType type1 = RubbishType.General, RubbishType type2 = RubbishType.General) {
//		
//		// Check if indicators are active
//		if (active) {
//			switch (type1) {
//			case RubbishType.Organic:
//				indicators [0].color = indicatorColors [(int)type2];
//				indicators [1].color = indicatorColors [(int)type1];
//				break;
//			case RubbishType.Recycle:
//				indicators [0].color = indicatorColors [(int)type1];
//				indicators [1].color = indicatorColors [(int)type2];
//				break;
//			default:
//				indicators [0].color = indicatorColors [(int)type1];
//				indicators [1].color = indicatorColors [(int)type2];
//				break;
//			}
//		} else {
//			// Set them to Grey
//			for (int i = 0; i < indicators.Length; i++) {
//				indicators [i].color = indicatorColors [3];
//			}
//		}
//
//	}

	// --------- GENERAL UI METHODS ---------
	private void ClearDisplay(GameObject panel) {
		for (int i = 0; i < panel.transform.childCount; i++) {
			Destroy (panel.transform.GetChild (i).gameObject);
		}
	}

	public string SetStringColor(string text, string color) {
		text = "<color=" + color + ">" + text + "</color>";
		return text;
	}
		
	/// <summary>
	/// Instantiates a button slot, usually for holding items.
	/// </summary>
	/// <returns>A button in which to add a onClick function to.</returns>
	/// <param name="slot">A GameObject that is instantiated. Must have Text, Image and Button component.</param>
	/// <param name="parent">A Transform that the instantiated object is parented to.</param>
	/// <param name="sprite">The sprite of the item that will be set on the Image component.</param>
	/// <param name="text">The text that will be set on the Text component.</param>
	private Button InstantiateSlot(GameObject slot, Transform parent, Sprite sprite, string text) {

		GameObject newSlot = Instantiate (slot) as GameObject;
		newSlot.transform.SetParent (parent, false);

		// Set the new slot's text and sprite
		newSlot.transform.GetComponentInChildren<Text> ().text = text;
		newSlot.transform.GetChild(0).GetComponent<Image>().sprite = sprite;

		Button craftButton = newSlot.GetComponent<Button> ();
		return craftButton;
	}

	// --------- RESOURCES UI ---------
	public void DisplayResource(string text, bool checkIfAvaliable = false) {
		if (!checkIfAvaliable) {
			resourceDisplay.gameObject.SetActive (true);
			resourceDisplay.text = text;
		} else {
			if (resourceDisplay.gameObject.activeSelf) {
				resourceDisplay.gameObject.SetActive (true);
				resourceDisplay.text = text;
			}
		}
	}

	public void TurnOffResourceDisplay() {
		resourceDisplay.gameObject.SetActive (false);
	}
		

	// --------- UPGRADES UI ---------
	public void ToggleUpgradesDisplay() {
		upgradesDisplaying = !upgradesDisplaying;
		upgradesPanel.SetActive (upgradesDisplaying);
	}

	private void RetrieveUpgradesUI() {
		// Get the panel in the object
		Transform[] children = new Transform[upgradesPanel.transform.childCount - 1];

		// Get all children in panel
		for (int i = 0; i < children.Length; i++) {
			children [i] = upgradesPanel.transform.GetChild (i + 1);

			// Get Buttons
			upgradeButtons.Add (children [i].GetComponent<Button> ());
//			Debug.Log (upgradeButtons[i]);
		}

		// Get Texts
		for (int i = 0; i < children.Length; i++) {
			upgradesTexts.Add (children [i].GetComponentInChildren<Text> ());
		}
	}

	// Display descriptions for each upgrade
	public void DisplayUpgradeText(string[] descriptions) {
		for (int i = 0; i < upgradesTexts.Count; i++) {
			upgradesTexts [i].text = descriptions [i];
		}
	}

	public List<Button> GetUpgradeButtons() {
		return upgradeButtons;
	}

	public void DisableUpgradeButton(UpgradeNames upgradeName) {
		upgradesTexts [(int)upgradeName].color = Color.green;
		upgradeButtons [(int)upgradeName].enabled = false;
	}

	public IEnumerator UpgradeUnavaliableFlash(UpgradeNames upgradeName) {
		Text upgradeText = upgradesTexts [(int)upgradeName];
		Color originalColor = upgradeText.color;

		for (int i = 0; i < 3; i++) {
			upgradeText.color = Color.red;
			yield return new WaitForSeconds (upgradeFlashTime);
			upgradeText.color = originalColor;
		}
	}


	// --------- INVENTORY UI ---------
	public void ToggleInventoryDisplay() {
		inventoryDisplaying = !inventoryDisplaying;
		inventoryGroup.SetActive (inventoryDisplaying);
		if (inventoryDisplaying) {
			DisplayInventory ();
		}
	}

	private void DisplayInventory() {
		ClearDisplay (inventoryPanel);
		List<string> inventoryItems = inventoryDatabase.GenerateInventoryStrings ();
		List<Sprite> inventorySprites = inventoryDatabase.GenerateInventorySprites ();
		Image slotImage;
		Text descriptionText;

		for (int i = 0; i < inventoryItems.Count; i++) {
			GameObject newSlot = Instantiate (inventorySlot) as GameObject;
			newSlot.transform.SetParent(inventoryPanel.transform, false);

			slotImage = newSlot.GetComponent<Image> ();
			descriptionText = newSlot.GetComponentInChildren<Text> ();

			slotImage.sprite = inventorySprites [i];
			descriptionText.text = inventoryItems [i];
		}

	}

	// ---------------------------------------------------------------------------------------------------

	// --------- TASKS UI ---------
	public void ToggleTasksDisplay() {
		taskDisplaying = !taskDisplaying;
		taskGroup.SetActive (taskDisplaying);

		if (taskDisplaying) {
			taskUIGroup.ToggleGroup (false);
			taskItemUIGroup.ToggleGroup (false);
			itemUIGroup.ToggleGroup (false);
			DisplayTasks ();
		}
	}

	private void DisplayTasks() {
		ClearDisplay (taskPanel);
		List<string> taskDescriptions = taskDatabase.GenerateTaskStrings ();

		for (int i = 0; i < taskDescriptions.Count; i++) {
			GameObject newSlot = Instantiate (taskSlot) as GameObject;
			newSlot.transform.SetParent(taskPanel.transform, false);

			newSlot.GetComponentInChildren<Text> ().text = taskDescriptions [i];
			int index = i;
			newSlot.GetComponent<Button> ().onClick.AddListener (delegate {
				DisplayTaskBreakdown(index);
			});
		}

	}

	public void DisplayTaskBreakdown(int taskID) {
		taskUIGroup.ToggleGroup (true);

		ClearDisplay (taskUIGroup.ItemsPanel);
		Task task = taskDatabase.FetchTaskByID (taskID);

		Dictionary<TaskItem, int> itemList = task.TaskItems;

		// Create TaskItems list
		foreach (TaskItem key in itemList.Keys) {
			Button slotButton = InstantiateSlot (taskUIGroup.Slot, taskUIGroup.ItemsPanel.transform, key.Sprite, itemList [key].ToString ());

			slotButton.onClick.AddListener (delegate {
				CheckTaskItemCraftingAvailable (key.ID);
			});
		}
	}


	// ------ TASKITEMS UI ------

	// Populates the TaskItem breakdown panel
	public void DisplayTaskItemBreakdown(int taskItemID) {
		taskItemUIGroup.groupObject.SetActive (true);
		ClearDisplay (taskItemUIGroup.ItemsPanel);

		TaskItem taskItem = itemDatabase.FetchTaskItemByID (taskItemID);
		Debug.Assert (taskItem.ID != -1, "Item id: " + taskItemID + " is not a TaskItem.");

		taskItemUIGroup.SetBreakdownText (taskItemID);
		CreateItemList (taskItem);

		taskItemUIGroup.CraftButton.onClick.AddListener (delegate {
			CheckTaskItemCraftingAvailable(taskItemID);
		});
	}
		
	// Populates the ItemsPanel with the required items from the given TaskItem
	private void CreateItemList(TaskItem taskItem) {
		Dictionary<CraftingItem, int> itemsRequired = taskItem.CraftingItems;
		KeyValuePair<Item, int> retrievedItem;

		// Loop through each key
		foreach (CraftingItem key in itemsRequired.Keys) {
//				Debug.Log (key.Title);
			string itemAmount = "";
			// Get the item from player's inventory
			retrievedItem = inventoryDatabase.FetchItemWithQuantityByID (key.ID);
			bool itemFound = true;

			// Check if the item IS in their inventory
			// if it IS then show how many there is
			// if not then set 0
			if (retrievedItem.Key.ID != -1) {
				itemAmount += retrievedItem.Value.ToString () + "/";
			} else {
				itemAmount += "0/";
				itemFound = false;
			}

			// Add the number of that item required to the string
			itemAmount += itemsRequired [key].ToString ();

			// Check if player has sufficient number of this item
			// true: set green
			// false: set red
			if (itemFound && (retrievedItem.Value >= itemsRequired [key])) {
				itemAmount = SetStringColor (itemAmount, "green");
			} else {
				itemAmount = SetStringColor (itemAmount, "red");
			}

			Button instantiatedButton = InstantiateSlot (taskItemUIGroup.Slot, taskItemUIGroup.ItemsPanel.transform, key.Sprite, itemAmount);

			// Check if item is craftable
			if (key.Craftable) {
				instantiatedButton.onClick.AddListener (delegate {
					DisplayItemBreakdown (key.ID);
				});
			} else {
				instantiatedButton.enabled = false;
			}

		}
	}

	// ------ ITEMS UI ------
	
	public void DisplayItemBreakdown(int itemID) {
		itemUIGroup.ToggleGroup (true);
		itemUIGroup.SetBreakdownText (itemID);
	}

	// ------ ITEM WARNING ------

	public void CheckTaskItemCraftingAvailable(int taskItemID) {
		taskItemCraftWarning.ToggleGroup (true);

		TaskItem taskItem = itemDatabase.FetchTaskItemByID (taskItemID);
		Debug.Assert (taskItem.ID != -1, "Unknown TaskItem ID: " + taskItemID + "; Should not happen.");

		bool hasUncraftable = false;
		bool insufficientResources = false;
		string description = "";
		string title = "";

		// Clear itemsPanel
		ClearDisplay(taskItemCraftWarning.ItemsPanel);

		// Check if player meets Item Requirements (i.e. player has missing items that cannot be crafted)
		Dictionary<CraftingItem, int> missingItems = CheckItemRequirements(taskItem);
		foreach (CraftingItem key in missingItems.Keys) {
			
			// Check if there is a key signifying that there are uncraftable items
			if (key.ID == -1 && missingItems [key] == -1) {
				hasUncraftable = true;
				description += "There are items missing that are uncraftable. We cannot complete this without them.";
				break;
			}
		}

		// check if player has insufficient resources, including the missing items
		Dictionary<ResourceType, int> totalResources = CheckResourceRequirements(taskItem, missingItems);

		// Check if there is a key signifying that there are insufficient resources
		if (totalResources.ContainsKey (ResourceType.Unknown)) {
			insufficientResources = true;
			description += "We do not have enough resources to complete this action. We will need to gather more.";
		}
			

		// Check if play has missing items (that are craftable)
		if (missingItems.Count > 0) {
			if (!hasUncraftable && !insufficientResources) {
				description = "We are missing some items, but we have enough resources to make them.";
				title = "Warning: Items Missing.";
				taskItemCraftWarning.SetBreakdownText (taskItemID, totalResources, description, title);
			} else {
				
				if (insufficientResources) {
					title = "Error: Insufficient Resources!";
				} else if (hasUncraftable) {
					title = "Error: Items Unavailable!";
				}
				taskItemCraftWarning.SetBreakdownText (taskItemID, totalResources, description, title, false);

			}

		
		// Player has no missing items (and sufficient resources)
		} else {
			// Craft the TaskItem !!!!!!!!!!!!!!!!!!!!

		}

		taskItemCraftWarning.CraftButton.onClick.AddListener (delegate {
			taskManager.AddActiveTaskItem (taskItem, 1);
			taskItemCraftWarning.ToggleGroup(false);
			taskUIGroup.ToggleGroup(false);
		});
	}

	private Dictionary<ResourceType, int> CheckResourceRequirements(TaskItem taskItem, Dictionary<CraftingItem, int> craftingItems) {
		// Calculate total resources required to craft the TaskITem
		Dictionary<ResourceType, int> totalResources = taskItem.RequiredResources;

		// Loop through each missing item
		foreach (CraftingItem key in craftingItems.Keys) {
			if (key.ID != -1 && key.Craftable) {
				Dictionary<ResourceType, int> requiredResources = key.RequiredResources;

				// loop through each resource required to craft this item
				foreach (ResourceType resourceType in requiredResources.Keys) {
					// Check if total resources list has the current resource type
					if (totalResources.ContainsKey (resourceType)) {
						totalResources [resourceType] += craftingItems [key] * requiredResources [resourceType];

					// if not then add a new resource type
					} else {
						totalResources.Add (resourceType, craftingItems [key] * requiredResources [resourceType]);
					}
				}
			}
		}

		foreach (ResourceType key in totalResources.Keys) {
			Resource resource = resourceDatabase.FetchResourceByID ((int)key);
			// Check if player has enough resources
			if (resource.Quantity < totalResources [key]) {
				
				// Add a key to the total resources list to signify that there are insufficient resources
				totalResources.Add (ResourceType.Unknown, -1);
				break;
			}
		}

		return totalResources;
	}

	private Dictionary<CraftingItem, int> CheckItemRequirements(TaskItem taskItem) {
		// Check if player is missing crafting items
		Dictionary<CraftingItem, int> missingItems = new Dictionary<CraftingItem, int>();
		Dictionary<CraftingItem, int> uncraftables = new Dictionary<CraftingItem, int>();
		Dictionary<CraftingItem, int> requiredItems = taskItem.CraftingItems;
		bool cannotCraft = false;

		// loop through each item required for crafting
		foreach (CraftingItem key in requiredItems.Keys) {

			bool itemMissing = false;
			KeyValuePair<Item, int> craftingItem = inventoryDatabase.FetchItemWithQuantityByID (key.ID);

			// if item to look for is craftable
			if (key.Craftable) {
				// check if TaskItem cannot be crafted
				if (!cannotCraft) {
					// Check if item exists in inventory
					if (craftingItem.Key.ID == -1) {
						missingItems.Add (key, requiredItems [key]);
						itemMissing = true;

						// Check if there is enough items in player's inventory
					} else if (craftingItem.Value < requiredItems [key]) {
						missingItems.Add (key, requiredItems [key] - craftingItem.Value);
						itemMissing = true;
					}
				}

				// if item to look for is NOT craftable
			} else {

				// check if inventory has the item OR if the player has enough of the item
				if (craftingItem.Key.ID == -1) {
					itemMissing = true;
					uncraftables.Add (key, requiredItems [key]);

				} else if (craftingItem.Value < requiredItems [key]) {
					itemMissing = true;
					uncraftables.Add (key, requiredItems [key] - craftingItem.Value);
				}

				if (itemMissing) {
					if (!cannotCraft) {
						// Clear itemsPanel to only show uncraftable items
						ClearDisplay (taskItemCraftWarning.ItemsPanel);
						cannotCraft = true;
					}
				}
			}

			// if item is doesn't exist in player's inventory OR player does not have enough 
			// instantiate a slot to display the item
			if (cannotCraft) {
				InstantiateSlot (
					taskItemCraftWarning.Slot, 
					taskItemCraftWarning.ItemsPanel.transform, 
					key.Sprite, 
					SetStringColor (uncraftables [key].ToString (), "red")
				);
			} else {
				if (itemMissing) {
					InstantiateSlot (
						taskItemCraftWarning.Slot, 
						taskItemCraftWarning.ItemsPanel.transform, 
						key.Sprite, 
						SetStringColor (missingItems [key].ToString (), "red")
					);
				}
			}
		}

		if (cannotCraft) {
			uncraftables.Add (new CraftingItem (), -1);
			return uncraftables;
		} else {
			return missingItems;
		}
	}
	
//	public void DisplayTaskItemCraftWarning(int taskItemID) {
//		taskItemCraftWarning.ToggleGroup (true);
//		TaskItem taskItem = itemDatabase.FetchTaskItemByID (taskItemID);
//		Debug.Assert (taskItem.ID != -1, "Unknown TaskItem ID: " + taskItemID + "; Should not happen.");
//
//		// clear itemsPanel
//		ClearDisplay(taskItemCraftWarning.ItemsPanel);
//
//		// Check if player is missing crafting items
//		Dictionary<CraftingItem, int> missingItems = new Dictionary<CraftingItem, int>();
//		Dictionary<CraftingItem, int> requiredItems = taskItem.CraftingItems;
//
//		// loop through each item required for crafting
//		foreach (CraftingItem key in requiredItems.Keys) {
//			bool itemMissing = false;
//			KeyValuePair<Item, int> craftingItem = inventoryDatabase.FetchItemWithQuantityByID (key.ID);
//
//			// Check if item exists in inventory
//			if (craftingItem.Key.ID == -1) {
//				missingItems.Add (key, requiredItems[key]);
//				itemMissing = true;
//
//			// Check if there is enough items in player's inventory
//			} else if (craftingItem.Value < requiredItems [key]) {
//				missingItems.Add (key, requiredItems [key] - craftingItem.Value);
//				itemMissing = true;
//			}
//
//			// if item is doesn't exist in player's inventory OR player does not have enough 
//			// instantiate a slot to display the item
//			if (itemMissing) {
//				InstantiateSlot (
//					taskItemCraftWarning.Slot, 
//					taskItemCraftWarning.ItemsPanel.transform, 
//					key.Sprite, 
//					SetStringColor (missingItems [key].ToString (), "red")
//				);
//			}
//		}
//
//		// Calculate total resources required to craft the TaskITem
//		Dictionary<ResourceType, int> totalResources = taskItem.RequiredResources;
//		Dictionary<ResourceType, int> currentResources = new Dictionary<ResourceType, int> ();
//
//		// Get the relevant resources from the resource database according to what resources are required to craft
//		foreach (ResourceType key in totalResources.Keys) {
//			Resource resource = resourceDatabase.FetchResourceByID ((int)key);
//
//			// Can check resources here ------------
//
//			currentResources.Add (key, resource.Quantity);
//		}
//
//		// Loop through each missting item
//		foreach (CraftingItem key in missingItems.Keys) {
////			Resource resource = resourceDatabase.FetchResourceByID ((int)key);
//			Dictionary<ResourceType, int> requiredResources = key.RequiredResources;
//
//			// loop through each resource required to craft this item
//			foreach(ResourceType resourceType in requiredResources.Keys) {
//				// Check if total resources list has the current resource type
//				if (totalResources.ContainsKey(resourceType)) {
//					totalResources[resourceType] += missingItems[key] * requiredResources[resourceType];
//
//				// if not then add a new resource type
//				} else {
//					totalResources.Add(resourceType, missingItems[key] * requiredResources[resourceType]);
//				}
//			}
//			
//
//		}
//
//		// Display the text
////		taskItemCraftWarning.SetBreakdownText(taskItemID, totalResources);
//
//		// Hook up craftButton
//	}

}

[System.Serializable]
public class ItemUIGroup : System.Object {
	public GameObject groupObject;

	protected Image icon;
	protected List<Text> textList = new List<Text> ();
	protected Button craftButton;
	protected ItemDatabase itemDatabase;
	protected ResourceDatabase resourceDatabase;

	public bool isDisplaying = true;

	public Image Icon {
		get { return icon; }
	}

	public List<Text> TextList {
		get { return textList; }
	}

	public Button CraftButton {
		get { return craftButton; }
	}

	public virtual void GetChildren() {
		GameObject databases = GameObject.FindGameObjectWithTag ("Databases");
		itemDatabase = databases.GetComponent<ItemDatabase> ();
		resourceDatabase = databases.GetComponent<ResourceDatabase> ();

		Transform groupTransform = groupObject.transform;

		// Get Image for Icon
		icon = groupTransform.GetChild (0).GetComponent<Image> ();

		// Get Texts
		GameObject texts = groupTransform.GetChild (1).gameObject;
		for (int i = 0; i < 3; i++) {
			textList.Add(texts.transform.GetChild(i).GetComponent<Text>());
		}

		// Get Button
		craftButton = groupTransform.GetChild (2).GetComponent<Button> ();
	}

	public virtual void SetBreakdownText (int itemID) {

		CraftingItem craftingItem = itemDatabase.FetchCraftingItemByID (itemID);
		Debug.Assert (craftingItem.ID != -1, "Invalid id: " + itemID);

		string resourceList = GenerateResourceList(craftingItem.RequiredResources);

		icon.sprite = craftingItem.Sprite;
		textList [0].text = craftingItem.Title;
		textList [1].text = craftingItem.Description;
		textList [2].text = resourceList;
	}

	protected virtual string GenerateResourceList(Dictionary<ResourceType, int> requiredResources) {
		string resourceList = "";

		// For the Resource List
		// Loop through all the keys in the Dictionary
		foreach (ResourceType key in requiredResources.Keys) {

			// check if resource type is not unknown
			if (key != ResourceType.Unknown) {
				// Get the resource
				Resource resource = resourceDatabase.FetchResourceByID ((int)key);
				Debug.Assert (resource.ID != -1, "Unkown resource ID: " + (int)key);

				// Displays: Title, amount the player has, amount required
				string resourceDetail = resource.Title + ": " + resource.Quantity + " / " + requiredResources [key];

				// Check if the player's resource is greater than the required amount of resource
				// Sets the respective resource to the color green or red based on condition
				if (resource.Quantity >= requiredResources [key]) {
					resourceList += SetStringColor (resourceDetail, "green") + "\n";
				} else {
					resourceList += SetStringColor (resourceDetail, "red") + "\n";
				}
			}
		}

		return resourceList;
	}


	public virtual void ToggleGroup(bool state) {
		groupObject.SetActive (state);
	}

	// Uses string markup to change color of text
	protected string SetStringColor(string text, string color) {
		text = "<color=" + color + ">" + text + "</color>";
		return text;
	}
}

[System.Serializable]
public class TaskItemUIGroup : ItemUIGroup {
	protected GameObject itemsPanel;
	public GameObject slot;

	public GameObject ItemsPanel {
		get { return itemsPanel; }
	}

	public GameObject Slot {
		get { return slot; }
	}

	public override void GetChildren() {
		base.GetChildren ();

		itemsPanel = groupObject.transform.GetChild (3).gameObject;
	}

	public override void SetBreakdownText (int taskItemID) {

		TaskItem taskItem = itemDatabase.FetchTaskItemByID (taskItemID);
		Debug.Assert (taskItem.ID != -1, "Item id: " + taskItemID + " is not a TaskItem.");

		string resourceList = GenerateResourceList(taskItem.RequiredResources);

		// Displays the texts in their respective Text; is in a specific order based on the hierarchy 
		textList [0].text = taskItem.Title; // Name
		textList [1].text = taskItem.Description; // Description
		textList [2].text = resourceList; // Resources required
		
	}
}

[System.Serializable]
public class TaskUIGroup : TaskItemUIGroup {
	protected Slider progressSlider;
	protected TaskDatabase taskDatabase;

	public Slider ProgressSlider {
		get { return progressSlider; }
	}

	public override void GetChildren() {
		taskDatabase = GameObject.FindGameObjectWithTag ("Databases").GetComponent<TaskDatabase> ();
		
		base.GetChildren ();

		itemsPanel = groupObject.transform.GetChild (2).gameObject;
		progressSlider = groupObject.transform.GetChild (3).GetComponent<Slider> ();
	}

	public override void SetBreakdownText (int taskID) {
		Task task = taskDatabase.FetchTaskByID (taskID);
		Debug.Assert (task.ID != -1, "Unknown taskID: " + taskID);

		textList [0].text = task.Title;
		textList [1].text = task.Description;
		textList [2].text = Mathf.FloorToInt(task.Progress).ToString() + "%";

		// Display progress bar
		Vector2 progress = taskDatabase.GetTaskProgressByID (taskID);
		progressSlider.value = progress.x / progress.y;
	}

}

[System.Serializable]
public class TaskItemCraftWarning : TaskItemUIGroup {

	protected Button cancelButton;
	protected Text title;

	public override void GetChildren ()
	{
		base.GetChildren ();

		title = groupObject.transform.GetChild (1).transform.GetChild(5).GetComponent<Text>();

		cancelButton = groupObject.transform.GetChild (4).GetComponent<Button> ();
	}

	public void SetBreakdownText (int taskItemID, Dictionary<ResourceType, int> resourcesRequired, string description, string titleText, bool canCraft = true) {
		
		TaskItem taskItem = itemDatabase.FetchTaskItemByID (taskItemID);
		Debug.Assert (taskItem.ID != -1, "Item id: " + taskItemID + " is not a TaskItem.");

		string resourceList = GenerateResourceList(resourcesRequired);

		// Displays the texts in their respective Text; is in a specific order based on the hierarchy 
		textList [0].text = taskItem.Title; // Name
		textList [1].text = description; // Description
		textList [2].text = resourceList; // Resources required

		title.text = titleText; // Change title

		craftButton.enabled = canCraft;
		
		cancelButton.onClick.AddListener (delegate {
			CancelButton ();
		});
	}

	public void CancelButton() {
		this.ToggleGroup (false);
	}
}