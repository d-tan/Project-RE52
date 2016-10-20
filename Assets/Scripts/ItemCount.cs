using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemCount : MonoBehaviour {

	public Text itemCountText;

	private int itemCount = 0;
	private float flashTime = 0.2f;
	private Renderer myRenderer;
	private Color originalColour;

	public RubbishType acceptedType;

	// Use this for initialization
	void Start () {
		myRenderer = GetComponent<Renderer> ();

		if (myRenderer) {
			originalColour = myRenderer.material.color;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		
		// Get script from item
		RubbishItem script = other.GetComponent<RubbishItem> ();

		// Check if it's a rubbish item
		if (script) {

			// Check if the types match
			if (script.ThisRubbishType == acceptedType) {
				itemCount++;
				StartCoroutine (CorrectFlash ());
			} else {
				itemCount--;
				StartCoroutine (IncorrectFlash ());
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

	IEnumerator IncorrectFlash() {
		myRenderer.material.color = Color.black;

		yield return new WaitForSeconds (flashTime);

		myRenderer.material.color = originalColour;

		StopCoroutine (IncorrectFlash ());
	}

	IEnumerator CorrectFlash() {
		myRenderer.material.color = Color.clear;

		yield return new WaitForSeconds (flashTime/2);

		myRenderer.material.color = originalColour;

		StopCoroutine (CorrectFlash ());
	}
}
