using UnityEngine;
using System.Collections;

public class TouchTest : MonoBehaviour {


	bool isHolding = false;

	GameObject heldObject;
	Rigidbody2D HOrb;
	RubbishItem HOScript;
	RaycastHit2D hit;

	Vector3 touchPos;
//	Vector3 heldObjectCentre;
//	Vector3 newHeldObjectCentre;

	private float timer = 0.0f;
	private float minflickTime = 0.5f;
	private float minflickDist = 0.5f;
	private float flickTimeMultiplier = 0.5f + 4.0f;
	private float minFlickMulitplier = 0.5f;
	private Vector3 flickOrigin = Vector3.zero;

	private float boolTimer = 0.2f;

	// Use this for initialization
	void Start () {
	
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
				// Cast a ray from Screen into the world
				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
				// Get the object it hits
				hit = Physics2D.CircleCast ((Vector2)ray.origin, 0.3f, (Vector2)ray.direction);

				// Check if the ray hit anything
				if (hit) {
					// Check if the object is an item
					if (hit.transform.CompareTag ("PickUpable")) {
						heldObject = hit.transform.gameObject; // set a reference of the object
//						heldObjectCentre = heldObject.transform.position; // get it's centre
						// set the current touch position relative to world co-ordinates
						touchPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
						isHolding = true;

						timer = 0.0f; // reset timer
						flickOrigin = touchPos;

						// get references to heldObject's Rigidbody 2D and Item script
						HOrb = heldObject.GetComponent<Rigidbody2D> ();
						HOScript = heldObject.GetComponent<RubbishItem> ();

						// Check if there is a script 
						if (HOScript) {
							HOScript.IsBeingHeld = true; // tell the object it is being held
							heldObject.layer = 9; // "HeldItem" layer // set the layer so it's not interactible with other items
						}
					}
				}
				break;

			case TouchPhase.Moved:
				if (isHolding) {
					if (heldObject) {
						touchPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
						heldObject.transform.position = new Vector3 (touchPos.x, touchPos.y, heldObject.transform.position.z);

					}
				}
				break;

			case TouchPhase.Ended:
				isHolding = false;

				if (heldObject) {
					if (timer < minflickTime) {
						float timeTaken = timer;

						Vector3 currentTouchPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);

						if (Vector2.Distance ((Vector2)flickOrigin, (Vector2)currentTouchPos) > minflickDist) {

							//							Rigidbody2D HOrb = heldObject.GetComponent<Rigidbody2D> ();
							Vector3 heading = currentTouchPos - flickOrigin;

							if (HOrb) {
								HOrb.velocity += (Vector2)heading * flickTimeMultiplier / 
									Mathf.Clamp (timeTaken, minFlickMulitplier, flickTimeMultiplier);
							} else {
								Debug.Log ("This item does not have a Rigidbody2D");
							}
						}
					}
				}
				StartCoroutine (TurnOffHoldingBool ());
				break;

			case TouchPhase.Canceled:
				isHolding = false;
				StartCoroutine (TurnOffHoldingBool ());
				break;
			}

		}
	}

	// Turns off the 'held' boolean and other things of the heldObject
	IEnumerator TurnOffHoldingBool () {
		yield return new WaitForSeconds (boolTimer);
		if (HOScript) {
			HOScript.IsBeingHeld = false;
			if (heldObject) {
				heldObject.layer = 8; // "Item" layer
				if (HOrb) {
					HOrb.isKinematic = false;
				}
			}
		}

		StopCoroutine (TurnOffHoldingBool ());
	}
}
