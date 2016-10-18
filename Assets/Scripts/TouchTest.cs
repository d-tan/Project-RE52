using UnityEngine;
using System.Collections;

public class TouchTest : MonoBehaviour {

	Ray ray;

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

		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {

			// Get Ray from touch input
			ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
			// Get all colliders hit by a 2D ray in the direction of the touch input
			RaycastHit2D[] hit = Physics2D.RaycastAll((Vector2)ray.origin,(Vector2)ray.direction);

			Debug.DrawRay (ray.origin, ray.direction * 20, Color.red); // Draw ray

			// Check if last (top) collider is null
			if ( hit[hit.Length - 1].collider != null )
			{
				Debug.Log( hit[hit.Length - 1].collider.name );
			}
		}
			
	}
}
