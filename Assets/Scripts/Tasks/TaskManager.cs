using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour {

	Dictionary<TaskItem, int> activeTaskItems = new Dictionary<TaskItem, int> ();
	Dictionary<TaskItem, List<Task>> taskItemTasks = new Dictionary<TaskItem, List<Task>>();

	TaskDatabase taskDatabase;
	ItemDatabase itemDatabase;
	GameUIManager UIManager;

	private float timer = 0.0f;
	private const float tickRate = 1.0f; // 1 second
	private const float taskRate = 0.1f; // rate at which Tasks progress

	void Start() {
		GameObject databases = GameObject.FindGameObjectWithTag ("Databases");
		taskDatabase = databases.GetComponent<TaskDatabase> ();
		itemDatabase = databases.GetComponent<ItemDatabase> ();
		UIManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameUIManager> ();

		ConstructActiveTaskItemsList ();
		ConstructTaskItemTasks ();
	}

	private void ConstructActiveTaskItemsList() {
		activeTaskItems = itemDatabase.ConstructTaskItemDictionary ();
	}

	private void ConstructTaskItemTasks() {
		taskItemTasks = taskDatabase.ConstructTaskItemTasks (activeTaskItems);
	}

	public void AddActiveTaskItem(TaskItem taskItem, int quantity, int sign = 1) {
		activeTaskItems [taskItem] += sign * quantity;
		Debug.Log (taskItem.Title + " quantity: " + activeTaskItems [taskItem]);
	}

	void Update() {
		timer += Time.deltaTime;

		if (timer >= tickRate) {
			timer -= tickRate;
			UpdateTasks ();
		}
	}

	private void UpdateTasks() {
		foreach (TaskItem key in activeTaskItems.Keys) {
			if (activeTaskItems [key] > 0) {
				foreach (Task task in taskItemTasks[key]) {
					task.Progress += taskRate * activeTaskItems [key];
				}
			}
		}
	}
}
