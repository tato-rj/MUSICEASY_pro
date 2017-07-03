using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InputClick : MonoBehaviour {

	public static InputClick instance;

	public float timeCount = 0;
	public int correctCount = 0;
	public int comboNum = 0;
	public List<int> timeCountArray = new List<int>();
	public GameObject correctAnswer;
	public GameObject wrongAnswer;
	public Toggle musicName;
	public GameObject[] buttons;

	void Awake () {

		instance = this;

	}

	void FixedUpdate () {
	
		if (NotesGameController.instance.gameOn) {
			timeCount += Time.deltaTime;
			if (timeCount > 5f && timeCount < 5.1f) {
				string noteName = GameObject.Find ("Whole(Clone)").tag;
				GameObject.Find (noteName).GetComponent<Animation>().Play();
			}			
		}
	}

	public void OnTap (Button button) {

		UpdateTimeCounter ();

		if (NotesGameController.instance.timeRemaining > 0) {

			GameObject note = GameObject.Find ("Whole(Clone)");

			NotesScoreController.instance.cardsPlayed++;

			//If answer is correct...
			if (button.name == note.tag) {
				
				CorrectAnswer ();

			//If answer is NOT correct...
			} else {

				WrongAnswer ();

			}

			NotesGameController.instance.CreateNewNote ();

		}		
	}

	public void UpdateTimeCounter () {
		timeCountArray.Add (Mathf.RoundToInt (timeCount));

		timeCount = 0f;
	}

	public void CorrectAnswer () {
		NotesScoreController.instance.IncrementScore ();
		NotesScoreController.instance.UpdateAccuracy ();
		StartCoroutine (AnswerIcon(correctAnswer));

		//Keep track of a combo
		if (correctCount == 5) {
			comboNum++;
			correctCount = 0;
			//print ("COMBO!");
		} else {
			correctCount++;
		}
	}

	public void WrongAnswer () {
		Handheld.Vibrate ();
		NotesScoreController.instance.UpdateAccuracy ();
		StartCoroutine (AnswerIcon(wrongAnswer));
		correctCount = 0;
	}
		
	public IEnumerator AnswerIcon (GameObject icon) {
	
		icon.SetActive (true);
	
		yield return new WaitForSeconds (0.5f);

		icon.SetActive (false);

	}

	public void ToggleMusicName () {
		
		if (musicName.isOn) {
			foreach (GameObject letterName in GameObject.FindGameObjectsWithTag("Letter names") ) {
				letterName.GetComponent<Text> ().enabled = true;
			}
			foreach (GameObject noteName in GameObject.FindGameObjectsWithTag("Note names") ) {
				noteName.GetComponent<Text> ().enabled = false;
			}

		} else {
			foreach (GameObject letterName in GameObject.FindGameObjectsWithTag("Letter names") ) {
				letterName.GetComponent<Text> ().enabled = false;
			}
			foreach (GameObject noteName in GameObject.FindGameObjectsWithTag("Note names") ) {
				noteName.GetComponent<Text> ().enabled = true;
			}
		}
	}

}
