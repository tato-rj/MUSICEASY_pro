using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using AppAdvisory.SharingSystem;

public class NotesResultsController : MonoBehaviour {

	public Text accuracy;
	public Text correctNotes;
	public Text wrongNotes;
	public Text average;
	public Text score;
	public GameObject message;
	public GameObject details;
	//public GameObject buttons;
	public GameObject[] stars;
	public GameObject[] greyStars;
	public GameObject[] yellowStars;
	public GameObject gameOver;
	public GameObject graphPanel;
	private bool showScore;

	void Awake () {

		showScore = true;
		float averageTime = (float)InputClick.instance.timeCountArray.Average ();
		int percentage = Mathf.RoundToInt(NotesScoreController.instance.percentage);
		int scoreNum = NotesScoreController.instance.scoreCount;
		int totalScore = Mathf.RoundToInt (((scoreNum * percentage) / averageTime));

		if (percentage == 0) {
			showScore = false;
		} else if (percentage > 0 && percentage < 50) {
			if (averageTime > 0 && averageTime < 2f) {
				//1
				message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback1");
			} else if (averageTime >= 2f && averageTime < 3f) {
				//2
				message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback2");
			} else if (averageTime >= 3f) {
				//3
				message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback3");
			}			
		} else if (percentage >= 50 && percentage < 80) {

			if (averageTime > 0 && averageTime < 1.5f) {
				//5
				message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback5");
			} else if (averageTime >= 1.5f && averageTime < 2.2f) {
				//4
				message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback4");
			} else if (averageTime >= 2.2f && averageTime < 3f) {
				//6
				message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback6");
			} else if (averageTime >= 3f && averageTime < 5f) {
				//8
				message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback8");
			} else if (averageTime >= 5f) {
				//7
				message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback7");
			}			
		} else if (percentage >=80) {

			if (averageTime > 0 && averageTime < 1.5f) {
				//13
				message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback13");
			} else if (averageTime >= 1.5f && averageTime < 2f) {
				//12
				message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback12");
			} else if (averageTime >= 2f && averageTime < 3f) {
				//11
				message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback11");
			} else if (averageTime >= 3f && averageTime < 5f) {
				//10
				message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback10");			
				//9
			} else if (averageTime >= 5f) {
				message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback9");	
			}			
		}


		if (showScore) {
			GetComponent<AudioSource> ().Play ();
			accuracy.text = NotesScoreController.instance.percentage + "%";
			correctNotes.text = NotesScoreController.instance.scoreCount.ToString();
			wrongNotes.text = (NotesScoreController.instance.cardsPlayed - NotesScoreController.instance.scoreCount).ToString ();
			average.text = averageTime.ToString("F1") + "s";
			score.text = LocalizationManager.instance.GetLocalizedValue ("totalScore") + " " + totalScore.ToString();
			SetupStars ();
		} else {
			gameOver.SetActive (true);
		}

		NotesGameController.instance.SaveRecords (percentage, totalScore);
	}

	void Start () {

		if (showScore) {
			NotesGameController.instance.LoadRecords ();
			StartCoroutine (AnimateStars ());
		}

	}

	void SetupStars () {
		
		if (NotesScoreController.instance.percentage > 0f) {
			greyStars [0].SetActive (false);
			yellowStars [0].SetActive (true);
		}

		if (NotesScoreController.instance.percentage > 70f) {
			greyStars [1].SetActive (false);
			yellowStars [1].SetActive (true);
		}

		if (NotesScoreController.instance.percentage == 100f) {
			//LevelManager.instance.requestFeedbackNow = true;
			greyStars [2].SetActive (false);
			yellowStars [2].SetActive (true);
		}

	}

	public IEnumerator AnimateStars () {

		for (int i=0; i<stars.Length; i++) {

			stars [i].SetActive (true);
			yield return new WaitForSeconds (0.2f);

		}

		yield return new WaitForSeconds (0.2f);

		message.SetActive (true);

	}

	public void BackToMainScreen () {
		FadeScene.instance.GoToMainScreen ();
	}

	public void BackToNotesGame () {
		FadeScene.instance.GoToNotesGame ();
	}

	public void RateApp () {
		Application.OpenURL ("itms-apps://itunes.apple.com/app/id1212142417"); 
	}

	public void ToggleGraph () {
		graphPanel.SetActive(!graphPanel.activeSelf);
	}

	public void ShareResults () {
		StartCoroutine (ShareSequence());
	}

	public IEnumerator ShareSequence() {
		VSSHARE.DOTakeScreenShot ();

		yield return new WaitForSeconds (0.5f);

		VSSHARE.DOOnclickedOnIconScreenshot ();
	}
		
}
