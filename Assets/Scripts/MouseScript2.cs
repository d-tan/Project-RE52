using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseScript2 : MonoBehaviour {

	public HeldObject heldObject;

	float timer = 0.0f;
	float releaseTimer = 0.0f;
	bool itemRelease = false;

//	private float timer = 0.0f;
	private float maxFlickTime = 0.5f; // maximum amount of time needed to be counted as a flick
	private float minFlickTime = 0.01f; // minimum amount of time needed to be counted as a flick
	private float minflickDist = 0.5f;
	private float flickTimeMultiplier = 0.5f + 4.0f;
	private float minFlickMulitplier = 0.5f;
	private float maxFlickMultiplier = 3.0f;
	private Vector3 flickOrigin = Vector3.zero;

	private float heldCoolDownTimer = 5.0f;

//	GameObject heldObject;
	RaycastHit2D[] hits;

	Vector3 oldTouchPos;
	Vector3 newtouchPos;

	ItemDatabase itemDatabase;
	List<string> itemNames = new List<string> ();

	Text debugText;

	void Start() {
		debugText = GameObject.FindGameObjectWithTag ("Respawn").GetComponent<Text> ();
		debugText.text = "";
		itemDatabase = GameObject.FindGameObjectWithTag ("Databases").GetComponent<ItemDatabase> ();

		bool moreItems = true;
		int index = 0;

		while (moreItems) {
			Item item = itemDatabase.FetchItemByID (index);
			if (item.ID != -1) {
				itemNames.Add (item.Title);
			} else {
				moreItems = false;
			}
			index++;
		}
	}

	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		ObjectPickUpDetection ();
		CheckItemRelease ();
	}

	// Detects if the player tries to pick up an object. Must be in update method
	void ObjectPickUpDetection() {
		Debug.DrawLine(heldObject.initialPos, heldObject.finalPos);
		// Check if the screen is being touched
//		if (Input.touchCount > 0) {
//			// Check which touch phase 
//			switch (Input.GetTouch (0).phase) {
//			case TouchPhase.Began:
//				StartOfTouch();
//				break;
//
//			case TouchPhase.Moved:
//				DragTouch();
//				break;
//
//			case TouchPhase.Ended:
//				EndOfTouch();
//				break;
//
//			case TouchPhase.Canceled:
//				heldObject.isBeingHeld = false;
//				StartCoroutine (IsHeldCoolDown ());
//				break;
//			}
//
//			if (heldObject.isBeingHeld) {
//				CollectSpeeds ();
//			}
//
//		}

		if (Input.GetMouseButtonDown (0)) {
			StartOfTouch ();
		} else if (Input.GetMouseButton (0)) {
			DragTouch ();
		} else if (Input.GetMouseButtonUp (0)) {
			EndOfTouch ();
		} else {
			heldObject.isBeingHeld = false;
			StartCoroutine (IsHeldCoolDown ());
		}

		if (heldObject.isBeingHeld) {
			CollectSpeeds ();
		}

	}


	private void StartOfTouch() {
		StartTouchReset ();
		// Cast a ray from Screen into the world
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Vector3 origin = ray.origin;
		// Get the object it hits
		hits = Physics2D.CircleCastAll ((Vector2)ray.origin, 0.2f, Vector2.zero);



		// Check if the ray hit anything
		if (hits.Length > 0) {

			int itemIndex = SelectBestItemOnPickUp(hits, origin);
//			for (int i = hits.Length - 1; i >= 0; i--) {
//				RubbishItem objectScript = hits [i].transform.GetComponent<RubbishItem> ();
//				if (hits [i].transform.CompareTag ("PickUpable")) {
//					Debug.Log ("raycast: " + objectScript.RubbishItemID + " name: " + itemNames[objectScript.RubbishItemID]);
//					itemIndex = i;
//				}
//
//				ShowBinStats (hits[i].transform.gameObject);
//			}

			// Check if the object is an item
			if (hits[itemIndex].transform.CompareTag ("PickUpable")) {
				heldObject.myObject = hits[itemIndex].transform.gameObject; // set a reference of the object
//				heldObjectCentre = heldObject.transform.position; // get it's centre

				SetUpHeldObject ();
				heldObject.dragTimer = 0.0f;
			}
		}
	}

	private void DragTouch() {
		if (heldObject.isBeingHeld) {
			if (heldObject.myObject) {
				// Object is being dragged
				heldObject.dragTimer += Time.deltaTime;

				oldTouchPos = newtouchPos;
				// Move object to new touch position
				newtouchPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
//				CollectSpeeds (touchPos, newTouchPos); // Collect the speeds of dragging the item

				heldObject.myObject.transform.position = new Vector3 (newtouchPos.x, newtouchPos.y, heldObject.myObject.transform.position.z);
			}
		}
	}

	private void StartTouchReset() {
		Debug.Log ("---------------------");
//		debugText.text = "";
		heldObject.initialPos = Vector3.zero;
		releaseTimer = 0.0f;
		slowCount = 0;
		itemRelease = false;

	}


	const float minSpeed = 4.0f;
	const float slowFactor = 0.32f;
	float previousSpeed = 0.0f;
	float lastSpeed = 0.0f;
	int slowCount = 0;
	const int minSlowCount = 3;
	Vector3 tempPos = Vector3.zero;

	private void CollectSpeeds() {
//		Debug.Log (Vector3.Distance (oldTouchPos, newtouchPos));
		previousSpeed = lastSpeed;
		lastSpeed = Vector3.Distance (oldTouchPos, newtouchPos) / Time.deltaTime;
		if (lastSpeed < 100.0f) {
//			sum += lastSpeed;
//			count++;
//			Debug.Log ("LastSpeed: " + lastSpeed);
//			if (lastSpeed < previousSpeed) {
////				Debug.Log ("New position set... by MIN-SPEED");
//				// if new Slow then get current position
//				if (slowCount == 0) {
//					tempPos = newtouchPos;
//				}
//				// Check if slowCount is over minimum
//				slowCount++;
//				if (slowCount >= minSlowCount) {
//					slowCount = 0;
//					heldObject.finalPos = tempPos;
////					Debug.Log ("New position set... by Slow-Count");
//				}
//
//				// Check for zero division
//				if (previousSpeed > 0.0f) {
//					// Check if the division is <= slow factor (smaller than slow factor means there is a great drop in speed
//					if (lastSpeed / previousSpeed <= slowFactor) {
//						heldObject.initialPos = newtouchPos;
////						Debug.Log ("New position set... by Slow-Factor");
//					}
//				}
//			} else {
//				slowCount = 0;
//			}
			heldObject.initialPos = newtouchPos;
		}
	}

	private void EndOfTouch() {
		itemRelease = true;
		heldObject.isBeingHeld = false; // turn off this holding bool

		if (heldObject.myObject) {
			heldObject.script.IsBeingHeld = false;
			// Check if time elapsed is within flick time requirements
//			if ((timer > minFlickTime) && (timer < maxFlickTime)) {
//				float timeTaken = timer;
//
//				Vector3 finalTouchPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
//
//				if (Vector2.Distance ((Vector2)flickOrigin, (Vector2)finalTouchPos) > minflickDist) {
////					Rigidbody2D HOrb = heldObject.GetComponent<Rigidbody2D> ();
//					Vector3 heading = finalTouchPos - flickOrigin;
//					heading = Vector3.Normalize (heading); // normalise the heading
//
//					if (heldObject.rb) {
//						heldObject.rb.velocity += (Vector2)heading * (flickTimeMultiplier + timeTaken) /
//							Mathf.Clamp (timeTaken, minFlickMulitplier, maxFlickMultiplier);
//					} else {
//						Debug.Log ("This item does not have a Rigidbody2D");
//					}
//				} 
//			}
//			DetermineAction();
			if (lastSpeed > minSpeed) {
				FlickItem ();
			} else {
				Debug.Log ("Not Flick!");
			}

		}
//		StartCoroutine (IsHeldCoolDown ());
	}

	private void CheckItemRelease() {
		if (itemRelease) {
			releaseTimer += Time.deltaTime;
		}
		if (releaseTimer >= heldCoolDownTimer) {
			Debug.Log ("Item reverted to normal layer (8)");
			heldObject.SwitchObjectLayerToHolding (false);
			itemRelease = false;
			releaseTimer = 0.0f;
		}
	}

	private void FlickItem() {
		heldObject.finalPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector3 heading = heldObject.finalPos - heldObject.initialPos;
		heading = Vector3.Normalize (heading); // normalise the heading

		heldObject.rb.velocity += (Vector2)heading * 20.0f;
	}

	// Turns off the 'held' boolean and other things of the heldObject
	IEnumerator IsHeldCoolDown () {
		yield return new WaitForSeconds (heldCoolDownTimer);

		if (heldObject.myObject) {
			heldObject.SwitchObjectLayerToHolding (false);
		}
	}

	// Finds the most suitable item to pick up based on some criteria
	private int SelectBestItemOnPickUp(RaycastHit2D[] hits, Vector3 origin) {
		List<int> indexes = new List<int>();
		List<float> distances = new List<float> ();
		int itemIndex = 0;

		// collect indexes of raycast hits that are pickupable
		for (int i = 0; i < hits.Length; i++) {
			ShowBinStats (hits[i].transform.gameObject);
//			RubbishItem objectScript = hits [i].transform.GetComponent<RubbishItem> ();
			if (hits [i].transform.CompareTag ("PickUpable")) {
				itemIndex = i;
//				Debug.Log ("raycast: " + objectScript.RubbishItemID + 
//					" name: " + itemNames[objectScript.RubbishItemID] + 
//					" Distance: " + Vector3.Distance(origin, hits [i].transform.position));

				// check if list has items in it
				if (indexes.Count == 0) {
					indexes.Add (i);

					distances.Add (Vector3.Distance(origin, hits [i].transform.position));
				} else {
					int insertIndex = -1;
					float distance2 = Vector3.Distance(origin, hits [i].transform.position);

					// collect distances of those hits from origin and sort them in ASC order
					for (int j = 0; j < indexes.Count; j++) {
						float distance1 = Vector3.Distance(origin, hits [indexes [j]].transform.position);
						//							Debug.Log ("Comparing: 2: " + distance2 + " to 1: " + distance1);

						if (distance2 < distance1) {
							insertIndex = j;
							break;
						}
					}
					if (insertIndex == -1) {
						indexes.Add (i);
						distances.Add (distance2);
					} else {
						indexes.Insert (insertIndex, i);
						distances.Insert (insertIndex, distance2);
					}
				}
			}
		}

		// Filter the hits that are too far from the closest hit
		for (int i = 0; i < indexes.Count; i++) {
//			Debug.Log (indexes [i]);
			if (distances[0] - distances[i] + 0.14 < 0) {
				indexes.RemoveAt (i);
				i--;
			}
		}

		// Find the topmost item from the remaining hits
		for (int i = 0; i < indexes.Count; i++) {
			if (i == 0) {
				itemIndex = indexes [0];
			} else {
//				Debug.Log (indexes [i]);
				if (hits [itemIndex].transform.GetSiblingIndex () < hits [i].transform.GetSiblingIndex ()) {
					itemIndex = indexes [i];
				}
			}
		}

		return itemIndex;
	}

	private void SetUpHeldObject() {
		// set the current touch position relative to world co-ordinates
		newtouchPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		heldObject.isBeingHeld = true;

		timer = 0.0f; // reset timer
		flickOrigin = newtouchPos;
		heldObject.initialPos = newtouchPos;

		// get references to heldObject's Rigidbody 2D and Item script
		heldObject.rb = heldObject.myObject.GetComponent<Rigidbody2D> ();
		heldObject.script = heldObject.myObject.GetComponent<RubbishItem> ();
		//		HighlightBin(HOScript); // Highlight the bin --- UI ---

		// Check if there is a script 
		heldObject.script.IsBeingHeld = true; // tell the object it is being held
		heldObject.SwitchObjectLayerToHolding(true);
		//		UIManager.ChangeIndicatorColors(true, HOScript.RubbishTypes[0], HOScript.RubbishTypes[1]);

		heldObject.rb.velocity = Vector2.zero; // reset velocity

	}

	// Checks if touched object is a bin, then call script's display text method
	private void ShowBinStats(GameObject hitTarget) {
		ItemCount targetScript = hitTarget.GetComponent<ItemCount> ();

		if (targetScript) {
			targetScript.DisplayText ();
		}
	}

}

//[System.Serializable]
//public class HeldObject : System.Object {
//	public GameObject myObject;
//	public RubbishItem script;
//	public Rigidbody2D rb;
//	public bool isBeingHeld;
//
//	public Vector3 initialPos;
//	public Vector3 finalPos;
//
//	public float dragTimer = 0.0f;
//				public float stationaryThreshold = 1.0f;
//
//	public void SwitchObjectLayerToHolding(bool state) {
//		if (state) {
//			myObject.layer = 9; // "HeldItem" layer // set the layer so it's not interactible with other items
////			rb.isKinematic = true;
//		} else {
//			if (myObject) {
//				myObject.layer = 8; // "Item" layer
//				rb.isKinematic = false;
//			}
//		}
//	}
//}