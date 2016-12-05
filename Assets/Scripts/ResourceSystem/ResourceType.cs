using UnityEngine;
using System.Collections;

public enum ResourceType {
	// enums will need to be added manually in ALPHABETICAL order if more Resourcec types are added
	// from the resource list file that is read.
	Building_Materials,
	Electronics,
	Fertaliser,
	General_Rubbsih,
	Metal,
	Paper,
	Plants,
	Plastics,
	Textiles
}

public class Resource {

	public int ID { get; set; }
	public string Title { get; set; }
	public string Slug { get; set; }
	public string Description { get; set; }
	public RubbishType RubbishType { get; set; }
	public int Quantity { get; set; }

	public Resource(int id, string title, string slug, string desc, RubbishType type) {
		this.ID = id;
		this.Title = title;
		this.Slug = slug;
		this.Description = desc;
		this.RubbishType = type;
		this.Quantity = 0;

	}
}
