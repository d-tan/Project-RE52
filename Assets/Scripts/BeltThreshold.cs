using UnityEngine;
using System.Collections;

public class BeltThreshold : MonoBehaviour {

	bool thresholdReached = false;
	float timer = 0.0f;
	float waitTime = 1.0f;

	RubbishItemSpawner parentScript;
	Collider2D basisCollider = null;

	// Use this for initialization
	void Start () {
		parentScript = transform.parent.GetComponent<RubbishItemSpawner> ();

		if (!parentScript) {
			Debug.Log ("This is not my Parent, Human!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		

		if (thresholdReached) {
			if (parentScript) {
				parentScript.ThresholdReached = true;
			}
		} else {
			if (parentScript) {
				parentScript.ThresholdReached = false;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (DetectItemScript (other)) {
//			timer = 0.0f;
		}
	}

	void OnTriggerStay2D (Collider2D other) {

		if (basisCollider == null) {
			basisCollider = other;
		}

		if (basisCollider.Equals (other)) {
			timer += Time.deltaTime;
		}

		if (timer >= waitTime) {
			if (DetectItemScript (other)) {
				thresholdReached = true;
			}

			timer = 0.0f;
			basisCollider = null;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (DetectItemScript(other)) {
			thresholdReached = false;
			if (basisCollider) {
				if (basisCollider.Equals (other)) {
					basisCollider = null;
				}
			}
		}
	}

	bool DetectItemScript(Collider2D other) {
		RubbishItem otherScipt = other.GetComponent<RubbishItem> ();

		return (bool)otherScipt;
	}
}
