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

	public int multiplier = 1; // needs reset vv
	public float multiplierTimer = 5.0f;
	public float currentMulitplierTimer;
	private bool isMuliplying = false;
	private int itemsToNextLevel = 0;
	private int currentItemStreak = 0;
	private int accumulatedItemCount = 0; // accumlated items need for next level
	private float itemTimeValue = 0.75f; // the value of each item that is added to the timer

	private int streakToStartMultiplier = 5; // Player need to get streak in limited time to start multiplier
	private float startMultiplierTimer = 0.0f;
	private float startMultiplierTime = 3.0f;

//	private int baseItemCount = 7;
	private float initialTimerStart = 0.65f; // percentage the slider timer starts at
	private float minItemTimeValue = 0.15f;

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
			countDownSlider.value = Mathf.Clamp (
				countDownSlider.value - Time.deltaTime / multiplierTimer, 
				countDownSlider.minValue, 
				countDownSlider.maxValue);

			if (countDownSlider.value == countDownSlider.minValue) {
				Color orangeColor = new Vector4 (1.0f, 0.65f, 0.0f, 1.0f);
				StopNResetMultiplier (orangeColor);
			}
		} else {
			startMultiplierTimer += Time.deltaTime;
			if (startMultiplierTimer >= startMultiplierTime) {
				currentItemStreak = 0;
				startMultiplierTimer = 0.0f;
			}
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
//		itemCountDownText.gameObject.SetActive (state);
	}

	/// <summary>
	/// Check if multiplier is avaliable to initiate or good to continue multiplier.
	/// </summary>
	/// <param name="sign">Given sign detects if player incorrectly scores</param>
	private void CheckMultiplierAvaliable (int sign) {
		// Check if player has sorted item correctly
		if (sign < 0) {
			// Incorrectly sorted
			StopNResetMultiplier (Color.red);

		} else {
			// Correctly sorted
			currentItemStreak++; // increase streak

			if (isMuliplying) {
				AddNCheckSliderTime ();
//				itemsToNextLevel--;
//				itemCountDownText.text = itemsToNextLevel.ToString ();
//				countDownSlider.value = countDownSlider.maxValue;
//				if (itemsToNextLevel <= 0) {
//					IncreaseMultiplier ();
//				}
			} else {
				// reset start multiplier timer
				startMultiplierTimer = 0.0f;
			}

			// check if player can start multiplier 
			if (startMultiplierTimer <= startMultiplierTime && currentItemStreak == streakToStartMultiplier) {
				StartMultiplier ();
			}
		}
	}

	// Increases the multiplier and associated variables
	private void IncreaseMultiplier() {
		StartCoroutine (MultiplierUIFlashColor ());

		multiplier++;
		multiplierText.text = "x" + multiplier.ToString ();

//		SetItemsToNextMultiplier ();
		SetNextSliderCoolDown ();
		countDownSlider.value = countDownSlider.maxValue * initialTimerStart;

		SetItemTimeValue ();

		RubbishItemSpawner.singleton.SetNewBeltSpeed ();
		RubbishItemSpawner.singleton.ChangeSpawnTime ();
	}

//	private void SetItemsToNextMultiplier () {
//		accumulatedItemCount += Mathf.RoundToInt(Mathf.Sqrt (multiplier + baseItemCount) / 1.5f);
//		itemsToNextLevel = baseItemCount + accumulatedItemCount;
//		itemCountDownText.text = itemsToNextLevel.ToString ();
//	}

	// Adds time to the slider timer and checks if next multiplier has been achieved
	private void AddNCheckSliderTime() {
		countDownSlider.value += itemTimeValue/currentMulitplierTimer;
		if (countDownSlider.value >= countDownSlider.maxValue) {
			IncreaseMultiplier ();
		}
	}

	private void SetItemTimeValue() {
		itemTimeValue = Mathf.Clamp(Mathf.Exp (-0.15f * multiplier + 0.1f), minItemTimeValue, 5.0f);
	}

	// Sets the cool down slider for the next level (i.e. reduces the time taken to countdown)
	private void SetNextSliderCoolDown() {
		multiplierTimer -= 3 * Mathf.Exp (-0.6f * multiplier);
		currentMulitplierTimer = multiplierTimer;
	}

	private void StartMultiplier() {
		isMuliplying = true; // Start multiplier
		multiplier++;
		multiplierText.text = "x" + multiplier.ToString ();
		ToggleMultiplierUI (true);
		countDownSlider.value = countDownSlider.maxValue * initialTimerStart;

		SetItemTimeValue ();
//		SetItemsToNextMultiplier ();
	}


	// Resets multiplier and associated variables
	private void StopNResetMultiplier(Color multiplierStopColor) {
		currentItemStreak = 0;
		multiplier = 1;
		accumulatedItemCount = 0;
		multiplierTimer = 5.0f;
		StartCoroutine (MultiplierUITurnOff (multiplierStopColor));
		isMuliplying = false;
		RubbishItemSpawner.singleton.ResetSpawnNSpeed ();
	}

	// Timer to turn off Multiplier UI. So the UI is not disabled instantly
	IEnumerator MultiplierUITurnOff(Color multiplierStopColor) {
		StopCoroutine (MultiplierUIFlashColor ());
		ChangeUIColor (multiplierStopColor);
		yield return new WaitForSeconds (2.0f);
		ChangeUIColor (Color.black); // Change back to default color
		itemsToNextLevel = 0;
		itemCountDownText.text = itemsToNextLevel.ToString ();
		ToggleMultiplierUI (false);
		countDownSlider.value = initialTimerStart;
	}

	IEnumerator MultiplierUIFlashColor () {
		ChangeUIColor (Color.green);
		yield return new WaitForSeconds (0.1f);
		ChangeUIColor (Color.black);
		yield return new WaitForSeconds (0.1f);
		ChangeUIColor (Color.green);
		yield return new WaitForSeconds (0.5f);
		ChangeUIColor (Color.black);
	}


	private void ChangeUIColor(Color colorChange) {
		multiplierText.color = colorChange;
		itemCountDownText.color = colorChange;
	}
}
