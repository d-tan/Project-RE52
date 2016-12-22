using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class TaskDatabase : MonoBehaviour {

	private List<Task> database = new List<Task>();
	private JsonData taskData;

	ItemDatabase itemDatabase;

	void Start() {
		itemDatabase = GetComponent<ItemDatabase> ();

		TextAsset file = Resources.Load("Json/Tasks") as TextAsset;
		taskData = JsonMapper.ToObject (file.text);

		ConstructTaskDatabase ();
	}

	private void ConstructTaskDatabase() {
		for (int i = 0; i < taskData.Count; i++) {
			database.Add (new Task (
				(int)taskData [i] ["id"], 
				taskData [i] ["title"].ToString (), 
				taskData [i] ["slug"].ToString (),
				taskData [i] ["description"].ToString (),
				ConstructTaskItemDictionary (taskData [i] ["taskItems"] ["IDs"])
			));
//			Debug.Log (database [i].Title + " desc: " + database [i].Description);
		}
	}

	private Dictionary<TaskItem, int> ConstructTaskItemDictionary(JsonData taskItemIDs) {
		Dictionary<TaskItem, int> taskItems = new Dictionary<TaskItem, int> ();

		for (int i = 0; i < taskItemIDs.Count; i++) {
			Item item = itemDatabase.FetchItemByID ((int)taskItemIDs [i.ToString ()]);
			Debug.Assert (item.ID != -1, "Item not found");
			Debug.Assert (item.IsCraftingItem, "Item is not a craftingItem, hence not a Task Item");
			CraftingItem craftingItem = (CraftingItem)item;
			Debug.Assert (craftingItem.IsTaskItem, "Item is not a Task Item");
			TaskItem taskItem = (TaskItem)craftingItem;

			taskItems.Add (taskItem, 0);
		}

		return taskItems;
	}

	public Task FetchTaskByID(int id) {
		for (int i = 0; i < database.Count; i++) {
			if (id == database [i].ID) {
				return database [i];
			}
		}

		return new Task ();
	}

	public List<string> GenerateTaskStrings() {
		List<string> descriptions = new List<string> ();
		for (int i = 0; i < database.Count; i++) {
			string text = "<b>" + database [i].Title + "</b>\t\t " + Mathf.FloorToInt (database [i].Progress) + "%\n";
			descriptions.Add (text);
		}

		return descriptions;
	}

	public List<string> GenerateBreakdownString(int id) {
		Task task = FetchTaskByID (id);
		List<string> texts = new List<string> ();

		Debug.Assert (task.ID != -1, "Invalid ID: " + id);

		texts.Add (task.Title);
		texts.Add (task.Description);
		texts.Add (task.Progress.ToString () + "%");

		return texts;
	}

	public Vector2 GetTaskProgressByID(int id) {
		Task task = FetchTaskByID (id);
		return new Vector2 (task.Progress, task.ProgressCompletion);
	}
}
