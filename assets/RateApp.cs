using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateApp : MonoBehaviour {

	public static RateApp instance;

	public GameObject starsContainer;
	public Sprite yellowStar;
	public Sprite greyStar;
	public GameObject rateButton;
	public GameObject contactButton;

	void Awake () {
		instance = this;
	}

	void Start () {

	}

	public void UpdateStars (int starPos) {

		for (int i = 0; i <= 4; i++) {
			if (i <= starPos) {
				starsContainer.transform.GetChild (i).GetComponent<Image> ().sprite = yellowStar;	
			} else {
				starsContainer.transform.GetChild (i).GetComponent<Image> ().sprite = greyStar;
			}

			if (starPos >= 3) {
				rateButton.SetActive (true);
				contactButton.SetActive (false);
			} else {
				contactButton.SetActive (true);
				rateButton.SetActive (false);
			}
		}
	}

	public void SetRatings () {
		
		int starPos = int.Parse(this.gameObject.name);
		UpdateStars (starPos);
		FeedbackManager.instance.SaveRating (starPos);

	}
}
