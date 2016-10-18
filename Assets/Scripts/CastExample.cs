using UnityEngine;
using System.Collections;

public class Cast : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update()
	{
		//If the left mouse button is clicked.
		if (Input.GetMouseButtonDown(1))
		{
			Debug.Log ("Click!");
			//Get the mouse position on the screen and send a raycast into the game world from that position.
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint,Vector2.zero);

			//If something was hit, the RaycastHit2D.collider will not be null.
			if ( hit.collider != null )
			{
				Debug.Log( hit.collider.name );
			}
		}
	}
}
