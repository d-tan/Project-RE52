using UnityEngine;
using System.Collections;

public enum RubbishType {
	General,
	Organic,
	Recycle
}

public class RubbishItem : MonoBehaviour {

	//	private int maxNumofTypes = 2;
	public RubbishType[] myTypes = new RubbishType[2];

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

	public RubbishType[] ThisRubbishTypes {
		get { 
			return myTypes;
		}
		set {
			myTypes = value;
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
