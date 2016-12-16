using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {

	public int ID { get; set; }
	public string Title { get; set; }
	public string Slug { get; set; }
	public string Description { get; set; }
	public float Progress { get; set; }
	public Dictionary<TaskItem, int> TaskItems { get; set; } // TaskItem, number of this item Active (i.e. that are working)
//	public Dictionary<TaskItem, int> MyTaskItems { get; set; }
	public bool Completed { get; set; }

	public Task(int id, string title, string slug, string description, Dictionary<TaskItem, int> taskItems) {
		this.ID = id;
		this.Title = title;
		this.Slug = slug;
		this.Description = description;
		this.Progress = 0;
		this.TaskItems = taskItems;
//		this.MyTaskItems = myItems;
		this.Completed = false;
	}

	public Task() {
		this.ID = -1;
		this.Progress = 0;
		this.Completed = false;
	}


	public void CheckCompletion() {
		// if progress is finished then set Completed variable

	}
}
