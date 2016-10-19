using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemCount : MonoBehaviour {

	public Text itemCountText;
	private int itemCount = 0;

	public RubbishType acceptedType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		
		// Get script from item
		RubbishItem script = other.GetComponent<RubbishItem> ();

		// Check if it's a rubbish item
		if (script != null) {

			// Check if the types match
			if (script.ThisRubbishType == acceptedType) {
				itemCount++;
			} else {
				itemCount--;
			}
			Destroy (other.gameObject);
			SetCountText (itemCount);
		}
	}

	// Set the UI Text
	void SetCountText(int givenCount) {
		if (itemCountText != null) {
			itemCountText.text = givenCount.ToString ();
		} else {
			Debug.Log ("My UI Text is null");
		}
	}
}
