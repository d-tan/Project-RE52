using UnityEngine;
using System.Collections;

public enum RubbishType {
	General,
	Organic,
	Recycle
}

public class RubbishItem : MonoBehaviour {

	public RubbishType myType;
	public float beltSpeed = 0.5f;

	private Collider2D itemCollider;

	void Start() {
		itemCollider = GetComponent<Collider2D> ();

		Debug.Log (itemCollider.bounds.size);
	}

	public RubbishType ThisRubbishType {
		get { 
			return myType;
		}
	}


	void Update() {

	}
		
}
