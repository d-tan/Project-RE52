using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// MUST be attached to conveyor belt object.
public class RubbishItemSpawner : MonoBehaviour {

	public static RubbishItemSpawner singleton;

	public GameObject[] items;
	public GameObject parentSpawnObject;
	public GameObject genericItem;

	private float beltSpeed = 2.0f; // Belt Speed details
	private float initialBeltSpeed;
	private float beltSpeedMax = 8.0f;

	private float spawnTimeBuffer = 0.35f; // Spawn Time Buffer details
	private float initialSpawnTimeBuffer;
	private float spawnTimeBufferMin = -0.4f;

	private bool thresholdHasReached = false; // Spawning details
	private bool isSpawning = true;
	private bool spawnItemRoutineActive = false;

	Vector3 beltSize;
	Vector3 beltPos;
	Collider2D beltCollider;

	ItemDatabase database;

	void Awake() {
		if (singleton == null) {
			singleton = this;
		}
	}

	// Use this for initialization
	void Start () {
		beltCollider = GetComponent<Collider2D> ();

		beltPos = this.transform.position;
		beltSize = beltCollider.bounds.extents;

		StartCoroutine (SpawnItems());

		initialBeltSpeed = beltSpeed; // hold the initial values
		initialSpawnTimeBuffer = spawnTimeBuffer;

		database = GameObject.FindGameObjectWithTag ("ItemDatabase").GetComponent<ItemDatabase> ();

		GameObject tempObject = Instantiate (genericItem);
		Item item = database.FetchItemByID (0);
		tempObject.GetComponent<SpriteRenderer> ().sprite = item.Sprite;
		RubbishType[] types = new RubbishType[2];
		types [0] = item.Type1;
		types [1] = item.Type2;
		tempObject.GetComponent<RubbishItem> ().ThisRubbishTypes = types;
		tempObject.AddComponent<PolygonCollider2D> ();
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
		yield return new WaitForSeconds (1.0f);

		while (isSpawning) { // isSpawning
			yield return new WaitForSeconds (
				Mathf.Clamp(Random.value + spawnTimeBuffer, 0.05f, initialSpawnTimeBuffer + 1.0f));

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

			// Move the Item if it's on the edge
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


	public void ChangeSpawnTime() {
		if (spawnTimeBuffer > spawnTimeBufferMin) {
			if (ScoreManager.singleton.multiplier < 4) {
				spawnTimeBuffer = Mathf.Clamp (
					Mathf.Exp(-ScoreManager.singleton.multiplier), spawnTimeBufferMin, initialSpawnTimeBuffer);
			} else {
				spawnTimeBuffer = Mathf.Clamp(
					initialSpawnTimeBuffer - Mathf.Exp (0.1f * ScoreManager.singleton.multiplier - 1.1f),
					spawnTimeBufferMin, initialSpawnTimeBuffer);
			}
		}
	}

	public void SetNewBeltSpeed() {
		if (beltSpeed < beltSpeedMax) {
			beltSpeed = Mathf.Clamp(
				Mathf.Exp(0.25f * ScoreManager.singleton.multiplier + 0.5f), initialBeltSpeed, beltSpeedMax);
		}
	}

	public void ResetSpawnNSpeed() {
		beltSpeed = initialBeltSpeed;
		spawnTimeBuffer = initialSpawnTimeBuffer;
	}
}
