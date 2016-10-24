using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// MUST be attached to conveyor belt object.
public class RubbishItemSpawner : MonoBehaviour {

	public GameObject[] items;
	public GameObject parentSpawnObject;
	float beltSpeed = 2.0f;
	float spawnTimebuffer = 0.3f;
	public Text beltText;
	public Text spawnText;

	private bool thresholdHasReached = false;
	private bool isSpawning = true;
	private bool spawnItemRoutineActive = false;

	Vector3 beltSize;
	Vector3 beltPos;
	Collider2D beltCollider;

	// Use this for initialization
	void Start () {
		beltCollider = GetComponent<Collider2D> ();

		beltPos = this.transform.position;
		beltSize = beltCollider.bounds.extents;

		StartCoroutine (SpawnItems());
	}
	
	// Update is called once per frame
	void Update () {
		if (thresholdHasReached) {
			isSpawning = false;
			ToggleItemSpawn (false);
		} else {
			isSpawning = true;
			if (!spawnItemRoutineActive) {
				ToggleItemSpawn (true);
			}
		}
	}

	// Spawn Items on the conveyor belt
	IEnumerator SpawnItems() {
		spawnItemRoutineActive = true;
		yield return new WaitForSeconds (3.0f);

		while (isSpawning) { // isSpawning
			yield return new WaitForSeconds (Random.value + spawnTimebuffer);

			// pick a random location
			float spawnPosX = Random.Range ((beltPos.x - beltSize.x), (beltPos.x + beltSize.x));
			float spawnPosY = beltCollider.transform.position.y - beltSize.y;

			// Pick random Item
			int index = (int)Mathf.Clamp (Mathf.Floor (Random.Range (0.0f, items.Length)), 0.0f, items.Length - 1);

			// Spawn the Item
			GameObject spawnedObject = Instantiate (
				                          items [index], 
				                          new Vector3 (spawnPosX, spawnPosY, beltPos.z), 
				                          Quaternion.identity, 
				                          parentSpawnObject.transform
			                          ) as GameObject;

			// Move the Item if it's off the edge
			Collider2D spawnedObjectCollider = spawnedObject.GetComponent<Collider2D> ();
			spawnedObject.transform.position = new Vector3 (Mathf.Clamp (
				spawnedObject.transform.position.x,
				(beltPos.x - beltSize.x) + spawnedObjectCollider.bounds.size.x,
				(beltPos.x + beltSize.x) - spawnedObjectCollider.bounds.size.x),
				spawnedObject.transform.position.y,
				spawnedObject.transform.position.z
			);
		}

		spawnItemRoutineActive = false;
	}

	// Turns the item spawner on/off depending on what is given
	void ToggleItemSpawn(bool toggleState) {
		if (toggleState) {
			StartCoroutine (SpawnItems ());
			isSpawning = true;
		} else {
			StopCoroutine (SpawnItems ());
			isSpawning = false;
		}
	}

	// Determines if the Threshold (if the conveyor belt is back-logged) has been reached
	public bool ThresholdReached {
		get { return thresholdHasReached; }
		set { thresholdHasReached = value; }
	}

	void OnTriggerEnter2D (Collider2D other){
		SetVelocityToItem (other, beltSpeed);
		SetEndOfBelt (other, false);
	}

	void OnTriggerStay2D (Collider2D other) {
		SetVelocityToItem (other, beltSpeed);
		SetEndOfBelt (other, false);
	}

	void OnTriggerExit2D (Collider2D other) {
		SetVelocityToItem (other, -beltSpeed);
		SetEndOfBelt (other, true);
	}

	// Set the velocity of an item
	void SetVelocityToItem(Collider2D other, float velocityY) {
		if (other.CompareTag ("PickUpable")) {
			Rigidbody2D rb = other.GetComponent<Rigidbody2D> ();
			RubbishItem otherScipt = other.GetComponent<RubbishItem> ();

			if ((bool)rb && (bool)otherScipt) {
				if (other.gameObject.layer != 9) { // 9 = "HeldItem" layer
					rb.velocity = new Vector2 (0.0f, velocityY);
				}
			} else {
				Debug.Log ("This item, " + other.ToString() + ", does not have a rigidbody");
			}
		}
	}

	// Tell the item it is at the end of the conveyor belt
	private void SetEndOfBelt(Collider2D other, bool status) {

		RubbishItem otherScipt = other.GetComponent<RubbishItem> ();

		if (otherScipt) {
			otherScipt.AtEnd = status;

			if (status && (other.gameObject.layer != 9)) { // 9 = "HeldItem" layer
				Rigidbody2D otherRB = other.GetComponent<Rigidbody2D> ();
				otherRB.isKinematic = true;
			}
		}
	}





	// --------------- UI Stuff ---------------
	public void IncreaseBuffer() {
		spawnTimebuffer += 0.1f;
		spawnText.text = "SpawnBuffer: " + spawnTimebuffer;
	}

	public void DecreaseBuffer() {
		spawnTimebuffer -= 0.1f;
		spawnText.text = "SpawnBuffer: " + spawnTimebuffer;
	}

	public void IncreaseSpeed() {
		beltSpeed += 0.1f;
		beltText.text = "BeltSpeed: " + beltSpeed;
	}

	public void DecreaseSpeed() {
		beltSpeed -= 0.1f;
		beltText.text = "BeltSpeed: " + beltSpeed;
	}

	public void ToggleBelt() {
		ToggleItemSpawn (!isSpawning);
	}
}
