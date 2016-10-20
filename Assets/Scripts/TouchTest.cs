using UnityEngine;
using System.Collections;

public class TouchTest : MonoBehaviour {


	bool isHolding = false;

	GameObject heldObject;
	RaycastHit2D hit;

	Vector3 touchPos;
	Vector3 heldObjectCentre;
//	Vector3 newHeldObjectCentre;

	private float timer = 0.0f;
	private float minflickTime = 0.5f;
	private float minflickDist = 1.5f;
	private float flickTimeMultiplier = 0.5f + 4.0f;
	private float minFlickMulitplier = 0.5f;
	private Vector3 flickOrigin = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

//		if (Input.touchCount > 0) {
//
//			if (Input.GetTouch (0).phase == TouchPhase.Began) { 
//				Debug.Log ("Touch Begins at: " + Input.GetTouch (0).position.ToString ());
//			} else if (Input.GetTouch (0).phase == TouchPhase.Ended) {
//				Debug.Log ("Touch Ends at: " + Input.GetTouch (0).position.ToString ());
//			} else if (Input.GetTouch (0).phase == TouchPhase.Moved) {
//				Debug.Log ("Touch Moves through: " + Input.GetTouch (0).position.ToString ());
//			}
//		}

		timer += Time.deltaTime;

		if (Input.touchCount > 0) {
			switch (Input.GetTouch (0).phase) {
			case TouchPhase.Began:
				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
				hit = Physics2D.CircleCast ((Vector2)ray.origin, 0.05f, (Vector2)ray.direction);
//				Debug.Log ("Ray: " + ray + " hit: " + hit);
				if (hit) {
					Debug.Log (hit.transform.name);
					if (hit.transform.CompareTag ("PickUpable")) {
						heldObject = hit.transform.gameObject;
						heldObjectCentre = heldObject.transform.position;
						touchPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
						isHolding = true;
						timer = 0.0f;

						flickOrigin = touchPos;
//						Debug.Log ("Began: " + touchPos);
					}
				}
				break;

			case TouchPhase.Moved:
				if (isHolding) {
					if (heldObject) {
						touchPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
//						Debug.Log ("Moved: " + touchPos);
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
							
							Rigidbody2D HOrb = heldObject.GetComponent<Rigidbody2D> ();
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
				break;

			case TouchPhase.Canceled:
				isHolding = false;
				break;
			}

		}
			
	}
}
