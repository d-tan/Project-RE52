using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// MUST be attached to conveyor belt object.
public class RubbishItemSpawner : MonoBehaviour {

	public GameObject[] items;
	public GameObject parentObject;
	public bool isSpawning = true;
	float beltSpeed = 2.0f;
	float spawnTimebuffer = 0.3f;
	public Text beltText;
	public Text spawnText;

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
	
	}

	IEnumerator SpawnItems() {
		yield return new WaitForSeconds (3.0f);
		
		while (isSpawning) {
			yield return new WaitForSeconds (Random.value + spawnTimebuffer);

			// pick a random location
			float spawnPosX = Random.Range ((beltPos.x - beltSize.x), (beltPos.x + beltSize.x));
			float spawnPosY = beltCollider.transform.position.y - beltSize.y/2;

			// Pick random Item
			int index = (int)Mathf.Clamp(Mathf.Floor(Random.Range (0.0f, items.Length)), 0.0f, items.Length - 1);

			GameObject spawnedObject = Instantiate (
				items[index], 
				new Vector3 (spawnPosX, spawnPosY, beltPos.z), 
				Quaternion.identity, 
				parentObject.transform
			) as GameObject;

			Collider2D spawnedObjectCollider = spawnedObject.GetComponent<Collider2D> ();
			spawnedObject.transform.position = new Vector3 (Mathf.Clamp (
				spawnedObject.transform.position.x,
				(beltPos.x - beltSize.x) + spawnedObjectCollider.bounds.size.x,
				(beltPos.x + beltSize.x) - spawnedObjectCollider.bounds.size.x),
				spawnedObject.transform.position.y,
				spawnedObject.transform.position.z
			);
		}
	}


	void OnTriggerEnter2D (Collider2D other){
		AddVelocityToItem (other, beltSpeed);
	}

	void OnTriggerExit2D (Collider2D other) {
		AddVelocityToItem (other, -beltSpeed);
	}

	void AddVelocityToItem(Collider2D other, float velocityY) {
		if (other.CompareTag ("PickUpable")) {
			Rigidbody2D rb = other.GetComponent<Rigidbody2D> ();

			if (rb) {
				rb.velocity += new Vector2 (0.0f, velocityY);
			} else {
				Debug.Log ("This item, " + other.ToString() + ", does not have a rigidbody");
			}
		}
	}


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
		isSpawning = !isSpawning;
	}
}
