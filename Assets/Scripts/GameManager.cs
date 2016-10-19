using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager current;


	// Use this for initialization
	void Start () {
		if (current == null) {
			current = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
