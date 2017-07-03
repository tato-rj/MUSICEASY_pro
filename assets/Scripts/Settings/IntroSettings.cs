using UnityEngine;
using System.Collections;

//CONTROLS THE SETTINGS BUTOTN ON THE INTRO PAGE
public class IntroSettings : MonoBehaviour {

	public GameObject contentSlide;
	public GameObject settingsOptions;
	public GameObject settingsOverlay;
	public GameObject mainScreenOverlay;

	void Awake () {
		
	}

	public void OpenSettings () {

		contentSlide.GetComponent<Animation> ().Play ("slideDown");
		settingsOptions.GetComponent<Animation> ().Play ("slideUp");
		settingsOverlay.GetComponent<Animation> ().Play ("fadeInScreen");
		mainScreenOverlay.GetComponent<Animation> ().Play ("fadeOutScreen");
		gameObject.SetActive (false);

	}

	public void CloseSettings () {

		contentSlide.GetComponent<Animation> ().Play ("slideUp");
		settingsOptions.GetComponent<Animation> ().Play ("slideDown");
		settingsOverlay.GetComponent<Animation> ().Play ("fadeOutScreen");
		mainScreenOverlay.GetComponent<Animation> ().Play ("fadeInScreen");
		gameObject.SetActive (true);

	}

}
