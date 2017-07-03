using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class RhythmsResultsController : MonoBehaviour {

	public Text score;
	public Text correctNotes;
	public Text wrongNotes;
	public Text missedNotes;
	public GameObject shiningStar;
	public GameObject[] stars;
	public GameObject[] greyStars;
	public GameObject[] yellowStars;
	public GameObject message;
	public GameObject graphPanel;

	void Awake () {

		Screen.orientation = ScreenOrientation.LandscapeLeft;

		int percentage = Mathf.RoundToInt(RhythmsScoreController.instance.percentage);
		int totalScore = RhythmsScoreController.instance.correctNotes * percentage;

		correctNotes.text = RhythmsScoreController.instance.correctNotes.ToString();
		wrongNotes.text = RhythmsScoreController.instance.wrongNotes.ToString ();
		missedNotes.text = RhythmsScoreController.instance.missedNotes.ToString();
		score.text = totalScore.ToString ();

		SetupStars ();

		if (percentage < 10) {
			message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback1");
		} else if (percentage >= 10 && percentage < 20) {
			message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback3");
		} else if (percentage >= 20 && percentage < 30) {
			message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback4");
		} else if (percentage >= 30 && percentage < 40) {
			message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback5");		
		} else if (percentage >= 40 && percentage < 50) {
			message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback6");		
		} else if (percentage >= 50 && percentage < 60) {
			message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback7");		
		} else if (percentage >= 60 && percentage < 70) {
			message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback8");		
		} else if (percentage >= 70 && percentage < 80) {
			message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback9");		
		} else if (percentage >= 80 && percentage < 90) {
			message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback10");			
		} else if (percentage >= 90 && percentage < 100) {
			message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback12");		
		} else if (percentage == 100) {
			message.GetComponent<Text> ().text = LocalizationManager.instance.GetLocalizedValue ("feedback13");			
		}

		RhythmsGameController.instance.SaveRecords (percentage, totalScore);

	}

	void Start () {
		//shiningStar.SetActive (true);
		StartCoroutine (AnimateStars());
	}

	void SetupStars () {

		if (RhythmsScoreController.instance.percentage > 0f) {
			greyStars [0].SetActive (false);
			yellowStars [0].SetActive (true);
		}

		if (RhythmsScoreController.instance.percentage > 50f) {
			greyStars [1].SetActive (false);
			yellowStars [1].SetActive (true);
		}

		if (RhythmsScoreController.instance.percentage == 100f) {
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

	public void BackToRhythmsGame () {
		FadeScene.instance.GoToRhythmsGame ();
	}

	public void RateApp () {
		Application.OpenURL ("itms-apps://itunes.apple.com/app/id1212142417"); 
	}

	public void ToggleGraph () {
		graphPanel.SetActive(!graphPanel.activeSelf);
	}
}
