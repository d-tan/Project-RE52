using UnityEngine;
using System.Collections;

public enum RubbishType {
	General,
	Organic,
	Recycle
}

public class RubbishItem : MonoBehaviour {

	public RubbishType myType;
	public float beltSpeed = 0.5f;

//	private Collider2D itemCollider;
	private Rigidbody2D rb;
	bool endOfConveyorbelt = false;
	bool isHeld = false;

	void Start() {
//		itemCollider = GetComponent<Collider2D> ();
		rb = GetComponent<Rigidbody2D> ();

		// Defaults of a rubbish item
		rb.mass = 10.0f;
		this.tag = "PickUpable";
		this.gameObject.layer = 8; // "Item" layer
	}

	public RubbishType ThisRubbishType {
		get { 
			return myType;
		}
	}


	void Update() {

	}

//	void OnCollisionEnter2D (Collision2D other) {
//		// Check if other collider is a pick-upable item
//		if (other.collider.CompareTag ("PickUpable")) {
//			RubbishItem otherScript = other.collider.GetComponent<RubbishItem> ();
//
//			// Check if other collider has script
//			if (otherScript) {
//				
//				// Check if other item is at the end of the belt
//				if (otherScript.AtEnd) {
//
//					Rigidbody2D otherRB = other.collider.GetComponent<Rigidbody2D> ();
//
//					// Check if other item has a rigidbody2D
//					if (otherRB) {
//						otherRB.velocity = Vector2.zero;
//					} else {
//						Debug.Log ("Other collider does not have a RigidBody");
//					}
//
//					// Check if this item has a rigidbody2D
//					if (rb) {
//						rb.velocity = Vector2.zero;
//						this.AtEnd = true;
//					} else {
//						Debug.Log ("This collider does not have a RigidBody");
//					}
//				}
//			} else {
//				Debug.Log ("Collided object does not have a RubbishItem Script");
//			}
//		}
//	}

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
