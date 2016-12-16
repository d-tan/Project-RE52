using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RubbishType {
	General,
	Organic,
	Recycle
}

public class RubbishItem : MonoBehaviour {

	public List<RubbishType> rubbishTypes = new List<RubbishType> ();
	private List<ResourceType> resourceTypes = new List<ResourceType> ();

	//	private Collider2D itemCollider;
	private Rigidbody2D rb;
	bool endOfConveyorbelt = false;
	bool isHeld = false;
	private int itemID;

	void Start() {
//		itemCollider = GetComponent<Collider2D> ();
		rb = GetComponent<Rigidbody2D> ();

		// Defaults of a rubbish item
		rb.mass = 10.0f;
		this.tag = "PickUpable";
		this.gameObject.layer = 8; // "Item" layer
	}

	public List<RubbishType> RubbishTypes {
		get { 
			return rubbishTypes;
		}
		set {
			rubbishTypes = value;
		}
	}

	public int RubbishItemID {
		get {
			return itemID;
		}
		set {
			itemID = value;
		}
	}

	public List<ResourceType> ResourceTypes {
		get {
			return resourceTypes;
		}
		set {
			resourceTypes = value;
		}
	}

	// Determines if this item is at the end of the converyor belt;
	public bool AtEnd {
		get { return endOfConveyorbelt; }
		set { endOfConveyorbelt = value; }
	}

	// Determines if this item is being held
	public bool IsBeingHeld {
		get { return isHeld; }
		set { isHeld = value; }
	}
}
