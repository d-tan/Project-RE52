using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour {

	public Dropdown dropDown;

	public TouchTest2 touch2;
	public TouchTest3 touch3;
	public TouchTest4 touch4;

	private void DisableAll() {
		touch2.enabled = false;
		touch3.enabled = false;
		touch4.enabled = false;
	}

	public void EnableMyScript() {
		DisableAll ();
		switch (dropDown.value) {
		case 0:
			touch2.enabled = true;
			break;
		case 1:
			touch3.enabled = true;
			break;
		case 2:
			touch4.enabled = true;
			break;
		}
	}
}
