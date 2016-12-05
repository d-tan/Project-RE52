﻿using UnityEngine;
using System.Collections;

public class Item {

	public int ID { get; set; }
	public string Title { get; set; }
	public string Slug { get; set; }
	public string Description { get; set; }
	public ResourceType Resource1 { get; set; }
	public ResourceType Resource2 { get; set; }
	public int Quantity1 { get; set; }
	public int Quantity2 { get; set; }
	public bool CraftingItem { get; set; }
	public ItemRarity Rarity { get; set; }


	public Sprite Sprite { get; set; }

	public Item(int id, string title, string slug, string desc, ResourceType resource1, ResourceType resource2, int quantity1, int quantity2, bool crafting, ItemRarity rare) {
		this.ID = id;
		this.Title = title;
		this.Slug = slug;
		this.Description = desc;
		this.Resource1 = resource1;
		this.Resource2 = resource2;
		this.Quantity1 = quantity1;
		this.Quantity2 = quantity2;
		this.CraftingItem = crafting;
		this.Rarity = rare;

		this.Sprite = Resources.Load<Sprite> ("Sprites/Items/" + slug);
	}

	public Item() {
		this.ID = -1;
	}
}