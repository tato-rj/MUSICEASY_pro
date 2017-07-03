using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RhythmsAccuracyBar : MonoBehaviour {

	public Text accuracy;
	private float xPos;
	private Image fillBar;
	private float time;


	void Awake () {
	
		fillBar = GetComponent<Image> ();

	}

	void FixedUpdate () {

		time += Time.deltaTime;

		fillBar.fillAmount = Mathf.Lerp (0, RhythmsScoreController.instance.percentage/100, time);
		accuracy.text = LocalizationManager.instance.GetLocalizedValue ("accuracy") + " " + Mathf.Round(Mathf.Lerp (0, RhythmsScoreController.instance.percentage, time)).ToString() + "%";
	
	}
}
