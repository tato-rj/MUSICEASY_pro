using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//1340
//TO BE USED BY EVERY SETTINGS AND SAVE BUTTONS
public class Settings : MonoBehaviour {

	public static Settings instance;

	public GameObject menu;
	public GameObject[] screenContent;
	public GameObject card;
	public GameObject settingsScreen;
	public GameObject help;

	void Awake () {

		instance = this;

	}

	public void ShowSettingsRhythm () {
		 
		RhythmsGameController.instance.gameOn = false;

		settingsScreen.GetComponent<AudioSource> ().Play ();

		screenContent[0].GetComponent<Animation> ().Play ("slideDown");
		screenContent[1].GetComponent<Animation> ().Play("slideDownCard");
		screenContent [2].SetActive (false);
		settingsScreen.GetComponent<Animation> ().Play ("slideUp");
		//settingsScreen.SetActive (true);

	}

	public void HideSettingsIGotRhythm () {

		RhythmsGameController.instance.SaveAllData();
		RhythmsGameController.instance.ResetGame ();

	}

	public void ShowSettingsNoteMaster () {

		NotesGameController.instance.gameOn = false;

		settingsScreen.GetComponent<AudioSource> ().Play ();
		foreach (GameObject item in screenContent) {
			item.GetComponent<Animation> ().Play ("slideDown");
		}
		card.GetComponent<Animation> ().Play("slideDownCard");
		menu.SetActive (false);
		settingsScreen.GetComponent<Animation> ().Play ("slideUp");

	}

	public void HideSettingsNoteMaster () {

		NotesGameController.instance.SaveData();
		NotesGameController.instance.ResetGame ();

	}

	public void ShowSettingsFlashCards () {

		settingsScreen.GetComponent<AudioSource> ().Play ();

		foreach (GameObject item in screenContent) {
			item.GetComponent<Animation> ().Play ("slideDown");
		}
		menu.SetActive (false);
		settingsScreen.GetComponent<Animation> ().Play ("slideUp");

	}

	public void HideSettingsFlashCards () {

		settingsScreen.GetComponent<AudioSource> ().Play ();
		FlashCardsController.instance.SortCards ();
		foreach (GameObject item in screenContent) {
			item.GetComponent<Animation> ().Play ("slideUp");
		}
		menu.SetActive (true);
		settingsScreen.GetComponent<Animation> ().Play ("slideDown");

	}

	public void ToggleHelp () {
	
		help.SetActive (!help.activeSelf);

	}

	public void ReturnToMainScreen () {

		FadeScene.instance.GoToMainScreen ();

	}


}
