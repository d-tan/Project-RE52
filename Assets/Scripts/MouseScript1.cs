using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseScript1 : MonoBehaviour {

	public HeldObject heldObject;

	float timer = 0.0f;

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
	}

	// Detects if the player tries to pick up an object. Must be in update method
	void ObjectPickUpDetection() {

//		// Check if the screen is being touched
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
	}


	private void StartOfTouch() {
		StartTouchReset ();
		// Cast a ray from Screen into the world
//		Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 origin = ray.origin;
		// Get the object it hits
		hits = Physics2D.CircleCastAll ((Vector2)ray.origin, 0.2f, Vector2.zero);

		if (hits.Length > 0) {
			int itemIndex = SelectBestItemOnPickUp (hits, origin);
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
			if (hits [itemIndex].transform.CompareTag ("PickUpable")) {
				heldObject.myObject = hits [itemIndex].transform.gameObject; // set a reference of the object
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
//		Debug.Log ("---------------------");
		debugText.text = "";
		speeds.Clear ();
		speedTrends.Clear ();
		newPositions.Clear ();
		slowCount = 0;
	}

	List<float> speeds = new List<float>();
	List<Vector3> newPositions = new List<Vector3>();
	const float minSpeed = 4.0f;

	private void CollectSpeeds() {
		float speed = Vector3.Distance (oldTouchPos, newtouchPos) / Time.deltaTime;
		if (speed < 100.0f) {
			speeds.Add (speed);
			newPositions.Add (newtouchPos);
//			debugText.text += "Speed: " + speed + " time: " + Time.deltaTime + "\n";
//			Debug.Log ("Speed: " + speed + " time: " + Time.deltaTime);
			DetectSpeedTrend ();
			if (speed < heldObject.stationaryThreshold) {
				heldObject.initialPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
				heldObject.dragTimer = 0.0f; // object stop being dragged
//			speeds.Clear(); 
//			Debug.Log ("Speed Reset--------");
			}
		}
	}

	private enum SpeedTrend
	{
		Slowing_Down,
		Speeding_Up
	}
	List<SpeedTrend> speedTrends = new List<SpeedTrend> ();
	int slowCount = 0;
	const int slowCountThreshold = 5;
	int slowCountBoundary = 3;
	const float slowFactorLimit = 0.2f;
	int newStartIndex = 0;
	private void DetectSpeedTrend() {
		if (speeds.Count > 1) {
			if (speeds [speeds.Count - 1] > speeds [speeds.Count - 2]) {
				slowCount = 0;
//				Debug.Log ("Speeding up++++");
				speedTrends.Add (SpeedTrend.Speeding_Up);
			} else {
				slowCount++;
//				Debug.Log ("Slowing down----");
				speedTrends.Add (SpeedTrend.Slowing_Down);

				// factor to determine if there is a great drop in speed
				float slowFactor = speeds [speeds.Count - 1] / speeds [speeds.Count - 2];
				if (slowFactor <= slowFactorLimit) {
					heldObject.initialPos = newPositions [speeds.Count - 1];
//					Debug.Log ("New initial posistion set. " + slowFactor);
				}
			}
//			if (speeds.Count - newStartIndex > slowCountThreshold) {
//				slowCountBoundary = 3;
//			} else {
//				slowCountBoundary = 3;
//			}
//			if (slowCount >= slowCountBoundary) {
//				heldObject.initialPos = newPositions [speeds.Count - 1];
//				newStartIndex = speeds.Count - 1;
//				Debug.Log ("New initial posistion set");
//			}
		}
	}

	private bool DetermineSpeedTrend() {
		const int countLimit = 4;
		// tally
//		int up = 0;
//		int down = 0;
//		bool done = false;
		// Alternating check
//		SpeedTrend alternatingCheck = SpeedTrend.Slowing_Down;
//		bool slowFirst = false;
//		bool phase1Pass = false;
//		bool alternating = false;
//		bool tally = true;

		bool flick = false;

		if (speedTrends.Count < 3) {
			if (speedTrends.Count > 0) {
				flick = true;
			} else {
				flick = false;
			}
		} else {
			if (speedTrends [speedTrends.Count - 1] == SpeedTrend.Speeding_Up) {
				flick = true;
			} else {
				int sloWCount = 1;
				for (int i = 1; i < 3; i++) {
					if (speedTrends [speedTrends.Count - 1 - i] == SpeedTrend.Slowing_Down) {
						sloWCount++;
					}
				}

				if (sloWCount >= 3) {
					flick = false;
				} else {
					flick = true;
				}
			}
		}



//		// Check if list is >= 4
//		if (speedTrends.Count >= countLimit) {
//			// Get last trend
//			alternatingCheck = speedTrends [speedTrends.Count - 1];
//			if (alternatingCheck == SpeedTrend.Slowing_Down) {
//				slowFirst = true;
//			}
//			// Check if it is alternating
//			if (alternatingCheck == speedTrends [speedTrends.Count - 2]) {
//				phase1Pass = false;
//
//			} else { // alternating
//				phase1Pass = true;
//				alternatingCheck = speedTrends [speedTrends.Count - 2];
//			}
//
//			// Check if it was alternating
//			if (phase1Pass) {
//				// Check next trend to see if it is still alternating
//				if (alternatingCheck == speedTrends [speedTrends.Count - 3]) {
//					alternating = false;
//
//				} else { // IS alternating
//					alternating = true;
//					alternatingCheck = speedTrends [speedTrends.Count - 3];
//				}
//			}
//		}
//
//		Debug.Log ("Alternating: " + alternating);
//
//		// if it is alternating (Note: list is >= 4)
//		if (alternating && slowFirst) {
//			tally = false;
//			if (slowFirst) {
//				if (speedTrends [speedTrends.Count - 4] == SpeedTrend.Slowing_Down) {
//					flick = true;
//				} else {
//					flick = true;
//				}
//			}
//		}
//
//		// if tallying is required
//		if (tally) {
//			// Go through each trend backwards
//			for (int i = 0; i < countLimit; i++) {
//				switch (speedTrends.Count) {
//				case 0: // 0 trends recorded
//					done = true;
//					break;
//				case 1:  // 1 trend recorded... so on
//					if (speedTrends [speedTrends.Count - 1 - i] == SpeedTrend.Speeding_Up) {
//						up++;
//					} else {
//						down++;
//					}
//					done = true;
//					break;
//				case 2:
//					if (speedTrends [speedTrends.Count - 1 - i] == SpeedTrend.Speeding_Up) {
//						up++;
//					} else {
//						down++;
//					}
//					if (i >= 1) {
//						done = true;
//					}
//					break;
//				default:
//					// Tally up the number of speed ups and downs
//					if (speedTrends [speedTrends.Count - 1 - i] == SpeedTrend.Speeding_Up) {
//						up++;
//					} else {
//						down++;
//					}
//					if (i >= 2) {
//						done = true;
//					}
//					break;
//				}
//
//				if (done) {
//					break;
//				}
//			}
//
//			switch (speedTrends.Count) {
//			case 0: // 0 trends recorded
//				flick = false;
//				break;
//			default:
//				if (up >= down) {
//					flick = true;
//				} else {
//					flick = false;
//				}
//				break;
//			}
//		}

		return flick;
	}

	private void EndOfTouch() {
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
			DetermineAction();

		}
		StartCoroutine (IsHeldCoolDown ());
	}

	// Determines what action is to be performed when the user Ends Touch
	private void DetermineAction() {
//		if (speeds.Count > 2) {
//			if (speeds [speeds.Count - 1] - speeds [0] > 3.0f) {
//				FlickItem ();
//			} else {
//				// drop
//			}
//		} else {
//			// drop
//		}

		if (DetermineSpeedTrend ()) {
			FlickItem ();
			Debug.Log ("Flick!");
		} else {
			Debug.Log ("Drop");
		}
	}

	private void FlickItem() {
		heldObject.finalPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
		Vector3 heading = heldObject.finalPos - heldObject.initialPos;
		heading = Vector3.Normalize (heading); // normalise the heading

//		float maxSpeed = 1.0f;
//		for (int i = 0; i < speeds.Count; i++) {
//			if (speeds [i] > maxSpeed) {
//				maxSpeed = speeds [i];
//			}
//		}

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
//			myObject.layer = 8; // "Item" layer
//			rb.isKinematic = false;
//		}
//	}
//}