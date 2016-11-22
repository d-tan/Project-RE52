using UnityEngine;
using System.Collections;

public class BeltThreshold : MonoBehaviour {

	bool thresholdReached = false;
	float timer = 0.0f;
	float waitTime = 1.5f;

	Collider2D basisCollider = null;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (thresholdReached) {
			RubbishItemSpawner.singleton.ThresholdReached = true;

		} else {
			RubbishItemSpawner.singleton.ThresholdReached = false;
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
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (DetectItemScript(other)) {
			if (basisCollider) {
				if (basisCollider.Equals (other)) {
					thresholdReached = false;
					basisCollider = null;
					timer = 0.0f;
				}
			}
		}
	}

	bool DetectItemScript(Collider2D other) {
		RubbishItem otherScipt = other.GetComponent<RubbishItem> ();

		return (bool)otherScipt;
	}
}
