using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemCount : MonoBehaviour {

	public Text itemCountText;
	public Image glowImage;

//	private int itemCount = 0;
	private float flashTime = 0.2f;
	private Renderer myRenderer;
	private Color originalColour;

	public RubbishType acceptedType;

	// Use this for initialization
	void Start () {
		myRenderer = GetComponent<Renderer> ();
		originalColour = GetComponent<SpriteRenderer> ().color;

//		glowImage.color = new Color (originalColour.r, originalColour.g, originalColour.b, 150.0f/255.0f);
		glowImage.color = new Color (1, 1, 1, 150.0f/255.0f);
		glowImage.gameObject.SetActive (false);
	}

	void OnTriggerEnter2D (Collider2D other) {
		TriggerCount (other);
	}

	void OnTriggerStay2D (Collider2D other) {
		TriggerCount (other);
	}


	void TriggerCount(Collider2D other) {
		// Get script from item
		RubbishItem otherScript = other.GetComponent<RubbishItem> ();

		// Check if it's a rubbish item
		if (otherScript) {
			if (!otherScript.IsBeingHeld) {
				// Check if the types match
				if (otherScript.MyRubbishTypes[0] == acceptedType || otherScript.MyRubbishTypes[1] == acceptedType) {
					ScoreManager.singleton.AddMinusScore (acceptedType, 1);
					StartCoroutine (CorrectFlash ());
				} else {
					ScoreManager.singleton.AddMinusScore (acceptedType, -1);
					StartCoroutine (IncorrectFlash ());
				}
				Destroy (other.gameObject);
//				SetCountText (itemCount);
			}
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

	public void GlowIndicator(bool state) {
		glowImage.gameObject.SetActive (state);
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
