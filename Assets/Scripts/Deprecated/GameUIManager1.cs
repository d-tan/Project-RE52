//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;
//
//public class GameUIManager : MonoBehaviour {
//
////	public Image[] indicators = new Image[2];
////	private Color[] indicatorColors = new Color[4];
//
//	public Text resourceDisplay;
//
//	// Menu Backtrack
//	List<GameObject> panelHierachy = new List<GameObject> ();
//	public GameObject menuBackTrackPanel;
//
//	// Panel Swapping
//	private enum Panels { // Alphabetical order
//		Inventory_,
//		Resources_,
//		Tasks_,
//		Upgrades_
//	}
//
//	delegate void PanelToggle (bool toggling);
//	private List<PanelToggle> toggleFunctions = new List<PanelToggle>();
//	private int panelEnumLength = System.Enum.GetValues (typeof(Panels)).Length;
//
//	// Upgrades
//	public UpgradesUI upgradesUI;
//	UpgradeDatabase upgradeDatabase;
//	UpgradeManager upgradeManager;
//
//	public GameObject upgradesPanel;
//	private float upgradeFlashTime = 0.5f;
//	List<Text> upgradesTexts = new List<Text>();
//	List<Button> upgradeButtons = new List<Button> ();
//
//	private bool upgradesDisplaying = true;
//
//	// Inventory
//	public GameObject inventoryGroup;
//	public GameObject inventoryPanel;
//	public GameObject inventorySlot;
//	InventoryDatabase inventoryDatabase;
//
//	private bool inventoryDisplaying = true;
//
//	// Resources
//	ResourceDatabase resourceDatabase;
//
//	// Tasks
//	public GameObject taskGroup;
//	public GameObject taskPanel;
//	public GameObject taskSlot;
//
//	public TaskUIGroup taskUIGroup;
//	TaskDatabase taskDatabase;
//	TaskManager taskManager;
//
//	private bool taskDisplaying = true;
//
//	// TaskItem
//	public TaskItemUIGroup taskItemUIGroup;
//	public TaskItemCraft taskItemCraft;
//	ItemDatabase itemDatabase;
//
//	// Item
//	public ItemUIGroup itemUIGroup;
//
//	void Start() {
////		indicatorColors [0] = new Color (1.0f, 103.0f / 255.0f, 103.0f / 255.0f, 160.0f / 255.0f);
////		indicatorColors [1] = new Color (54.0f / 255.0f, 188.0f / 255.0f, 71.0f / 255.0f, 160.0f / 255.0f);
////		indicatorColors [2] = new Color (241.0f / 255.0f, 244.0f / 255.0f, 48.0f / 255.0f, 160.0f / 255.0f);
////		indicatorColors [3] = new Color (184.0f / 255.0f, 184.0f / 255.0f, 184.0f / 255.0f, 160.0f / 255.0f);
//
//		GameObject databases = GameObject.FindGameObjectWithTag ("Databases");
//
//		// Resources
//		resourceDisplay.gameObject.SetActive (false);
//
//		// Upgrades
////		upgradeWarning.GetChildren();
////		upgradeWarning.ToggleGroup (false);
//		upgradesUI.GetCraftPanelChildren();
//		upgradesUI.GetUpgradeChildren ();
//		upgradesUI.ToggleCraftPanel (false);
//		upgradeManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<UpgradeManager> ();
//		upgradeDatabase = databases.GetComponent<UpgradeDatabase> ();
//		AddUpgradeFunctions ();
//		ToggleUpgradesDisplay ();
//
//		// Inventory
//		ToggleInventoryDisplay();
//		inventoryDatabase = databases.GetComponent<InventoryDatabase>();
//
//		// Resources
//		resourceDatabase = databases.GetComponent<ResourceDatabase>();
//
//		// Items
//		itemUIGroup.GetChildren();
//		itemUIGroup.ToggleGroup (false);
//
//		// TaskItems
//		taskItemCraft.GetCraftPanelChildren();
//		taskItemCraft.ToggleCraftPanel (false);
//		taskItemUIGroup.GetChildren();
//		taskItemUIGroup.ToggleGroup (false);
//		itemDatabase = databases.GetComponent<ItemDatabase> ();
//
//		// Tasks
//		taskUIGroup.GetChildren();
//		taskUIGroup.ToggleGroup (false);
//		ToggleTasksDisplay ();
//		taskDatabase = databases.GetComponent<TaskDatabase>();
//		taskManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TaskManager> ();
//
//		GeneratePanelList ();
//	}
//
//
////	public void ChangeIndicatorColors(bool active, RubbishType type1 = RubbishType.General, RubbishType type2 = RubbishType.General) {
////		
////		// Check if indicators are active
////		if (active) {
////			switch (type1) {
////			case RubbishType.Organic:
////				indicators [0].color = indicatorColors [(int)type2];
////				indicators [1].color = indicatorColors [(int)type1];
////				break;
////			case RubbishType.Recycle:
////				indicators [0].color = indicatorColors [(int)type1];
////				indicators [1].color = indicatorColors [(int)type2];
////				break;
////			default:
////				indicators [0].color = indicatorColors [(int)type1];
////				indicators [1].color = indicatorColors [(int)type2];
////				break;
////			}
////		} else {
////			// Set them to Grey
////			for (int i = 0; i < indicators.Length; i++) {
////				indicators [i].color = indicatorColors [3];
////			}
////		}
////
////	}
//
//	// --------- MENU BACKTRACK ---------
//
//	public void AddPanelToHierachy(GameObject panel) {
////		Debug.Log ("Adding: " + panel.name);
//		panelHierachy.Add (panel);
//
//		// Turn on backtrack panel
//		menuBackTrackPanel.SetActive (true);
//
//		// Set backtrack panel to be behind the given panel
//		menuBackTrackPanel.transform.SetSiblingIndex(panel.transform.GetSiblingIndex () - 1);
//	}
//
//	public void TurnOffTopPanel() {
//		if (panelHierachy.Count > 0) {
//			// Get the last item in the list and turn it off
//			panelHierachy [panelHierachy.Count - 1].SetActive (false);
//
////			Debug.Log ("Removing: " + panelHierachy [panelHierachy.Count - 1].name + " | " + (panelHierachy.Count - 1));
//			// remove from list
//			panelHierachy.RemoveAt (panelHierachy.Count - 1);
//
//			// Check if list is now empty
//			if (panelHierachy.Count == 0) {
//				// reset it and turn it off
//				menuBackTrackPanel.transform.SetSiblingIndex(0);
//				menuBackTrackPanel.SetActive (false);
//
//			// List is not empty
//			} else {
//				
//				// Set backtrack panel to be behind new top panel
//				menuBackTrackPanel.transform.SetSiblingIndex(
//					panelHierachy [panelHierachy.Count - 1].transform.GetSiblingIndex ());
////				Debug.Log ("Now behind: " + panelHierachy [panelHierachy.Count - 1].name);
//			}
//		}
//	}
//
//
//	// --------- GENERAL UI METHODS ---------
//	private void ClearDisplay(GameObject panel) {
//		for (int i = 0; i < panel.transform.childCount; i++) {
//			GameObject child = panel.transform.GetChild (i).gameObject;
//			child.GetComponent<Button> ().onClick.RemoveAllListeners ();
//			Destroy (child);
//		}
//	}
//
//	/// <summary>
//	/// Sets the color of the given string.
//	/// </summary>
//	/// <returns>The string in the color given.</returns>
//	/// <param name="text">The string to change color.</param>
//	/// <param name="color">The color to change to used in markup.</param>
//	public string SetStringColor(string text, string color) {
//		text = "<color=" + color + ">" + text + "</color>";
//		return text;
//	}
//		
//	/// <summary>
//	/// Instantiates a button slot, usually for holding items.
//	/// </summary>
//	/// <returns>A button in which to add a onClick function to.</returns>
//	/// <param name="slot">A GameObject that is instantiated. Must have Text, Image and Button component.</param>
//	/// <param name="parent">A Transform that the instantiated object is parented to.</param>
//	/// <param name="sprite">The sprite of the item that will be set on the Image component.</param>
//	/// <param name="text">The text that will be set on the Text component.</param>
//	private Button InstantiateSlot(GameObject slot, Transform parent, Sprite sprite, string text) {
//
//		GameObject newSlot = Instantiate (slot) as GameObject;
//		newSlot.transform.SetParent (parent, false);
//
//		// Set the new slot's text and sprite
//		newSlot.transform.GetComponentInChildren<Text> ().text = text;
//		newSlot.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
//
//		Button craftButton = newSlot.GetComponent<Button> ();
//		return craftButton;
//	}
//
//
//	private string ItemAmount(int itemID, int amountRequired) {
//		string itemAmount = "";
//		KeyValuePair<Item, int> retrievedItem = new KeyValuePair<Item, int>();
//		// Get the item from player's inventory
//		retrievedItem = inventoryDatabase.FetchItemWithQuantityByID (itemID);
//		bool itemFound = true;
//
//		// Check if the item IS in their inventory
//		// if it IS then show how many there is
//		// if not then set 0
//		if (retrievedItem.Key.ID != -1) {
//			itemAmount += retrievedItem.Value.ToString () + "/";
//		} else {
//			itemAmount += "0/";
//			itemFound = false;
//		}
//
//		// Add the number of that item required to the string
//		itemAmount += amountRequired.ToString ();
//
//		// Check if player has sufficient number of this item
//		// true: set green
//		// false: set red
//		if (itemFound && (retrievedItem.Value >= amountRequired)) {
//			itemAmount = SetStringColor (itemAmount, "green");
//		} else {
//			itemAmount = SetStringColor (itemAmount, "red");
//		}
//
//		return itemAmount;
//	}
//
//	private string ResourceAmount(int resourceID, int amountRequired) {
//		string resourceAmount = "";
//		Resource resource = resourceDatabase.FetchResourceByID (resourceID);
//		Debug.Assert (resource.ID != -1, "This should not happen. resourceID: " + resourceID + " not found.");
//
//		resourceAmount = resource.Quantity.ToString () + "/" + amountRequired.ToString ();
//
//		if (resource.Quantity < amountRequired) {
//			resourceAmount = SetStringColor (resourceAmount, "red");
//		} else {
//			resourceAmount = SetStringColor (resourceAmount, "green");
//		}
//
//		return resourceAmount;
//	}
//
//	// --------- PANEL SWAP ---------
//
//	private void GeneratePanelList() {
//		toggleFunctions.Add (ToggleInventoryDisplay);
//		toggleFunctions.Add (TurnOffResourceDisplay);
//		toggleFunctions.Add (ToggleTasksDisplay);
//		toggleFunctions.Add (ToggleUpgradesDisplay);
//	}
//
//	/// <summary>
//	/// Turns off all other panels except the one parsed.
//	/// </summary>
//	/// <param name="panel">the panel object to be kept on.</param>
//	private void TurnOffPanels(Panels panel) {
//		for (int i = 0; i < panelEnumLength; i++) {
//			if ((int)panel != i) {
//				toggleFunctions [i] (false);
//			}
//		}
//	}
//
//	// --------- RESOURCES UI ---------
//	public void DisplayResource(string text, bool checkIfAvaliable = false) {
//		if (!checkIfAvaliable) {
//			resourceDisplay.gameObject.SetActive (true);
//			resourceDisplay.text = text;
//		} else {
//			if (resourceDisplay.gameObject.activeSelf) {
//				resourceDisplay.gameObject.SetActive (true);
//				resourceDisplay.text = text;
//			}
//		}
//	}
//
//	public void TurnOffResourceDisplay(bool state = false) {
//		resourceDisplay.gameObject.SetActive (false);
//	}
//		
//
//	// --------- UPGRADES UI ---------
//
//	// Adds Functions to each button in the upgrades panel
//	private void AddUpgradeFunctions() {
//		upgradesUI.head.onClick.AddListener (() => BaseUpgradeFunction (UpgradeSection.Head));
//		upgradesUI.leftArm.onClick.AddListener (() => BaseUpgradeFunction (UpgradeSection.Left_Arm));
//		upgradesUI.rightArm.onClick.AddListener (() => BaseUpgradeFunction (UpgradeSection.Right_Arm));
//		upgradesUI.tracks.onClick.AddListener (() => BaseUpgradeFunction (UpgradeSection.Tracks));
//	}
//
//	public void BaseUpgradeFunction(UpgradeSection section) {
//		upgradesUI.ToggleCraftPanel (true);
//		Upgrade upgradeToDisplay = upgradeDatabase.FetchUpgradeBySection (section);
//		Debug.Assert (upgradeToDisplay.ID != -1, "Can't find upgrade from section: " + section);
//
//		upgradesUI.SetTextsAndIcons (upgradeToDisplay);
////		upgradeManager.AddFunctionToUIButton (upgradesUI.CraftButton, section);
//		upgradesUI.CraftButton.onClick.RemoveAllListeners();
//		upgradesUI.CraftButton.onClick.AddListener (() => upgradeManager.AddFunctionToUIButton(upgradesUI.CraftButton, section));
//		upgradesUI.CraftButton.onClick.AddListener (() => upgradesUI.DisableUpgrade (section));
//
//		ClearDisplay (upgradesUI.ItemsPanel);
//		Item item = new Item ();
//		foreach (int key in upgradeToDisplay.Items.Keys) {
//			string itemAmount = ItemAmount(key, upgradeToDisplay.Items[key]);
//			item = itemDatabase.FetchItemByID (key);
//
//			Button itemButton = InstantiateSlot (taskItemUIGroup.Slot, upgradesUI.ItemsPanel.transform, item.Sprite, itemAmount); 
//			itemButton.enabled = false;
//		}
//
//		ClearDisplay (upgradesUI.ResourcesPanel);
//		Resource resource = new Resource ();
//		foreach (int key in upgradeToDisplay.RequiredResources.Keys) {
//			string resourceAmount = ResourceAmount(key, upgradeToDisplay.RequiredResources[key]);
//			resource = resourceDatabase.FetchResourceByID (key);
//
//			Button resourceButton = InstantiateSlot (taskItemUIGroup.Slot, upgradesUI.ResourcesPanel.transform, item.Sprite, resourceAmount); 
//			resourceButton.enabled = false;
//		}
//
//		upgradesUI.CheckUpgradeRequirements (upgradeToDisplay);
//	}
//
//	public void ToggleUpgradesDisplay(bool toggling = true) {
//		if (toggling) {
//			upgradesDisplaying = !upgradesDisplaying;
//			if (upgradesDisplaying) {
//				TurnOffPanels (Panels.Upgrades_);
//			}
//		} else {
//			upgradesDisplaying = false;
//		}
//		upgradesUI.ToggleCraftPanel (false);
//		upgradesUI.groupObject.SetActive (upgradesDisplaying);
//	}
//
//	// --------- INVENTORY UI ---------
//	/// <summary>
//	/// Toggles the inventory display.
//	/// </summary>
//	/// <param name="toggling">If set to <c>true</c> Inventory will toggle as normal, else inventory will be turned off.</param>
//	public void ToggleInventoryDisplay(bool toggling = true) {
//		if (toggling) {
//			inventoryDisplaying = !inventoryDisplaying;
//		} else {
//			inventoryDisplaying = false;
//		}
//		inventoryGroup.SetActive (inventoryDisplaying);
//		if (inventoryDisplaying) {
//			TurnOffPanels (Panels.Inventory_);
//			DisplayInventory ();
//		}
//	}
//
//	private void DisplayInventory() {
//		ClearDisplay (inventoryPanel);
//		List<string> inventoryItems = inventoryDatabase.GenerateInventoryStrings ();
//		List<Sprite> inventorySprites = inventoryDatabase.GenerateInventorySprites ();
//		Image slotImage;
//		Text descriptionText;
//
//		for (int i = 0; i < inventoryItems.Count; i++) {
//			GameObject newSlot = Instantiate (inventorySlot) as GameObject;
//			newSlot.transform.SetParent(inventoryPanel.transform, false);
//
//			slotImage = newSlot.GetComponent<Image> ();
//			descriptionText = newSlot.GetComponentInChildren<Text> ();
//
//			slotImage.sprite = inventorySprites [i];
//			descriptionText.text = inventoryItems [i];
//		}
//
//	}
//
//	// ---------------------------------------------------------------------------------------------------
//
//	// --------- TASKS UI ---------
//	public void ToggleTasksDisplay(bool toggling = true) {
//		if (toggling) {
//			taskDisplaying = !taskDisplaying;
//		} else {
//			taskDisplaying = false;
//		}
//		taskGroup.SetActive (taskDisplaying);
//
//		if (taskDisplaying) {
//			// Turn off other panels to only show Tasks panel
//			TurnOffPanels (Panels.Tasks_);
//
//			// Turn off sub panels to only show Tasks panel
//			taskUIGroup.ToggleGroup (false);
//			taskItemUIGroup.ToggleGroup (false);
//			itemUIGroup.ToggleGroup (false);
//
//			// Turn of backtrack panel
//			menuBackTrackPanel.SetActive (false);
//			DisplayTasks ();
//		}
//	}
//
//	private void DisplayTasks() {
//		ClearDisplay (taskPanel);
//		List<string> taskDescriptions = taskDatabase.GenerateTaskStrings ();
//
//		for (int i = 0; i < taskDescriptions.Count; i++) {
//			GameObject newSlot = Instantiate (taskSlot) as GameObject;
//			newSlot.transform.SetParent(taskPanel.transform, false);
//
//			newSlot.GetComponentInChildren<Text> ().text = taskDescriptions [i];
//			int index = i;
//			newSlot.GetComponent<Button> ().onClick.AddListener (() => DisplayTaskBreakdown(index));
//		}
//
//	}
//
//	public void DisplayTaskBreakdown(int taskID) {
//		taskUIGroup.ToggleGroup (true);
//
//		taskUIGroup.SetBreakdownText (taskID);
//
//		ClearDisplay (taskUIGroup.ItemsPanel);
//		Task task = taskDatabase.FetchTaskByID (taskID);
//
//		Dictionary<TaskItem, int> itemList = task.TaskItems;
//
//		// Create TaskItems list
//		foreach (TaskItem key in itemList.Keys) {
//			Button slotButton = InstantiateSlot (taskUIGroup.Slot, taskUIGroup.ItemsPanel.transform, key.Sprite, itemList [key].ToString ());
//
//			slotButton.onClick.AddListener (() => DisplayTaskItemBreakdown(key.ID));
//			slotButton.onClick.AddListener (() => taskItemUIGroup.CheckRequirements (key));
//		}
//	}
//
//
//	// ------ TASKITEMS UI ------
//
//	// Populates the TaskItem breakdown panel
//	public void DisplayTaskItemBreakdown(int taskItemID) {
//		taskItemUIGroup.ToggleGroup (true);
//		ClearDisplay (taskItemUIGroup.ItemsPanel);
//
//		TaskItem taskItem = itemDatabase.FetchTaskItemByID (taskItemID);
//		Debug.Assert (taskItem.ID != -1, "Item id: " + taskItemID + " is not a TaskItem.");
//
//		taskItemUIGroup.SetBreakdownText (taskItemID);
//		CreateItemList (taskItem);
//
//		taskItemUIGroup.CraftButton.onClick.AddListener (() => taskItemUIGroup.CraftTaskItem(taskItem));
//		
//	}
//		
//	// Populates the ItemsPanel with the required items from the given TaskItem
//	private void CreateItemList(TaskItem taskItem) {
//		Dictionary<CraftingItem, int> itemsRequired = taskItem.CraftingItems;
//		KeyValuePair<Item, int> retrievedItem;
//
//		// Loop through each key
//		foreach (CraftingItem key in itemsRequired.Keys) {
////			Debug.Log (key.Title);
//			string itemAmount = ItemAmount(key.ID, itemsRequired [key]);
//
//			Button instantiatedButton = InstantiateSlot (taskItemUIGroup.Slot, taskItemUIGroup.ItemsPanel.transform, key.Sprite, itemAmount);
//
//			instantiatedButton.enabled = false;
//		}
//	}
//
//
//
//}
//
//[System.Serializable]
//public class ItemUIGroup : System.Object {
//	public GameObject groupObject;
//
//	protected GameUIManager UIManager;
//	protected Image icon;
//	protected List<Text> textList = new List<Text> ();
//	protected Button craftButton;
//	protected ItemDatabase itemDatabase;
//	protected ResourceDatabase resourceDatabase;
//	protected InventoryDatabase inventory;
//
//	public bool isDisplaying = true;
//
//	public Image Icon {
//		get { return icon; }
//	}
//
//	public List<Text> TextList {
//		get { return textList; }
//	}
//
//	public Button CraftButton {
//		get { return craftButton; }
//	}
//
//	public virtual void GetChildren() {
//		UIManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameUIManager> ();
//		GameObject databases = GameObject.FindGameObjectWithTag ("Databases");
//		itemDatabase = databases.GetComponent<ItemDatabase> ();
//		resourceDatabase = databases.GetComponent<ResourceDatabase> ();
//		inventory = databases.GetComponent<InventoryDatabase> ();
//
//		Transform groupTransform = groupObject.transform;
//
//		// Get Image for Icon
//		icon = groupTransform.GetChild (0).GetComponent<Image> ();
//
//		// Get Texts
//		GameObject texts = groupTransform.GetChild (1).gameObject;
//		for (int i = 0; i < 3; i++) {
//			textList.Add(texts.transform.GetChild(i).GetComponent<Text>());
//		}
//
//		// Get Button
//		craftButton = groupTransform.GetChild (2).GetComponent<Button> ();
//	}
//
//	public void SetBreakdownText (int itemID) {
//		
//		CraftingItem craftingItem = itemDatabase.FetchCraftingItemByID (itemID);
//		Debug.Assert (craftingItem.ID != -1, "Invalid id: " + itemID);
//
//		bool hasResources = false;
//
//		icon.sprite = craftingItem.Sprite;
//		textList [0].text = craftingItem.Title;
//		textList [1].text = craftingItem.Description;
//		this.RefreshResources (craftingItem);
//
//		hasResources = CheckResourceRequirements (craftingItem);
//		craftButton.enabled = hasResources;
//		if (hasResources) {
//			craftButton.onClick.AddListener (() => CraftItem (craftingItem));
//		}
//	}
//
//	protected virtual string GenerateResourceList(Dictionary<ResourceType, int> requiredResources) {
//		string resourceList = "";
//
//		// For the Resource List
//		// Loop through all the keys in the Dictionary
//		foreach (ResourceType key in requiredResources.Keys) {
//
//			// check if resource type is not unknown
//			if (key != ResourceType.Unknown) {
//				// Get the resource
//				Resource resource = resourceDatabase.FetchResourceByID ((int)key);
//				Debug.Assert (resource.ID != -1, "Unkown resource ID: " + (int)key);
//
//				// Displays: Title, amount the player has, amount required
//				string resourceDetail = resource.Title + ": " + resource.Quantity + " / " + requiredResources [key];
//
//				// Check if the player's resource is greater than the required amount of resource
//				// Sets the respective resource to the color green or red based on condition
//				if (resource.Quantity >= requiredResources [key]) {
//					resourceList += SetStringColor (resourceDetail, "green") + "\n";
//				} else {
//					resourceList += SetStringColor (resourceDetail, "red") + "\n";
//				}
//			}
//		}
//
//		return resourceList;
//	}
//
//	public void CraftItem(CraftingItem item) {
//		inventory.AddItemByID (item.ID);
//		foreach (ResourceType key in item.RequiredResources.Keys) {
//			resourceDatabase.RemoveResourceByID (key, item.RequiredResources [key]);
//
//		}
//		Debug.Log ("Item Added");
//
//		this.RefreshResources (item);
//	}
//
//	private bool CheckResourceRequirements(CraftingItem craftingItem) {
//		bool hasResources = true;
//		Resource resource = new Resource ();
//		foreach (ResourceType key in craftingItem.RequiredResources.Keys) {
//			resource = resourceDatabase.FetchResourceByID ((int)key);
//			if (resource.Quantity < craftingItem.RequiredResources [key]) {
//				hasResources = false;
//			}
//		}
//
//		return hasResources;
//	}
//
//
//	public virtual void ToggleGroup(bool state) {
//		// if object was turned off some other way
//		if (isDisplaying && state) {
//			this.craftButton.onClick.RemoveAllListeners ();
//		}
//		isDisplaying = state;
//		groupObject.SetActive (state);
//		if (!state) {
//			this.craftButton.onClick.RemoveAllListeners ();
//			UIManager.TurnOffTopPanel ();
//		} else {
//			UIManager.AddPanelToHierachy (groupObject);
//		}
//	}
//
//	// Uses string markup to change color of text
//	protected string SetStringColor(string text, string color) {
//		text = "<color=" + color + ">" + text + "</color>";
//		return text;
//	}
//
//	/// <summary>
//	/// Refreshes the resource text.
//	/// </summary>
//	/// <param name="craftingItem">The crafting item in which to reference.</param>
//	/// <param name="index">Index of the resource Text object in the TextsList</param>
//	protected void RefreshResources(CraftingItem craftingItem, int index = 2) {
//		this.textList [index].text = GenerateResourceList(craftingItem.RequiredResources);
//	}
//}
//
//[System.Serializable]
//public class TaskItemUIGroup : ItemUIGroup {
//	protected GameObject itemsPanel;
//	public GameObject slot;
//
//	protected TaskManager taskManager;
//
//	public GameObject ItemsPanel { get { return itemsPanel; } }
//
//	public GameObject Slot { get { return slot; } }
//
//	public override void GetChildren() {
//		taskManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TaskManager> ();
//		base.GetChildren ();
//
//		itemsPanel = groupObject.transform.GetChild (3).gameObject;
//	}
//
//	public new void SetBreakdownText (int taskItemID) {
//		TaskItem taskItem = itemDatabase.FetchTaskItemByID (taskItemID);
//		Debug.Assert (taskItem.ID != -1, "Item id: " + taskItemID + " is not a TaskItem.");
//
////		string resourceList = GenerateResourceList(taskItem.RequiredResources);
//
//		// Displays the texts in their respective Text; is in a specific order based on the hierarchy 
//		textList [0].text = taskItem.Title; // Name
//		textList [1].text = taskItem.Description; // Description
////		textList [2].text = resourceList; // Resources required
//		this.RefreshResources(taskItem);
//
////		inventory.AddItemByID (10, 21);
////		Debug.Log ("added Item");
//	}
//
//	public void CheckRequirements(TaskItem taskItem) {
//		Dictionary<CraftingItem, int> missingItems = CheckItemRequirements (taskItem);
//		Dictionary<ResourceType, int> missingResources = CheckResourceRequirements (taskItem);
//
//		if (missingItems.Count == 0 && missingResources.Count == 0) {
//			CraftButton.enabled = true;
//			CraftButton.image.color = Color.green;
//		} else {
//			CraftButton.enabled = false;
//			CraftButton.image.color = Color.red;
//		}
//	}
//
//	private Dictionary<CraftingItem, int> CheckItemRequirements(TaskItem taskItem) {
//		Dictionary<CraftingItem, int> missingItems = new Dictionary<CraftingItem, int> ();
//		KeyValuePair<Item, int> itemInInventory = new KeyValuePair<Item, int> ();
//
//		foreach (CraftingItem key in taskItem.CraftingItems.Keys) {
//			itemInInventory = inventory.FetchItemWithQuantityByID (key.ID);
//
//			if (itemInInventory.Key.ID != -1) {
//				if (itemInInventory.Value < taskItem.CraftingItems [key]) {
//					missingItems.Add (key, taskItem.CraftingItems [key] - itemInInventory.Value);
//				}
//			} else {
//				missingItems.Add (key, taskItem.CraftingItems [key]);
//			}
//		}
//
//		return missingItems;
//	}
//
//	private Dictionary<ResourceType, int> CheckResourceRequirements(TaskItem taskItem) {
//		Dictionary<ResourceType, int> missingResources = new Dictionary<ResourceType, int> ();
//		Resource resourceToCheck = new Resource ();
//
//		foreach (ResourceType key in taskItem.RequiredResources.Keys) {
//			resourceToCheck = resourceDatabase.FetchResourceByID ((int)key);
//			Debug.Assert (resourceToCheck.ID != -1, "This should not happen. resourceID: " + key + " not found.");
//
//			if (resourceToCheck.Quantity < taskItem.RequiredResources [key]) {
//				missingResources.Add (key, taskItem.RequiredResources [key] - resourceToCheck.Quantity);
//			}
//		}
//
//		return missingResources;
//	}
//
//
//	public void CraftTaskItem(TaskItem taskItem) {
//		taskManager.AddActiveTaskItem (taskItem, 1);
//		taskManager.RemoveItems (taskItem.CraftingItems);
//		taskManager.RemoveResources (taskItem.RequiredResources);
//	}
//
//	/// <summary>
//	/// Refreshes the resource text.
//	/// </summary>
//	/// <param name="craftingItem">The crafting item in which to reference.</param>
//	/// <param name="index">Index of the resource Text object in the TextsList</param>
//	public void RefreshResources(TaskItem taskItem, int index = 2) {
//		this.textList [index].text = GenerateResourceList(taskItem.RequiredResources);
//	}
//}
//
//[System.Serializable]
//public class TaskUIGroup : TaskItemUIGroup {
//	protected Slider progressSlider;
//	protected TaskDatabase taskDatabase;
//
//	public Slider ProgressSlider {
//		get { return progressSlider; }
//	}
//
//	public override void GetChildren() {
//		taskDatabase = GameObject.FindGameObjectWithTag ("Databases").GetComponent<TaskDatabase> ();
//		
//		base.GetChildren ();
//
//		itemsPanel = groupObject.transform.GetChild (2).gameObject;
//		progressSlider = groupObject.transform.GetChild (3).GetComponent<Slider> ();
//	}
//
//	public new void SetBreakdownText (int taskID) {
//		Task task = taskDatabase.FetchTaskByID (taskID);
//		Debug.Assert (task.ID != -1, "Unknown taskID: " + taskID);
//
//		textList [0].text = task.Title;
//		textList [1].text = task.Description;
//		textList [2].text = Mathf.FloorToInt(task.Progress).ToString() + "%";
//
//		// Display progress bar
//		Vector2 progress = taskDatabase.GetTaskProgressByID (taskID);
//		progressSlider.value = progress.x / progress.y;
//	}
//
//	public override void ToggleGroup (bool state)
//	{
//		isDisplaying = state;
//		groupObject.SetActive (state);
//		if (state) {
//			UIManager.AddPanelToHierachy (groupObject);
//		} else {
//			UIManager.TurnOffTopPanel ();
//		}
//	}
//}
//
//[System.Serializable]
//public class TaskItemCraft : System.Object {
//
//	protected InventoryDatabase inventory;
//	protected ItemDatabase itemDatabase;
//	protected ResourceDatabase resourceDatabase;
//
//	public GameObject craftPanel;
//	private List<Text> textsList = new List<Text>();
//	private GameObject itemsPanel;
//	private GameObject resourcesPanel;
//	private Image icon;
//	private Image result;
//	private Button craftButton;
//
//	//	private bool isDisplaying = false;
//
//	public Button CraftButton { get { return craftButton; } }
//	public GameObject ItemsPanel{ get { return itemsPanel; } }
//	public GameObject ResourcesPanel{ get { return resourcesPanel; } }
//	//	public bool Displaying{ get { return isDisplaying; } }
//
//	public void GetCraftPanelChildren() {
//		GameObject databases = GameObject.FindGameObjectWithTag ("Databases");
//		inventory = databases.GetComponent<InventoryDatabase> ();
//		itemDatabase = databases.GetComponent<ItemDatabase> ();
//		resourceDatabase = databases.GetComponent<ResourceDatabase> ();
//
//		Transform panelTransform = craftPanel.transform;
//
//		icon = panelTransform.GetChild (0).GetComponent<Image>();
//		result = panelTransform.GetChild (1).GetComponent<Image>();
//
//		Transform textListObject = panelTransform.GetChild (2);
//		textsList.Add (textListObject.GetChild (0).GetComponent<Text> ());
//		textsList.Add (textListObject.GetChild (1).GetComponent<Text> ());
//
//		itemsPanel = panelTransform.GetChild (3).GetChild(0).gameObject;
//		resourcesPanel = panelTransform.GetChild (4).GetChild(0).gameObject;
//
//		craftButton = panelTransform.GetChild (5).GetComponent<Button> ();
//	}
//
//	public void SetTextsAndIcons(TaskItem taskItem) {
//		textsList [0].text = taskItem.Title;
//		textsList [1].text = taskItem.Description;
//
//		icon.sprite = taskItem.Sprite;
//		result.sprite = taskItem.Sprite;
//	}
//
//	public void CheckUpgradeRequirements(TaskItem taskItem) {
//		Dictionary<CraftingItem, int> missingItems = CheckMissingItems(taskItem);
//		Dictionary<ResourceType, int> missingResources = CheckMissingResources (taskItem);
//
//		// Check if items are missing
//		if (missingItems.Count == 0 && missingResources.Count == 0) {
//			CraftButton.enabled = true;
//			CraftButton.image.color = Color.green;
//		} else {
//			CraftButton.enabled = false;
//			CraftButton.image.color = Color.red;
//		}
//
//	}
//
//	// Checks if items are missing from inventory
//	private Dictionary<CraftingItem, int> CheckMissingItems(TaskItem taskItem) {
//		Dictionary<CraftingItem, int> missingItems = new Dictionary<CraftingItem, int> ();
//		KeyValuePair<Item, int> itemToCheck = new KeyValuePair<Item, int> ();
//
//		// Loop through each item required for crafting
//		foreach (CraftingItem key in taskItem.CraftingItems.Keys) {
//			itemToCheck = inventory.FetchItemWithQuantityByID (key.ID); // Get item
//
//			// Check if player has item in their inventory
//			if (itemToCheck.Key.ID != -1) { // item exists in inventory
//				// Check if player has enough items to craft
//				if (itemToCheck.Value < taskItem.CraftingItems [key]) {
//					missingItems.Add (key, taskItem.CraftingItems [key] - itemToCheck.Value);
//				}
//
//			} else {
//				missingItems.Add (key, taskItem.CraftingItems [key]);
//			}
//		}
//
//		return missingItems;
//	}
//
//	// Checks if player has enough resources
//	private Dictionary<ResourceType, int> CheckMissingResources(TaskItem taskItem) {
//		Dictionary<ResourceType, int> missingResources = new Dictionary<ResourceType, int> ();
//		Resource resourceToCheck = new Resource ();
//
//		foreach (ResourceType key in taskItem.RequiredResources.Keys) {
//			resourceToCheck = resourceDatabase.FetchResourceByID ((int)key);
//			Debug.Assert (resourceToCheck.ID != -1, "This should not happen. resourceID: " + key + " not found.");
//
//			if (resourceToCheck.Quantity < taskItem.RequiredResources [key]) {
//				missingResources.Add ((ResourceType)key, taskItem.RequiredResources [key] - resourceToCheck.Quantity);
//			}
//
//		}
//
//		return missingResources;
//	}
//
//	public void ToggleCraftPanel(bool state) {
//		craftPanel.SetActive (state);
//	}
//}
//
//
//[System.Serializable]
//public class UpgradesUI : System.Object {
//
//	protected InventoryDatabase inventory;
//	protected ItemDatabase itemDatabase;
//	protected ResourceDatabase resourceDatabase;
//
//	public GameObject groupObject;
//	public List<Button> upgradeButtonSections = new List<Button> ();
//	public Button head;
//	public Button torso;
//	public Button leftArm;
//	public Button rightArm;
//	public Button tracks;
//
//	public GameObject craftPanel;
//	private List<Text> textsList = new List<Text>();
//	private GameObject itemsPanel;
//	private GameObject resourcesPanel;
//	private Image icon;
//	private Image result;
//	private Button craftButton;
//
////	private bool isDisplaying = false;
//
//	public Button CraftButton { get { return craftButton; } }
//	public GameObject ItemsPanel{ get { return itemsPanel; } }
//	public GameObject ResourcesPanel{ get { return resourcesPanel; } }
////	public bool Displaying{ get { return isDisplaying; } }
//
//	public void GetUpgradeChildren() {
//		upgradeButtonSections.Add (head);
//		upgradeButtonSections.Add (torso);
//		upgradeButtonSections.Add (leftArm);
//		upgradeButtonSections.Add (rightArm);
//		upgradeButtonSections.Add (tracks);
//	}
//
//	public void GetCraftPanelChildren() {
//		GameObject databases = GameObject.FindGameObjectWithTag ("Databases");
//		inventory = databases.GetComponent<InventoryDatabase> ();
//		itemDatabase = databases.GetComponent<ItemDatabase> ();
//		resourceDatabase = databases.GetComponent<ResourceDatabase> ();
//
//		Transform panelTransform = craftPanel.transform;
//
//		icon = panelTransform.GetChild (0).GetComponent<Image>();
//		result = panelTransform.GetChild (1).GetComponent<Image>();
//
//		Transform textListObject = panelTransform.GetChild (2);
//		textsList.Add (textListObject.GetChild (0).GetComponent<Text> ());
//		textsList.Add (textListObject.GetChild (1).GetComponent<Text> ());
//
//		itemsPanel = panelTransform.GetChild (3).GetChild(0).gameObject;
//		resourcesPanel = panelTransform.GetChild (4).GetChild(0).gameObject;
//
//		craftButton = panelTransform.GetChild (5).GetComponent<Button> ();
//	}
//
//	public void SetTextsAndIcons(Upgrade upgrade) {
//		textsList [0].text = upgrade.Title;
//		textsList [1].text = upgrade.Description;
//
//		icon.sprite = upgrade.Icon;
//		result.sprite = upgrade.Icon;
//	}
//
//	public void CheckUpgradeRequirements(Upgrade upgrade) {
//		Dictionary<Item, int> missingItems = CheckMissingItems(upgrade);
//		Dictionary<ResourceType, int> missingResources = CheckMissingResources (upgrade);
//
//		// Check if items are missing
//		if (missingItems.Count == 0 && missingResources.Count == 0) {
//			CraftButton.enabled = true;
//			CraftButton.image.color = Color.green;
//		} else {
//			CraftButton.enabled = false;
//			CraftButton.image.color = Color.red;
//		}
//
//	}
//
//	// Checks if items are missing from inventory
//	private Dictionary<Item, int> CheckMissingItems(Upgrade upgrade) {
//		Dictionary<Item, int> missingItems = new Dictionary<Item, int> ();
//		KeyValuePair<Item, int> itemToCheck = new KeyValuePair<Item, int> ();
//
//		// Loop through each item required for crafting
//		foreach (int key in upgrade.Items.Keys) {
//			itemToCheck = inventory.FetchItemWithQuantityByID (key); // Get item
//
//			// Check if player has item in their inventory
//			if (itemToCheck.Key.ID != -1) { // item exists in inventory
//				// Check if player has enough items to craft
//				if (itemToCheck.Value < upgrade.Items [key]) {
//					missingItems.Add (itemToCheck.Key, upgrade.Items [key] - itemToCheck.Value);
//				}
//
//			} else {
//				// get reference to crafting item from database
//				CraftingItem craftingItem = itemDatabase.FetchCraftingItemByID (key);
//				Debug.Assert (craftingItem.ID != -1, "Should not happen. ID: " + key + " not found");
//
//				missingItems.Add (craftingItem, upgrade.Items [key]);
//			}
//		}
//
//		return missingItems;
//	}
//
//	// Checks if player has enough resources
//	private Dictionary<ResourceType, int> CheckMissingResources(Upgrade upgrade) {
//		Dictionary<ResourceType, int> missingResources = new Dictionary<ResourceType, int> ();
//		Resource resourceToCheck = new Resource ();
//
//		foreach (int key in upgrade.RequiredResources.Keys) {
//			resourceToCheck = resourceDatabase.FetchResourceByID (key);
//			Debug.Assert (resourceToCheck.ID != -1, "This should not happen. resourceID: " + key + " not found.");
//
//			if (resourceToCheck.Quantity < upgrade.RequiredResources [key]) {
//				missingResources.Add ((ResourceType)key, upgrade.RequiredResources [key] - resourceToCheck.Quantity);
//			}
//		}
//
//		return missingResources;
//	}
//
//	public void DisableUpgrade(UpgradeSection section) {
//		Button upgradeButton = upgradeButtonSections [(int)section];
//		upgradeButton.enabled = false;
//		upgradeButton.image.color = Color.green;
//
//		ToggleCraftPanel (false);
//	}
//
//	public void ToggleCraftPanel(bool state) {
//		craftPanel.SetActive (state);
//	}
//}