using UnityEngine;
using System.Collections;

public class NoteDetector : MonoBehaviour {

	public GameObject topArrow;
	public GameObject lowerArrow;
	public GameObject metronome;
	public GameObject redBar;
	private Vector3 growBall = new Vector3 (1.2f,1.2f,0);
	private Vector3 defaultBall = new Vector3 (1f,1f,0);

	void OnTriggerEnter2D (Collider2D other) {

		if (other.tag == "Top") {

			topArrow.transform.localScale = growBall;
			RhythmsScoreController.instance.notesPassed += 1;
			//topArrow.GetComponent<Animation> ().Play ();

		} else if (other.tag == "Bottom") {

			lowerArrow.transform.localScale = growBall;
			RhythmsScoreController.instance.notesPassed += 1;
			//lowerArrow.GetComponent<Animation> ().Play ();

		}

	}
		
	void OnTriggerExit2D (Collider2D other) {

		if (other.tag == "Top") {
			
			topArrow.transform.localScale = defaultBall;
			RhythmsScoreController.instance.UpdateAccuracy ();

		} else if (other.tag == "Bottom") {

			lowerArrow.transform.localScale = defaultBall;
			RhythmsScoreController.instance.UpdateAccuracy ();
		}

		if (other.name == "End") {
			metronome.GetComponent<Animation> ().Play("metronomeOut");
			redBar.GetComponent<Animation> ().Play("redBarOut");
		}		
	}
}
