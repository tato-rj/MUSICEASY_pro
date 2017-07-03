using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TapDetector : MonoBehaviour {

	public static TapDetector instance;

	public GameObject topArrow;
	public GameObject bottomArrow;
	public GameObject rightPad;
	public GameObject leftPad;
	public GameObject[] rightHandAnswer;
	public GameObject[] leftHandAnswer;

	void Awake () {

		instance = this;

	}

	public void PadTap (Button pad) {

		GameObject arrow = topArrow;
		GetComponent<AudioSource> ().Play ();
		if (pad.name == "Button left") {
			arrow = bottomArrow;
		}
		//IF NOTE TAP WAS CORRECT
		if (arrow.transform.localScale.x > 1f) {

			if (pad.name == "Button right") {
				rightHandAnswer [0].SetActive(false);
				rightHandAnswer [0].SetActive(true);
			} else {
				leftHandAnswer [0].SetActive(false);
				leftHandAnswer [0].SetActive(true);
			}

			pad.GetComponent<Animation> ().Play ("padCorrect");

			RhythmsScoreController.instance.UpdateCorrectNotesCount ();
			RhythmsScoreController.instance.UpdateAccuracy ();
		//IF TAP WAS WRONG
		} else {
			Handheld.Vibrate ();
			if (pad.name == "Button right") {
				rightHandAnswer [1].SetActive(false);
				rightHandAnswer [1].SetActive(true);
			} else {
				leftHandAnswer [1].SetActive(false);
				leftHandAnswer [1].SetActive(true);
			}

			pad.GetComponent<Animation> ().Play ("padWrong");
			RhythmsScoreController.instance.UpdateWrongNotesCount ();
			RhythmsScoreController.instance.UpdateAccuracy ();

		}
	}
}
