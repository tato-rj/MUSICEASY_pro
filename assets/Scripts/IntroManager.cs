using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour {

	public GameObject[] elements;
	public GameObject[] buttons;
	public GameObject title;
	public GameObject welcome;
	public GameObject helpPanel;
	public GameObject requestScreen;

	void Awake () {
		Screen.orientation = ScreenOrientation.Portrait;
	}

	void Start () {

		if (FadeScene.instance.animateIntro) {
			if (TutorialScreen.instance.LoadTutorialStatus()) {

				SceneManager.LoadScene ("intro");

			} else {
			
				StartCoroutine (IntroSequence ());

			}
		
		} else {

			//title.SetActive (true);

			foreach (GameObject child in elements) {

				child.SetActive (true);

			}

			StartCoroutine (ShowButtons());

		}
	
	}

	public IEnumerator IntroSequence () {

		yield return new WaitForSeconds (0.5f);

		welcome.SetActive (true);

		yield return new WaitForSeconds (1f);

		welcome.GetComponent<Animation> ().Play ("fadeOut");

		yield return new WaitForSeconds (1f);

		foreach (GameObject child in elements) {

			child.SetActive (true);

		}

		StartCoroutine (ShowButtons());
		FadeScene.instance.animateIntro = false;
	}

	public IEnumerator ShowButtons () {

		foreach (GameObject child in buttons) {

			child.GetComponent<Animation>().Play();
			child.GetComponent<AudioSource> ().Play ();
			yield return new WaitForSeconds (0.1f);

		}

	}


	public void ToggleHelpScreen () {

		helpPanel.SetActive (!helpPanel.activeSelf);
	
	}

	public void RateApp () {
		Application.OpenURL ("itms-apps://itunes.apple.com/app/id1212142417"); 
	}

	public void ContactUs () {

		string email = "contact@leftlaneapps.com";
		string subject = MyEscapeURL ("Contact from MusicEasy");

		Application.OpenURL ("mailto:" + email + "?subject=" + subject);

	}

	string MyEscapeURL (string url) {

		return WWW.EscapeURL (url).Replace ("+", "%20");

	}

}
