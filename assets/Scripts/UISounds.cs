using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour {

	private AudioSource[] sounds;
	private AudioSource settingsButtonClick;
	private AudioSource inputButtonClick;

	void Awake () {
		sounds = GetComponents<AudioSource> ();
		settingsButtonClick = sounds[0];
		//inputButtonClick = sounds[1];
	}

	public void PlaySettingsButtonSound () {
		settingsButtonClick.Play ();
	}

	public void PlayInputButtonSound () {
		inputButtonClick.Play ();
	}
}
