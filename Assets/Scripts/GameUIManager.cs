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

	void Start() {
		indicatorColors [0] = new Color (1.0f, 103.0f / 255.0f, 103.0f / 255.0f, 160.0f / 255.0f);
		indicatorColors [1] = new Color (54.0f / 255.0f, 188.0f / 255.0f, 71.0f / 255.0f, 160.0f / 255.0f);
		indicatorColors [2] = new Color (241.0f / 255.0f, 244.0f / 255.0f, 48.0f / 255.0f, 160.0f / 255.0f);
		indicatorColors [3] = new Color (184.0f / 255.0f, 184.0f / 255.0f, 184.0f / 255.0f, 160.0f / 255.0f);

		// Resources
		resourceDisplay.gameObject.SetActive (false);

		// Upgrades
		RetrieveUpgradesUI ();
		ToggleUpgradesDisplay ();

		// Inventory
		ToggleInventoryDisplay();
		inventoryDatabase = GameObject.FindGameObjectWithTag("Databases").GetComponent<InventoryDatabase>();
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
		ClearInventoryDisplay ();
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

	private void ClearInventoryDisplay() {
		for (int i = 0; i < inventoryPanel.transform.childCount; i++) {
			Destroy (inventoryPanel.transform.GetChild (0).gameObject);
		}
	}
}
