using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item {

	public int ID { get; set; }
	public string Title { get; set; }
	public string Slug { get; set; }
	public string Description { get; set; }
	public Dictionary <ResourceType, int> ResourcesGiven { get; set; }
	public bool IsCraftingItem { get; set; }
	public ItemRarity Rarity { get; set; }


	public Sprite Sprite { get; set; }

	public Item(int id, string title, string slug, string desc, Dictionary<ResourceType, int> resources, bool crafting, ItemRarity rare) {
		this.ID = id;
		this.Title = title;
		this.Slug = slug;
		this.Description = desc;
		this.ResourcesGiven = resources;
		this.IsCraftingItem = crafting;
		this.Rarity = rare;

		this.Sprite = Resources.Load<Sprite> ("Sprites/Items/" + slug);
	}

	public Item() {
		this.ID = -1;
	}
}

public class CraftingItem : Item {

	public bool Craftable { get; set; }
	public Dictionary<ResourceType, int> RequiredResources { get; set; }
	public bool IsTaskItem { get; set; }

	public CraftingItem(int id, string title, string slug, string desc, Dictionary<ResourceType, int> resources, bool crafting, ItemRarity rare, bool craftable, Dictionary<ResourceType, int> requiredResources, bool taskItem) {
		this.ID = id;
		this.Title = title;
		this.Slug = slug;
		this.Description = desc;
		this.ResourcesGiven = resources;
		this.IsCraftingItem = crafting;
		this.Rarity = rare;
		this.Craftable = craftable;
		this.RequiredResources = requiredResources;
		this.IsTaskItem = taskItem;

		this.Sprite = Resources.Load<Sprite> ("Sprites/Items/" + slug);

	}

	// is not craftable
	public CraftingItem(int id, string title, string slug, string desc, Dictionary<ResourceType, int> resources, bool crafting, ItemRarity rare, bool craftable) {
		this.ID = id;
		this.Title = title;
		this.Slug = slug;
		this.Description = desc;
		this.ResourcesGiven = resources;
		this.IsCraftingItem = crafting;
		this.Rarity = rare;
		this.Craftable = craftable;
		this.IsTaskItem = false;

		this.Sprite = Resources.Load<Sprite> ("Sprites/Items/" + slug);

	}

	public CraftingItem() {
		this.ID = -1;
		this.IsCraftingItem = true;
		this.Craftable = false;
	}
}

public class TaskItem : CraftingItem {

	public Dictionary<CraftingItem, int> CraftingItems { get; set; } 

	public TaskItem(int id, string title, string slug, string desc, Dictionary<ResourceType, int> resources, bool crafting, ItemRarity rare, bool craftable, Dictionary<ResourceType, int> requiredResources, bool taskItem, Dictionary<CraftingItem, int> craftingItems) {
		this.ID = id;
		this.Title = title;
		this.Slug = slug;
		this.Description = desc;
		this.ResourcesGiven = resources;
		this.IsCraftingItem = crafting;
		this.Rarity = rare;
		this.Craftable = craftable;
		this.RequiredResources = requiredResources;
		this.IsTaskItem = taskItem;
		this.CraftingItems = craftingItems;

		this.Sprite = Resources.Load<Sprite> ("Sprites/Items/" + slug);

	}

	public TaskItem () {
		this.ID = -1;
		this.IsCraftingItem = true;
		this.Craftable = true;
		this.IsTaskItem = true;
	}
}
