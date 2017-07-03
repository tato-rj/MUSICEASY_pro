using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotesScoreController : MonoBehaviour {

	public static NotesScoreController instance;
	public GameObject score;
	public GameObject accuracy;
	public int scoreCount = 0;
	public float percentage = 0;
	public int cardsPlayed = 0;

	void Awake () {

		instance = this;

	}

	public void IncrementScore () {

		scoreCount++;
		score.GetComponent<Text> ().text = scoreCount.ToString();

	}

	public void UpdateAccuracy () {
		
	 	percentage = scoreCount * 100 / cardsPlayed;
		accuracy.GetComponent<Text> ().text = percentage.ToString() + "%";

	}

	public void ResetScores () {

		scoreCount = 0;
		score.GetComponent<Text> ().text = "0";
		percentage = 0;
		accuracy.GetComponent<Text> ().text = "-";
		cardsPlayed = 0;

	}


}
