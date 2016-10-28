using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public static ScoreManager singleton;

	public Text genRubScoreText;
	public Text orgRubScoreText;
	public Text recRubScoreText;
	public Text multiplierText;
	public Slider countDownSlider;
	public Text itemCountDownText;

	private int genRubScore = 0;
	private int orgRubScore = 0;
	private int recRubScore = 0;

	private int multiplier = 1; // needs reset vv
	private float multiplierCountDown = 5.0f;
	private bool isMuliplying = false;
	private int itemsToNextLevel = 0;
	private int currentItemStreak = 0;
	private int accumulatedItemCount = 0; // accumlated items need for next level

	private int streakToStartMultiplier = 5;
	private int baseItemCount = 7;

	// Use this for initialization
	void Awake () {
		if (singleton == null) {
			singleton = this;
		}
	}

	void Start() {
		// Disable multiplier UI
		ToggleMultiplierUI(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (isMuliplying) {
			countDownSlider.value -= Time.deltaTime / multiplierCountDown;
		}
	}

	public void AddMinusScore (RubbishType type, int sign) {
		CheckMultiplierAvaliable (sign);
		switch (type) {
		case RubbishType.General:
			genRubScore += 1 * (int)Mathf.Sign (sign) * multiplier;
			genRubScoreText.text = genRubScore.ToString();
			break;

		case RubbishType.Organic:
			orgRubScore += 1 * (int)Mathf.Sign (sign) * multiplier;
			orgRubScoreText.text = orgRubScore.ToString();
			break;

		case RubbishType.Recycle:
			recRubScore += 1 * (int)Mathf.Sign (sign) * multiplier;
			recRubScoreText.text = recRubScore.ToString();
			break;

		default:
			Debug.Log ("RubbishType unknown: " + type);
			break;
		}
	}

	// Toggle Multipler UI on/off based on given state
	private void ToggleMultiplierUI(bool state) {
		multiplierText.gameObject.SetActive (state);
		countDownSlider.gameObject.SetActive (state);
		itemCountDownText.gameObject.SetActive (state);
	}

	private void ChangeUIColor(Color colorChange) {
		multiplierText.color = colorChange;
		itemCountDownText.color = colorChange;
	}

	/// <summary>
	/// Check if multiplier is avaliable to initiate 
	/// </summary>
	/// <param name="sign">Given sign detects if player incorrectly scores</param>
	private void CheckMultiplierAvaliable (int sign) {
		if (sign < 0) {
			ResetMultiplier ();
		} else {
			currentItemStreak++;
			if (isMuliplying) {
				itemsToNextLevel--;
				itemCountDownText.text = itemsToNextLevel.ToString ();

				if (itemsToNextLevel <= 0) {
					IncreaseMultiplier ();
				}
			}

			if (currentItemStreak == streakToStartMultiplier) {
				StartMultiplier ();
			}
		}
	}

	// Increases the multiplier and associated variables
	private void IncreaseMultiplier() {
		multiplier++;
		multiplierText.text = "x" + multiplier.ToString ();
		SetItemsToNextMultiplier ();
		itemCountDownText.text = itemsToNextLevel.ToString ();
	}

	private void SetItemsToNextMultiplier () {
		accumulatedItemCount += Mathf.RoundToInt(Mathf.Sqrt (multiplier + baseItemCount) / 1.5f);
		itemsToNextLevel = baseItemCount + accumulatedItemCount;
	}

	private void StartMultiplier() {
		isMuliplying = true; // Start multiplier
		multiplier++;
		multiplierText.text = "x" + multiplier.ToString ();
		ToggleMultiplierUI (true);
		SetItemsToNextMultiplier ();
		itemCountDownText.text = itemsToNextLevel.ToString ();
	}

	// Resets multiplier and associated variables
	private void ResetMultiplier() {
		currentItemStreak = 0;
		multiplier = 1;
		accumulatedItemCount = 0;
		multiplierCountDown = 5.0f;
		StartCoroutine (MultiplierUITurnOff ());
		isMuliplying = false;
	}

	// Timer to turn off Multiplier UI. So the UI is not disabled instantly
	IEnumerator MultiplierUITurnOff() {
		ChangeUIColor (Color.red);
		yield return new WaitForSeconds (3.0f);
		ChangeUIColor (Color.black);
		itemsToNextLevel = 0;
		itemCountDownText.text = itemsToNextLevel.ToString ();
		ToggleMultiplierUI (false);
		countDownSlider.value = 1.0f;
		StopCoroutine (MultiplierUITurnOff ());
	}

}
