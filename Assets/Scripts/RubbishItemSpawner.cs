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
	Collider beltCollider;

	// Use this for initialization
	void Start () {
		beltCollider = GetComponent<Collider> ();

		beltPos = this.transform.position;
		beltSize = beltCollider.bounds.size;

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
			float spawnPosX = Random.Range (beltPos.x - beltSize.x / 2, beltPos.x + beltSize.x / 2);
			float spawnPosY = beltCollider.transform.position.y - beltSize.y/2;

//			Debug.Log ("positions: " + new Vector2(spawnPosX, spawnPosY));

			// Pick random Item
			int index = (int)Mathf.Clamp(Mathf.Floor(Random.Range (0.0f, items.Length)), 0.0f, items.Length - 1);

			GameObject spawnedObject = Instantiate (
				items[index], 
				new Vector3 (spawnPosX, spawnPosY, beltPos.z - 0.3f), 
				Quaternion.identity, 
				parentObject.transform
			) as GameObject;
		}
	}


	void OnTriggerEnter (Collider other){
		AddVelocityToItem (other, beltSpeed);
	}

	void OnTriggerExit(Collider other) {
		AddVelocityToItem (other, -beltSpeed);
	}

	void AddVelocityToItem(Collider other, float velocityY) {
		if (other.CompareTag ("PickUpable")) {
			Rigidbody rb = other.GetComponent<Rigidbody> ();

			if (rb) {
				rb.velocity += new Vector3 (0.0f, velocityY, 0.0f);
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
