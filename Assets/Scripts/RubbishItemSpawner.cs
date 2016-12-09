using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// MUST be attached to conveyor belt object.
public class RubbishItemSpawner : MonoBehaviour {

//	public static RubbishItemSpawner singleton;

	public GameObject parentSpawnObject;
	public GameObject shellItem;

	private float beltSpeed = 2.0f; // Belt Speed details
	private float initialBeltSpeed;
	private float beltSpeedMax = 8.0f;

	private float spawnTimeBuffer = 0.35f; // Spawn Time Buffer details
	private float initialSpawnTimeBuffer;
	private float spawnTimeBufferMin = -0.4f;

	private bool thresholdHasReached = false; // Spawning details
	private bool isSpawning = true;
	private bool spawnItemRoutineActive = false;

	// super-rare = 0.5% | rare = 2% | uncommon = 10.5% | common = 85%     accumulative
	private Vector4 rarityThresholds = new Vector4 (1.0f, 0.13f, 0.025f, 0.005f); // (common, uncommon, rare, super-rare)

	Vector3 beltSize;
	Vector3 beltPos;
	Collider2D beltCollider;

	ItemDatabase itemDatabase;
	ResourceDatabase resourceDatabase;
	ScoreManager scoreManager;
//	GameUIManager UIManager;

	// Use this for initialization
	void Start () {
		beltCollider = GetComponent<Collider2D> ();

		beltPos = this.transform.position;
		beltSize = beltCollider.bounds.extents;

		StartCoroutine (SpawnItems());

		initialBeltSpeed = beltSpeed; // hold the initial values
		initialSpawnTimeBuffer = spawnTimeBuffer;

		GameObject databases = GameObject.FindGameObjectWithTag ("Databases");
		GameObject managers = GameObject.FindGameObjectWithTag ("GameController");

		itemDatabase = databases.GetComponent<ItemDatabase> ();
		resourceDatabase = databases.GetComponent<ResourceDatabase> ();
//		UIManager = managers.GetComponent<GameUIManager> ();
		scoreManager = managers.GetComponent<ScoreManager> ();
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

			InstantiateItem (new Vector3 (spawnPosX, spawnPosY, beltPos.z));
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

	private void InstantiateItem (Vector3 spawnPos) {
		GameObject spawnedObject = Instantiate (
			shellItem, 
			spawnPos, 
			Quaternion.identity, 
			parentSpawnObject.transform
		) as GameObject;

		// Get Item info from database

		Item item = itemDatabase.FetchItemByID (PickRandomItem());
		spawnedObject.GetComponent<RubbishItem> ().MyRubbishItemID = item.ID; // set RubbishItem class itemId

		RubbishType[] rubbishTypes = new RubbishType[2]; // get rubbish types from item class
		// Get resource from resouces database by ID, then retrieve its rubbish type
		rubbishTypes [0] = resourceDatabase.FetchResourceByID((int)item.Resource1).RubbishType;
		rubbishTypes [1] = resourceDatabase.FetchResourceByID((int)item.Resource2).RubbishType;

		ResourceType[] resourceTypes = new ResourceType[2];
		resourceTypes [0] = item.Resource1;
		resourceTypes [0] = item.Resource2;

		spawnedObject.GetComponent<SpriteRenderer> ().sprite = item.Sprite; // set sprite
		RubbishItem spawnedObjectScript = spawnedObject.GetComponent<RubbishItem> ();
		spawnedObjectScript.MyRubbishTypes = rubbishTypes; // set rubbish types
		spawnedObjectScript.MyResourceTypes = resourceTypes; // set resource types
		PolygonCollider2D spawnedObjectCollider = spawnedObject.AddComponent<PolygonCollider2D> (); // give polygon collider

		// Move the Item if it's on the edge
		spawnedObject.transform.position = new Vector3 (Mathf.Clamp (
			spawnedObject.transform.position.x,
			(beltPos.x - beltSize.x) + spawnedObjectCollider.bounds.size.x,
			(beltPos.x + beltSize.x) - spawnedObjectCollider.bounds.size.x),
			spawnedObject.transform.position.y,
			spawnedObject.transform.position.z
		);
	}

	// Algorithm to pick an item
	private int PickRandomItem() {
		int chosenItemID = 0; // variable to store item ID
		float randomNum = Random.value; // Pick random value in [0,1] < inclusive

		// Compare random value to rarity thresholds
		if (randomNum <= rarityThresholds.w) { // Super-Rare
			Debug.Log("SUPER-RARE " + randomNum);
			chosenItemID = itemDatabase.PickRandomItem (ItemRarity.SuperRare);

		} else if (randomNum <= rarityThresholds.z) { // Rare
			chosenItemID = itemDatabase.PickRandomItem (ItemRarity.Rare);

		} else if (randomNum <= rarityThresholds.y) { // Uncommon
			chosenItemID = itemDatabase.PickRandomItem (ItemRarity.Uncommon);

		} else { // Common
			chosenItemID = itemDatabase.PickRandomItem (ItemRarity.Common);
		}

		return chosenItemID;
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
					if (rb.velocity.y < velocityY) {
						rb.velocity = new Vector2 (0.0f, velocityY);
					} else {
						rb.velocity = Vector2.zero;
					}
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
			if (scoreManager.multiplier < 4) {
				spawnTimeBuffer = Mathf.Clamp (
					Mathf.Exp(-scoreManager.multiplier), spawnTimeBufferMin, initialSpawnTimeBuffer);
			} else {
				spawnTimeBuffer = Mathf.Clamp(
					initialSpawnTimeBuffer - Mathf.Exp (0.1f * scoreManager.multiplier - 1.1f),
					spawnTimeBufferMin, initialSpawnTimeBuffer);
			}
		}
	}

	public void SetNewBeltSpeed() {
		if (beltSpeed < beltSpeedMax) {
			beltSpeed = Mathf.Clamp(
				Mathf.Exp(0.25f * scoreManager.multiplier + 0.5f), initialBeltSpeed, beltSpeedMax);
		}
	}

	public void ResetSpawnNSpeed() {
		beltSpeed = initialBeltSpeed;
		spawnTimeBuffer = initialSpawnTimeBuffer;
	}
}
