﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {

	public Image[] indicators = new Image[2];
	private Color[] indicatorColors = new Color[4];

	public Text resourceDisplay;

	public GameObject upgradesPanel;
	List<Text> upgradesTexts = new List<Text>();
	List<Button> upgradeButtons = new List<Button> ();


	private bool upgradesDisplaying = true;

	void Start() {
		indicatorColors [0] = new Color (1.0f, 103.0f / 255.0f, 103.0f / 255.0f, 160.0f / 255.0f);
		indicatorColors [1] = new Color (54.0f / 255.0f, 188.0f / 255.0f, 71.0f / 255.0f, 160.0f / 255.0f);
		indicatorColors [2] = new Color (241.0f / 255.0f, 244.0f / 255.0f, 48.0f / 255.0f, 160.0f / 255.0f);
		indicatorColors [3] = new Color (184.0f / 255.0f, 184.0f / 255.0f, 184.0f / 255.0f, 160.0f / 255.0f);

		resourceDisplay.gameObject.SetActive (false);
		RetrieveUpgradesUI ();
//		ToggleUpgradesDisplay ();
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
}
