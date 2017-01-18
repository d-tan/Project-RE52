using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMenuBacktrack : MonoBehaviour, IPointerDownHandler {

	GameUIManager UIManager;

	void Start() {
		UIManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameUIManager> ();
	}

	public void OnPointerDown(PointerEventData eventData) {
//		Debug.Log ("Pointer Down");
		UIManager.TurnOffTopPanel ();
	}
}
