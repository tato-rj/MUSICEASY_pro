using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RhythmsScoreController : MonoBehaviour {

	public static RhythmsScoreController instance;

	public Text missedNotesText;
	public Text percentageText;
	public int missedNotes = 0;
	public float percentage = 0;
	public int notesPassed = 0;
	public int correctNotes = 0;
	public int wrongNotes = 0;

	void Awake () {

		instance = this;

	}

	void FixedUpdate () {
		missedNotesText.text = missedNotes.ToString ();
	}

	public void UpdateCorrectNotesCount () {
	
		correctNotes++;
		//scoreText.text = correctNotes.ToString ();


	}

	public void UpdateWrongNotesCount () {

		wrongNotes++;

	}

	public void UpdateAccuracy () {

		missedNotes = notesPassed - correctNotes;
		int tapsNum = correctNotes + wrongNotes;


		if (tapsNum > 0) {
			percentage = correctNotes * 100 / (tapsNum + missedNotes);
			percentageText.text = percentage.ToString() + "%";

		}
	}

	public void ResetAllScores () {

		correctNotes = 0;
		missedNotesText.text = correctNotes.ToString ();
		wrongNotes = 0;
		missedNotes = 0;
		percentage = 0;
		percentageText.text = "-";
		notesPassed = 0;

	}

}
