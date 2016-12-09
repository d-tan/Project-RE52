using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade {
	public int ID { get; set; }
	public string Title { get; set; }
	public string Slug { get; set; }
	public string Description { get; set; }
	public UpgradeSection Section { get; set; }
	public Dictionary<int, int> Items { get; set; } // <ID, quantity>
	public Dictionary<int, int> Resources { get; set; } // <ID, quantity>

	public Upgrade (int id, string title, string slug, string description, UpgradeSection section, Dictionary<int, int> items, Dictionary<int, int> resources) {
		this.ID = id;
		this.Title = title;
		this.Slug = slug;
		this.Description = description;
		this.Section = section;
		this.Items = items;
		this.Resources = resources;
	}
}
