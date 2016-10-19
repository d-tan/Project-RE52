using UnityEngine;
using System.Collections;

public class DragingScript : MonoBehaviour
{
	//This code is for 2D click/drag gameobject
	//Please make sure to change Camera Projection to Orthographic
	//Add Collider (not 2DCollider) to gameObject  

	public GameObject gameObjectTodrag; //refer to GO that being dragged

	public Vector3 GOcenter; //gameobjectcenter
	public Vector3 touchPosition; //touch or click position
	public Vector3 offset;//vector between touchpoint/mouseclick to object center
	public Vector3 newGOCenter; //new center of gameObject

	RaycastHit hit; //store hit object information

	public bool draggingMode = false;

	private float timer = 0.0f;
	private float minflickTime = 0.5f;
	private float minflickDist = 1.5f;
	private float flickTimeMultiplier = 0.5f + 3.5f;

	private Vector3 flickOrigin = Vector3.zero;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

		//***********************
		// *** CLICK TO DRAG ****
		//***********************

		#if UNITY_EDITOR
		//first frame when user click left mouse
		if (Input.GetMouseButtonDown (0)) {
			//convert mouse click position to a ray
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			//if ray hit a Collider ( not 2DCollider)
			if (Physics.Raycast (ray, out hit)) {
				gameObjectTodrag = hit.collider.gameObject;
				GOcenter = gameObjectTodrag.transform.position;
				touchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				offset = touchPosition - GOcenter;
				draggingMode = true;
			}
		}

		//every frame when user hold on left mouse
		if (Input.GetMouseButton (0)) {
			if (draggingMode) {
				touchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				newGOCenter = touchPosition - offset;
				gameObjectTodrag.transform.position = new Vector3 (newGOCenter.x, newGOCenter.y, GOcenter.z);
			}
		}

		//when mouse is released
		if (Input.GetMouseButtonUp (0)) {
			draggingMode = false;
		}
		#endif

		//***********************
		// *** TOUCH TO DRAG ****
		//***********************
//		foreach (Touch touch in Input.touches)
//		{
		if (Input.touchCount > 0) {
			switch (Input.GetTouch (0).phase) {
			//When just touch
			case TouchPhase.Began:
				//convert mouse click position to a ray
				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);

				//if ray hit a Collider ( not 2DCollider)
				// if (Physics.Raycast(ray, out hit))
				if (Physics.SphereCast (ray, 0.3f, out hit)) {
					if (hit.transform.tag == "PickUpable") {
						gameObjectTodrag = hit.collider.gameObject;
						GOcenter = gameObjectTodrag.transform.position;
						touchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						offset = touchPosition - GOcenter;
						draggingMode = true;

						flickOrigin = touchPosition;

						timer = 0.0f;

						SetRBVelZero (gameObjectTodrag);
					}
				}
				break;

			case TouchPhase.Moved:
				if (draggingMode) {
					touchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					newGOCenter = touchPosition - offset;
					gameObjectTodrag.transform.position = new Vector3 (newGOCenter.x, newGOCenter.y, GOcenter.z);
				}
				Debug.Log (timer);
				break;

			case TouchPhase.Ended:
				draggingMode = false;

				if (timer < minflickTime) {
					Debug.Log ("time: " + timer + " minTime: " + minflickTime);
					float timeTaken = timer;
					Vector3 currentTouchPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					if (Vector3.Distance (flickOrigin, currentTouchPos) > minflickDist) {
						Debug.Log (Vector3.Distance (flickOrigin, currentTouchPos));
						Rigidbody rb = gameObjectTodrag.GetComponent<Rigidbody> ();

						Vector3 heading = currentTouchPos - flickOrigin;
						Debug.Log ("Heading is: " + heading);
						if (rb != null) {
							rb.velocity += heading * flickTimeMultiplier/Mathf.Clamp(timeTaken, 0.7f, flickTimeMultiplier);
						} else {
							Debug.Log ("Pick up object has not rigidbody");
						}
					}
				}
				break;
			}
		}
		timer += Time.deltaTime;
	}
//		}

	void SetRBVelZero (GameObject givenObject) {
		Rigidbody rb = givenObject.GetComponent<Rigidbody> ();

		if (rb) {
			rb.velocity = Vector3.zero;
		} else {
			Debug.Log ("Given Object has no Rigidbody");
		}
	}

}
