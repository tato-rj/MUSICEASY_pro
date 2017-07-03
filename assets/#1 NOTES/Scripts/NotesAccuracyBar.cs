using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotesAccuracyBar : MonoBehaviour {

	public Text accuracy;
	private float xPos;
	private Image fillBar;
	private float time;


	void Awake () {

		fillBar = GetComponent<Image> ();

	}

	void FixedUpdate () {

		time += Time.deltaTime;

		fillBar.fillAmount = Mathf.Lerp (0, NotesScoreController.instance.percentage/100, time);
		accuracy.text = LocalizationManager.instance.GetLocalizedValue ("accuracy") + " " + Mathf.Round(Mathf.Lerp (0, NotesScoreController.instance.percentage, time)).ToString() + "%";

	}
}
