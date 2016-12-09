using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class ResourceDatabase : MonoBehaviour {

	private List<Resource> database = new List<Resource> ();
	private JsonData resourceData; 

	// Use this for initialization
	void Start () {
		TextAsset file = Resources.Load ("Json/ResourceList") as TextAsset;
		resourceData = JsonMapper.ToObject (file.text);

		ConstructResourceDatabase ();
	}

	private void ConstructResourceDatabase() {
		string debugString = "ID: (int) name in DB | enum name \n";
		for (int i = 0; i < resourceData.Count; i++) {
			database.Add( new Resource(
				(int)resourceData[i]["id"],
				resourceData[i]["title"].ToString(),
				resourceData[i]["slug"].ToString(),
				resourceData[i]["description"].ToString(),
				(RubbishType)(int)resourceData[i]["rubbishType"]
			));
			debugString += database [i].ID + " " + database [i].Title + " | " + (ResourceType)database [i].ID + "\n";
		}
		Debug.Log (debugString);
	}

	public Resource FetchResourceByID(int id) {
		for (int i = 0; i < database.Count; i++) {
			if (database [i].ID == id) {
				return database [i];
			}
		}
		return database [0];
	}

	public int DatabaseLength {
		get {
			return database.Count;
		}
	}
}
