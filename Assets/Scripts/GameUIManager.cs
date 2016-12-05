using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {

	public Image[] indicators = new Image[2];
	private Color[] indicatorColors = new Color[4];


	void Start() {
		indicatorColors [0] = new Color (1.0f, 103.0f / 255.0f, 103.0f / 255.0f, 160.0f / 255.0f);
		indicatorColors [1] = new Color (54.0f / 255.0f, 188.0f / 255.0f, 71.0f / 255.0f, 160.0f / 255.0f);
		indicatorColors [2] = new Color (241.0f / 255.0f, 244.0f / 255.0f, 48.0f / 255.0f, 160.0f / 255.0f);
		indicatorColors [3] = new Color (184.0f / 255.0f, 184.0f / 255.0f, 184.0f / 255.0f, 160.0f / 255.0f);

	}


	public void ChangeIndicatorColors(bool active, RubbishType type1 = RubbishType.General, RubbishType type2 = RubbishType.General) {
		
		// Check if indicators are active
		if (active) {
			switch (type1) {
			case RubbishType.Organic:
				indicators [0].color = indicatorColors [(int)type2];
				indicators [1].color = indicatorColors [(int)type1];
				break;
			case RubbishType.Recycle:
				indicators [0].color = indicatorColors [(int)type1];
				indicators [1].color = indicatorColors [(int)type2];
				break;
			default:
				indicators [0].color = indicatorColors [(int)type1];
				indicators [1].color = indicatorColors [(int)type2];
				break;
			}
		} else {
			// Set them to Grey
			for (int i = 0; i < indicators.Length; i++) {
				indicators [i].color = indicatorColors [3];
			}
		}

	}
}
