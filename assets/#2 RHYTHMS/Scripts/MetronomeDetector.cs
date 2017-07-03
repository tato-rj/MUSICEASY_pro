using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetronomeDetector : MonoBehaviour {

	public Animation[] countdown;
	Animation anim;
	List<Text> beats = new List<Text>();
	Color32 originalColor;
	int count = 0;
	int countDownIndex;
	AudioSource[] clicks;


	void Start () {
		foreach (Text beatText in transform.parent.GetComponentsInChildren<Text>()) {
			beats.Add (beatText);
		}
		originalColor = beats [0].color;
		clicks = GetComponents<AudioSource> ();
		countDownIndex = countdown.Length - beats.Count;
		//anim = countdown.GetComponent<Animation> ();
	}

	void OnTriggerEnter2D (Collider2D other) {

		if (count == 0) {
			clicks [1].Play ();
		} else {
			clicks [0].Play ();
		}
			
		ResetColor ();
		beats [count].color = Color.white;
		count++;
		if (count == beats.Count) {
			count = 0;
		}

		if (other.name == "Metronome") {

			countdown [0].transform.parent.gameObject.SetActive (false);

		}

		if (other.name == "Countdown") {
			
			countdown [countDownIndex] ["levelName"].speed = Metronome.instance.speed/25f;
			countdown [countDownIndex].Play ();
			countDownIndex++;
		}
	

	}

	void ResetColor () {
		for (int i=0; i<beats.Count; i++) {
			beats [i].color = originalColor;
		}
	}
}
