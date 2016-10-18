using UnityEngine;
using System.Collections;

public enum RubbishType {
	General,
	Organic,
	Recycle
}

public class RubbishItem : MonoBehaviour {

	public RubbishType myType;

	void Start() {
		
	}

	public RubbishType ThisRubbishType {
		get { 
			return myType;
		}
	}


	void Update() {

	}


	private void PickUp() {
		
	}
}
