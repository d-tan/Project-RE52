using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour {

	//structure
	Vector3 leftSide = new Vector3(-5.0f, -4.0f, 0f);
	Vector3 rightSide = new Vector3(5.0f, -4.0f, 0f);
	Vector3 top = Vector3.zero;
	Vector3 bottom = new Vector3 (0f, -4.0f, 0f);
//	Vector3 Obottom = new Vector3 ();

	Vector3 touchOrigin = Vector3.zero;

//	Vector2 recycleBinPos = new Vector2 (-4.22f, 0f);
//	Vector2 generalBinPos = new Vector2 (0f, 1.08f);
//	Vector2 organicBinPos = new Vector2 (4.22f, 0f);
//	Vector2 organicRange = new Vector2 (0f, 55f);
//	Vector2 recycleRange = new Vector2 (125f, 180f);
//
	Vector2 rightSideBoundary = new Vector2 (2.5f, 1f);
	Vector2 leftSideBoundary = new Vector2(-2.5f, 1f);

//	Vector2 OorganicRange = new Vector2 (0f, 65f);
//	Vector2 OrecycleRange = new Vector2 (125f, 180f);

//	Vector3 right = new Vector3();
//	Vector3 left = new Vector3();
	float magnitude = 5.0f;

//	float width = 0.0f;
//	float angleScale = 0.0f;

	void Start() {
		SetDetectionAreas ();
//		width = Camera.main.orthographicSize * 2f * Camera.main.aspect;
//		angleScale = organicRange.y / (width / 2.0f - 0.1f);
//		Debug.Log ("scale: " + angleScale);
	}

	void Update() {
		if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				GetPositions ();
				SetDetectionAreas();
			}
		}

		Debug.DrawLine (leftSide, rightSide, Color.red);
		Debug.DrawLine (bottom, top, Color.red);
		Debug.DrawLine (bottom, rightSideBoundary, Color.green);
		Debug.DrawLine (bottom, leftSideBoundary, Color.green);

	}


	private void GetPositions() {
		Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
		touchOrigin = ray.origin;

//		organicRange = OorganicRange;
//		recycleRange = OrecycleRange;
//
		bottom.x = touchOrigin.x;
		top.x = touchOrigin.x;

		float touchAngle = Vector2.Angle (Vector2.right, (Vector2)touchOrigin);
		Debug.Log ("Touch: " + touchAngle + " point: " + (Vector2)touchOrigin);
		float rightAngle = Vector2.Angle (Vector2.right, rightSideBoundary - (Vector2)bottom);
		float leftAngle = Vector2.Angle (Vector2.right, leftSideBoundary - (Vector2)bottom);

		Debug.Log ("right: " + rightAngle + " left: " + leftAngle);

		if (0 <= touchAngle && touchAngle < rightAngle) {
			Debug.Log ("Organic Bin");
		} else if (leftAngle < touchAngle && touchAngle <= 180) {
			Debug.Log ("Recycle Bin");
		} else {
			Debug.Log ("General Bin");
		}
	}



	private void SetDetectionAreas() {
//		float rad = 0.0f;
//		Vector4 simplifiedAngle = CheckAngle (organicRange.y);
//
//		rad = ConvertToRadians (simplifiedAngle.x);
//
//		right = CalculateVector (simplifiedAngle, rad);
////		Debug.Log ("right: " + right);
//
//		simplifiedAngle = CheckAngle (recycleRange.x);
//		rad = ConvertToRadians (simplifiedAngle.x);
//
//		left = CalculateVector (simplifiedAngle, rad);
////		Debug.Log ("left: " + left);


	}

	private float ConvertToRadians(float degrees) {
//		Debug.Log ("degrees: " + degrees + " radians: " + (degrees * (Mathf.PI / 180f)) + " cos: " + Mathf.Cos((degrees * (Mathf.PI / 180f))));
		return degrees * (Mathf.PI / 180f);
	}

//	private Vector4 CheckAngle(float angle) {
//		float givenAngle = angle;
//		float x = 1f;
//		float y = 1f;
//		bool xState = true;
//		float trigState = 1; // -1 = sin, 1 = cos;
//		
//		while (givenAngle > 90.0f) {
//			givenAngle -= 90.0f;
//			if (xState) {
//				x = -1.0f * x;
//			} else {
//				y = -1.0f * y;
//			}
//			xState = !xState;
//			trigState = -1.0f * trigState;
//		}
//
////		Debug.Log ("angle: " + givenAngle + " x: " + x + " y: " + y + " trig: " + trigState + " (1-Cos, -1-Sin)");
//		return new Vector4 (givenAngle, x, y, trigState);
//	}

//	private Vector2 CalculateVector(Vector4 simpleAngle, float rad) {
//		Vector2 finalVector = Vector2.zero;
//		if (simpleAngle.w > 0) {
//			finalVector.x = simpleAngle.y * magnitude * Mathf.Cos (rad);
//			finalVector.y = simpleAngle.z * magnitude * Mathf.Sin (rad) + bottom.y;
////			Debug.Log ("cos: " + Mathf.Cos (rad) + " sin: " + Mathf.Sin (rad));
//		} else {
//			finalVector.x = simpleAngle.y * magnitude * Mathf.Sin (rad);
//			finalVector.y = simpleAngle.z * magnitude * Mathf.Cos (rad) + bottom.y;
//		}
////		if (simpleAngle.w > 0) {
////			finalVector.x = Mathf.Cos (rad);
////			finalVector.y = Mathf.Sin (rad);
////			//			Debug.Log ("cos: " + Mathf.Cos (rad) + " sin: " + Mathf.Sin (rad));
////		} else {
////			finalVector.x = Mathf.Sin (rad);
////			finalVector.y = Mathf.Cos (rad);
////		}
//
//		return finalVector;
//	}
}
