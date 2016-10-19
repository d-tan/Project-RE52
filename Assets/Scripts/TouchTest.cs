using UnityEngine;
using System.Collections;

public class TouchTest : MonoBehaviour {

	Ray ray;

	bool isHolding = false;

	GameObject heldObject;

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

		if (Input.touchCount > 0) {

//			RaycastHit2D[] hit;

//			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				// Get Ray from touch input
				ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
				// Get all colliders hit by a 2D ray in the direction of the touch input
			RaycastHit2D[] hit = Physics2D.RaycastAll ((Vector2)ray.origin, (Vector2)ray.direction);
//			}

			Debug.DrawRay (ray.origin, ray.direction * 20, Color.red); // Draw ray

			if (hit.Length > 0) {
				// Check if last (top) collider is null
				if (hit [hit.Length - 1].collider != null) {
				
					if (!isHolding) {
						Debug.Log (hit [hit.Length - 1].collider.name);

						heldObject = hit [hit.Length - 1].collider.gameObject;
						Debug.Log ("I'm holding " + heldObject.ToString ());
					}
					
				}
			}

			if (Input.GetTouch (0).phase == TouchPhase.Moved) {
				if (heldObject != null) {
					Debug.Log ("Moving " + heldObject.ToString () + " with position: " + heldObject.transform.position.ToString());
					Debug.Log ("Moving to touch position: " + Input.GetTouch (0).position.ToString ());
					heldObject.transform.position = new Vector3(Input.GetTouch (0).position.x, Input.GetTouch (0).position.y, 0f) * (11/641);
				} else {
					Debug.Log ("Held object is null");
					isHolding = false;
				}
			}
		}
			
	}
}
