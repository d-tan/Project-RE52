using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {

	public Image[] indicators = new Image[2];
	private Color[] indicatorColors = new Color[4];

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
	public GameObject taskBreakDownGroup;
	public GameObject taskItemSlot;
	private Image taskIcon;
	private Slider progressSlider;
	private GameObject taskItemsPanel;
	private GameObject taskTexts;
	TaskDatabase taskDatabase;

	private bool taskDisplaying = true;

	// TaskItem
	public GameObject taskItemGroup;
	private GameObject taskItemPanel;
	private Image taskItemIcon;
	private GameObject itemsPanel;
	private List<Text> taskItemsUIText = new List<Text> ();
	private Button taskCraftButton;
	ItemDatabase itemDatabase;

	private bool taskItemDisplaying = true;

	// Item
	public GameObject itemGroup;
	private Image itemIcon;
	private List<Text> itemUIText = new List<Text> ();
	private Button itemCraftButton;

	private bool itemDisplaying = true;

	void Start() {
		indicatorColors [0] = new Color (1.0f, 103.0f / 255.0f, 103.0f / 255.0f, 160.0f / 255.0f);
		indicatorColors [1] = new Color (54.0f / 255.0f, 188.0f / 255.0f, 71.0f / 255.0f, 160.0f / 255.0f);
		indicatorColors [2] = new Color (241.0f / 255.0f, 244.0f / 255.0f, 48.0f / 255.0f, 160.0f / 255.0f);
		indicatorColors [3] = new Color (184.0f / 255.0f, 184.0f / 255.0f, 184.0f / 255.0f, 160.0f / 255.0f);

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

		// Items
		GetItemBreakdownChildren();
		ToggleItemDisplay ();

		// TaskItems
		GetTaskItemBreakdownChildren();
		ToggleTaskItemsDisplay ();
		itemDatabase = databases.GetComponent<ItemDatabase> ();

		// Tasks
		GetTaskBreakdownChildren();
		ToggleTasksDisplay();
		taskDatabase = databases.GetComponent<TaskDatabase>();


	}


	public void ChangeIndicatorColors(bool active, RubbishType type1 = RubbishType.General, RubbishType type2 = RubbishType.General) {
		
		// Check if indicators are active
		if (active) {
			switch (type1) {
			case RubbishType.Organic:
				indicators [0].color = indicatorColors [(int)type2];
				indicators [1].color = indicatorColors [(int)type1];
				break;
			case RubbishType.Recycle:
				indicators [0].color = indicatorColors [(int)type1];
				indicators [1].color = indicatorColors [(int)type2];
				break;
			default:
				indicators [0].color = indicatorColors [(int)type1];
				indicators [1].color = indicatorColors [(int)type2];
				break;
			}
		} else {
			// Set them to Grey
			for (int i = 0; i < indicators.Length; i++) {
				indicators [i].color = indicatorColors [3];
			}
		}

	}

	// --------- GENERAL UI METHODS ---------
	private void ClearDisplay(GameObject panel) {
		for (int i = 0; i < panel.transform.childCount; i++) {
			Destroy (panel.transform.GetChild (0).gameObject);
		}
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


	// --------- TASKS UI ---------
	public void ToggleTasksDisplay() {
		taskDisplaying = !taskDisplaying;
		itemGroup.SetActive (false);
		taskItemGroup.SetActive (false);
		taskBreakDownGroup.SetActive (false);
		taskGroup.SetActive (taskDisplaying);

		if (taskDisplaying) {
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

	// Gets a reference to all children in the task breakdown panel. The children have a SPECIFIC order
	private void GetTaskBreakdownChildren() {
		Transform breakdownTransform = taskBreakDownGroup.transform;

		taskIcon = breakdownTransform.GetChild (0).GetComponent<Image> ();
		progressSlider = breakdownTransform.GetChild (1).GetComponent<Slider> ();
		taskItemsPanel = breakdownTransform.GetChild (2).gameObject;
		taskTexts = breakdownTransform.GetChild (3).gameObject;
	}

	public void DisplayTaskBreakdown(int taskID) {
		taskBreakDownGroup.SetActive (true);
		taskItemGroup.SetActive (false);
		itemGroup.SetActive (false);
		ClearDisplay (taskItemsPanel);
		Task task = taskDatabase.FetchTaskByID (taskID);
		List<string> taskDescription = taskDatabase.GenerateBreakdownString(taskID);
		Text textDisplay;

		// Display Texts
		Debug.Assert (taskDescription.Count > 0, "Generated string is empty");
		for (int i = 0; i < taskDescription.Count; i++) {
			textDisplay = taskTexts.transform.GetChild (i).GetComponent<Text> ();

			textDisplay.text = taskDescription [i];
		}

		// Display progress bar
		Vector2 progress = taskDatabase.GetTaskProgressByID (taskID);
		progressSlider.value = progress.x / progress.y;

		Dictionary<TaskItem, int> itemList = task.TaskItems;

		// Create TaskItems list
		foreach (TaskItem key in itemList.Keys) {
			GameObject newSlot = Instantiate (taskItemSlot) as GameObject;
			newSlot.transform.SetParent (taskItemsPanel.transform, false);

			newSlot.GetComponentInChildren<Text> ().text = itemList [key].ToString();

			newSlot.GetComponent<Button> ().onClick.AddListener (delegate {
				DisplayTaskItemBreakdown (key.ID);
			});
		}
	}


	// ------ TASKITEMS UI ------
	public void ToggleTaskItemsDisplay() {
		taskItemDisplaying = !taskItemDisplaying;
		taskItemGroup.SetActive (taskItemDisplaying);

	}

	// Gets a reference to relevant children in the Task Items panel. The children have a SPECIFIC order
	private void GetTaskItemBreakdownChildren() {
		Transform breakdownTransform = taskItemGroup.transform;

		taskItemIcon = breakdownTransform.GetChild (0).gameObject.GetComponent<Image>();
		Transform texts = breakdownTransform.GetChild (1);
		for (int i = 0; i < 3; i++) {
			taskItemsUIText.Add (texts.GetChild (i).GetComponent<Text> ());
		}

		taskCraftButton = breakdownTransform.GetChild (2).GetComponent<Button> ();

		itemsPanel = breakdownTransform.GetChild (3).gameObject;
	}

	// Populates the TaskItem breakdown panel
	public void DisplayTaskItemBreakdown(int taskItemID) {
		taskItemGroup.SetActive (true);
		itemGroup.SetActive (false);
		TaskItem taskItem = itemDatabase.FetchTaskItemByID (taskItemID);
		Debug.Assert (taskItem.ID != -1, "Item id: " + taskItemID + " is not a TaskItem.");
//		List<string> resourceList = itemDatabase.GenerateTaskItemResourceList (taskItemID);

		SetTaskItemUIText (taskItem);
		CreateItemList (taskItem);

	}

	// Populates all relevant texts with the given TaskItem details
	private void SetTaskItemUIText(TaskItem taskItem) {
		string resourceList = "";

		Dictionary<ResourceType, int> resourcesRequired = taskItem.RequiredResources;

		// For the Resource List
		// Loop through all the keys in the Dictionary
		foreach (ResourceType key in resourcesRequired.Keys) {
			// Get the resource
			Resource resource = resourceDatabase.FetchResourceByID ((int)key);
			Debug.Assert (resource.ID != -1, "Unkown resource ID: " + (int)key);

			// Check if the player's resource is greater than the required amount of resource
			// Sets the respective resource to the color green or red based on condition
			if (resource.Quantity >= resourcesRequired [key]) {
				// Displays: Title, amount the player has, amount required
				resourceList += "<color=green>" + resource.Title + ": " + resource.Quantity + " / " + resourcesRequired [key] +
				"</color>\n";
			} else {
				resourceList += "<color=red>" + resource.Title + ": " + resource.Quantity + " / " + resourcesRequired [key] +
				"</color>\n";
			}
		}

		// Displays the texts in their respective Text; is in a specific order based on the hierarchy 
		taskItemsUIText [0].text = taskItem.Title; // Name
		taskItemsUIText [1].text = taskItem.Description; // Description
		taskItemsUIText [2].text = resourceList; // Resources required
	}

	// Populates the ItemsPanel with the required items from the given TaskItem
	private void CreateItemList(TaskItem taskItem) {
		ClearDisplay (itemsPanel);
		Dictionary<CraftingItem, int> itemsRequired = taskItem.CraftingItems;
		KeyValuePair<Item, int> retrievedItem;

		// Loop through each key
		foreach (CraftingItem key in itemsRequired.Keys) {
			string itemAmount = "";
			// Get the item from player's inventory
			retrievedItem = inventoryDatabase.FetchItemWithQuantityByID (key.ID);
			bool itemFound = true;

			// Check if the item IS in their inventory
			// if it IS then show how many there is
			// if not then set 0
			if (retrievedItem.Key.ID != -1) {
				itemAmount += retrievedItem.Value.ToString() + "/";
			} else {
				itemAmount += "0/";
				itemFound = false;
			}

			// Add the number of that item required to the string
			itemAmount += itemsRequired [key].ToString ();

			// Check if player has sufficient number of this item
			// true: set green
			// false: set false
			if (itemFound && (retrievedItem.Value >= itemsRequired [key])) {
				itemAmount = "<color=green>" + itemAmount + "</color>";
			} else {
				itemAmount = "<color=red>" + itemAmount + "</color>";
			}

			// Create new slot
			GameObject newSlot = Instantiate (taskItemSlot) as GameObject;
			newSlot.transform.SetParent (itemsPanel.transform, false);

			// Set the new slot's text and sprite
			newSlot.transform.GetComponentInChildren<Text> ().text = itemAmount;
			newSlot.transform.GetChild(0).GetComponent<Image>().sprite = key.Sprite;

			Button craftButton = newSlot.GetComponent<Button> ();
			// Check if item is craftable
			if (key.Craftable) {
				Debug.Log ("Crafting Item is craftable. Adding Method to listener");
				craftButton.onClick.AddListener (delegate {
					DisplayItemBreakdown (key.ID);
				});
			} else {
				craftButton.enabled = false;
			}

		}
	}

	// ------ ITEMS UI ------
	public void ToggleItemDisplay() {
		itemDisplaying = !itemDisplaying;
		itemGroup.SetActive (itemDisplaying);

	}

	private void GetItemBreakdownChildren() {
		Transform breakdown = itemGroup.transform;

		// Get Image for Icon
		itemIcon = breakdown.GetChild (0).GetComponent<Image> ();

		// Get Texts
		GameObject texts = breakdown.GetChild (1).gameObject;
		for (int i = 0; i < 3; i++) {
			itemUIText.Add(texts.transform.GetChild(i).GetComponent<Text>());
		}

		// Get Button
		itemCraftButton = breakdown.GetChild (2).GetComponent<Button> ();
	}
	
	public void DisplayItemBreakdown(int itemID) {
		itemGroup.SetActive (true);
		CraftingItem craftingItem = itemDatabase.FetchCraftingItemByID (itemID);
		Debug.Assert (craftingItem.ID != -1, "Invalid id: " + itemID);

		string resourceList = "";

		Dictionary<ResourceType, int> resourcesRequired = craftingItem.RequiredResources;

		// For the Resource List
		// Loop through all the keys in the Dictionary
		foreach (ResourceType key in resourcesRequired.Keys) {
			// Get the resource
			Resource resource = resourceDatabase.FetchResourceByID ((int)key);
			Debug.Assert (resource.ID != -1, "Unkown resource ID: " + (int)key);

			// Check if the player's resource is greater than the required amount of resource
			// Sets the respective resource to the color green or red based on condition
			if (resource.Quantity >= resourcesRequired [key]) {
				// Displays: Title, amount the player has, amount required
				resourceList += "<color=green>" + resource.Title + ": " + resource.Quantity + " / " + resourcesRequired [key] +
					"</color>\n";
			} else {
				resourceList += "<color=red>" + resource.Title + ": " + resource.Quantity + " / " + resourcesRequired [key] +
					"</color>\n";
			}
		}

		itemIcon.sprite = craftingItem.Sprite;
		itemUIText [0].text = craftingItem.Title;
		itemUIText [1].text = craftingItem.Description;
		itemUIText [2].text = resourceList;
	}
}
