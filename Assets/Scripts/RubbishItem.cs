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

	private Rigidbody itemRB;

	void Start() {
		itemRB = GetComponent<Rigidbody> ();
	}

	public RubbishType ThisRubbishType {
		get { 
			return myType;
		}
	}


	void Update() {

	}
		
}
