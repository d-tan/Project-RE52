using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour {

	TaskDatabase taskDatabase;
	GameUIManager UIManager;

	void Start() {
		taskDatabase = GameObject.FindGameObjectWithTag ("Databases").GetComponent<TaskDatabase> ();
		UIManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameUIManager> ();
	}


}
