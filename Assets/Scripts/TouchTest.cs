using UnityEngine;
using System.Collections;

public class TouchTest : MonoBehaviour {

	bool isHolding = false;

	GameObject heldObject;
	Rigidbody2D HOrb;
	RubbishItem HOScript;
	RaycastHit2D[] hits;

	Vector3 touchPos;
//	Vector3 heldObjectCentre;
//	Vector3 newHeldObjectCentre;

	private float timer = 0.0f;
	private float maxFlickTime = 0.5f; // maximum amount of time needed to be counted as a flick
	private float minFlickTime = 0.01f; // minimum amount of time needed to be counted as a flick
	private float minflickDist = 0.5f;
	private float flickTimeMultiplier = 0.5f + 4.0f;
	private float minFlickMulitplier = 0.5f;
	private float maxFlickMultiplier = 3.0f;
	private Vector3 flickOrigin = Vector3.zero;

	private float heldCoolDownTimer = 5.0f;

	GameUIManager UIManager;
	GameObject binParent;
	ItemCount[] binScripts;

	void Start() {
		UIManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameUIManager> ();

		// Get A reference to the bin scripts
		binParent = GameObject.FindGameObjectWithTag ("BinParent");
		binScripts = new ItemCount[3]; // 3 bins
		for (int i = 0; i < binParent.transform.childCount; i++) {
			ItemCount script = binParent.transform.GetChild (i).GetComponent<ItemCount>();

			// Set scripts in order general, organic, recycle
			switch (script.acceptedType) {
			case RubbishType.General:
				binScripts [0] = script;
				break;
			case RubbishType.Organic:
				binScripts [1] = script;
				break;
			case RubbishType.Recycle:
				binScripts [2] = script;
				break;
			}
		}

		// binScripts array must be filled and NOT null
		for (int i = 0; i < binScripts.Length; i++) {
			Debug.Assert (binScripts [i]);
		}
	}

	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		ObjectPickUpDetection ();
	}

	// Detects if the player tries to pick up an object. Must be in update method
	void ObjectPickUpDetection() {

		// Check if the screen is being touched
		if (Input.touchCount > 0) {
			// Check which touch phase 
			switch (Input.GetTouch (0).phase) {
			case TouchPhase.Began:
				StartOfTouch();
				break;

			case TouchPhase.Moved:
				DragTouch();
				break;

			case TouchPhase.Ended:
				EndOfTouch();
				break;

			case TouchPhase.Canceled:
				isHolding = false;
				StartCoroutine (IsHeldCoolDown ());
				break;
			}

		}
	}


	private void StartOfTouch() {
		// Cast a ray from Screen into the world
		Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
		// Get the object it hits
		hits = Physics2D.CircleCastAll ((Vector2)ray.origin, 0.3f, Vector2.zero);

		// Check if the ray hit anything
		if (hits.Length > 0) {

			int itemIndex = 0;
			for (int i = hits.Length - 1; i >= 0; i--) {
				if (hits [i].transform.CompareTag ("PickUpable")) {
					itemIndex = i;
				}

				ShowBinStats (hits[i].transform.gameObject);
			}
			// Check if the object is an item
			if (hits[itemIndex].transform.CompareTag ("PickUpable")) {
				heldObject = hits[itemIndex].transform.gameObject; // set a reference of the object
//				heldObjectCentre = heldObject.transform.position; // get it's centre

				// set the current touch position relative to world co-ordinates
				touchPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
				isHolding = true;

				timer = 0.0f; // reset timer
				flickOrigin = touchPos;

				// get references to heldObject's Rigidbody 2D and Item script
				HOrb = heldObject.GetComponent<Rigidbody2D> ();
				HOScript = heldObject.GetComponent<RubbishItem> ();
				HighlightBin(HOScript); // Highlight the bin --- UI ---

				// Check if there is a script 
				if (HOScript) {
					HOScript.IsBeingHeld = true; // tell the object it is being held
					heldObject.layer = 9; // "HeldItem" layer // set the layer so it's not interactible with other items
//					UIManager.ChangeIndicatorColors(true, HOScript.RubbishTypes[0], HOScript.RubbishTypes[1]);
				}

				// Check if there is a Rigidbody2D
				if (HOrb) {
					HOrb.velocity = Vector2.zero; // reset velocity
				}
			}
		}
	}

	private void DragTouch() {
		if (isHolding) {
			if (heldObject) {
				// Move object to new touch position
				touchPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
				heldObject.transform.position = new Vector3 (touchPos.x, touchPos.y, heldObject.transform.position.z);

			}
		}
	}

	private void EndOfTouch() {
		isHolding = false; // turn off this holding bool 
		// turn off item's holding bool
		if (HOScript) {
			HighlightBin (HOScript);
			HOScript.IsBeingHeld = false;
//			UIManager.ChangeIndicatorColors (false);
		}

		if (heldObject) {
			// Check if time elapsed is within flick time requirements
			if ((timer > minFlickTime) && (timer < maxFlickTime)) {
				float timeTaken = timer;

				Vector3 finalTouchPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);

				if (Vector2.Distance ((Vector2)flickOrigin, (Vector2)finalTouchPos) > minflickDist) {
					//							Rigidbody2D HOrb = heldObject.GetComponent<Rigidbody2D> ();
					Vector3 heading = finalTouchPos - flickOrigin;
					heading = Vector3.Normalize (heading); // normalise the heading

					if (HOrb) {
						HOrb.velocity += (Vector2)heading * (flickTimeMultiplier + timeTaken) /
							Mathf.Clamp (timeTaken, minFlickMulitplier, maxFlickMultiplier);
					} else {
						Debug.Log ("This item does not have a Rigidbody2D");
					}
				} 
			}
		}
		StartCoroutine (IsHeldCoolDown ());
	}

	// Turns off the 'held' boolean and other things of the heldObject
	IEnumerator IsHeldCoolDown () {
		yield return new WaitForSeconds (heldCoolDownTimer);

		if (heldObject) {
			heldObject.layer = 8; // "Item" layer
			if (HOrb) {
				HOrb.isKinematic = false;
			}
		}


		StopCoroutine (IsHeldCoolDown ());
	}

	private void HighlightBin(RubbishItem script) {
		for (int i = 0; i < script.RubbishTypes.Count; i++) {
			switch (script.RubbishTypes[i]) {
			case RubbishType.General:
				binScripts [(int)RubbishType.General].GlowIndicator (isHolding);
				break;
			case RubbishType.Organic:
				binScripts [(int)RubbishType.Organic].GlowIndicator (isHolding);
				break;
			case RubbishType.Recycle:
				binScripts [(int)RubbishType.Recycle].GlowIndicator (isHolding);
				break;
			}
		}
	}


	// Checks if touched object is a bin, then call script's display text method
	private void ShowBinStats(GameObject hitTarget) {
		ItemCount targetScript = hitTarget.GetComponent<ItemCount> ();

		if (targetScript) {
			targetScript.DisplayText ();
		}
	}
}
