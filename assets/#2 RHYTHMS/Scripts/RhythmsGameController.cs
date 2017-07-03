using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RhythmsGameController : MonoBehaviour {

	public static RhythmsGameController instance;

	public GameObject staffContainer;
	public GameObject[][] levels = new GameObject[3][];

	public GameObject[] easy;
	public GameObject[] medium;
	public GameObject[] hard;
	public GameObject[] easyRH;
	public GameObject[] mediumRH;
	public GameObject[] hardRH;

	public GameObject exercise;

	//SETTINGS
	public GameObject settingsScreen;
	public GameObject settingsLevels;
	public GameObject settingsTimeSignature;
	public GameObject settingsHands;
	public GameObject settingsMeasuresNum;
	public Slider speedSlider;

	public Text timeSignature;
	//public float timeSignatureNum;
	public GameObject startButton;

	public GameObject[] pads;
	public float startX;
	public float startY;
	public bool gameOn;

	//SET STANDARD FOR HANDS (BOTH)
	public GameObject leftHand;
	public GameObject target;

	public List<int> tempRhythmScoreRecords = new List<int>();
	public List<int> tempRhythmAccuracyRecords = new List<int>();

	public GameObject countdown;
	public GameObject metronome;

	void Awake () {

		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
		}
		gameOn = false;

		Screen.orientation = ScreenOrientation.LandscapeLeft;
		startX = 30f;
		LoadData ();
		LoadRecords ();

		//If both hands are selected
		if (!settingsHands.GetComponentsInChildren<Toggle> () [0].isOn) {
			startY = 0;
			levels [0] = easy;
			levels [1] = medium;
			levels [2] = hard;
			//If one hand only is selected
		} else {
			startY = 0.28f;
			levels [0] = easyRH;
			levels [1] = mediumRH;
			levels [2] = hardRH;
		}
	}
		
	void FixedUpdate () {

		if (!gameOn) {
			StopAllCoroutines ();
		}

	}

	public void StartGame () {

		gameOn = true;
		//FadeScene.instance.GetComponent<AudioSource> ().Stop ();
		pads [0].SetActive (true);
		pads [1].SetActive (true);
		startButton.SetActive (false);
		//metronome.SetActive (true);

		//CHECK LEVEL
		for (int i=0; i<settingsLevels.GetComponentsInChildren<Toggle>().Length; i++) {
			if (settingsLevels.GetComponentsInChildren<Toggle> ()[i].isOn) {
				if (i == 1) {
					startX += 6f;
				} else if (i == 2) {
					startX += 16f;
				} 
				//INSTANTIATE EXERCISE
				exercise = (GameObject)Instantiate (
					levels[i][UnityEngine.Random.Range(0,levels[i].Length)], 
					new Vector3(startX, startY, 0),
					Quaternion.identity);
				
				exercise.transform.parent = staffContainer.transform;
			}
		}

		SetupHands ();
		//GET THE TIME SIGNATURE
		GetTimeSignature (exercise.transform.localScale.z);
	
	}

	void GetTimeSignature (float num) {
		//timeSignatureNum = num;

		String numText = num.ToString ();

		timeSignature.text = numText.Substring(0, numText.Length-1) + "\n" + numText.Substring(numText.Length-1);
		timeSignature.gameObject.SetActive (true);
		Metronome.instance.metronome.transform.Find (num.ToString()).gameObject.SetActive(true);
	}

	void SetupHands () {

		//IF RIGHT HAND ONLY
		if (settingsHands.GetComponentsInChildren<Toggle> () [0].isOn) {
			//CENTER RIGHT HAND ANSWER
			//TapDetector.instance.rightHandAnswer[0].transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2 (0,0);

			leftHand.GetComponent<SpriteRenderer> ().color = new Color32 (0, 0, 0, 50);
			//GameObject.FindGameObjectWithTag ("Bottom").SetActive (false);
			pads [0].SetActive (false);
		//IF BOTH HANDS
		} else {
			//CENTER RIGHT HAND ANSWER
			//TapDetector.instance.rightHandAnswer[0].transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2 (80f,0);

			leftHand.GetComponent<SpriteRenderer> ().color = new Color32 (255, 98, 73, 255);
			//GameObject.FindGameObjectWithTag ("Bottom").SetActive (true);
			pads [0].SetActive (true);
		}
		target.SetActive (true);

	}
		
	public void ResetGame () {

		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);

	}

	/*
	 *
	 * SAVE AND LOAD USER SETTINGS
	 *
	 */

	public void SaveAllData () {


			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + "/iGotRhythmSettings.dat");
			IGotRhythmSettings data = new IGotRhythmSettings ();

			//SAVE LEVEL
			//1) SAVE CURRENT LEVEL
			for (int i = 0; i < settingsLevels.GetComponentsInChildren<Toggle> ().Length; i++) {
				data.onLevel [i] = settingsLevels.GetComponentsInChildren<Toggle> () [i].isOn;
			}

			//SAVE HANDS
			for (int i = 0; i < settingsHands.GetComponentsInChildren<Toggle> ().Length; i++) {
				data.hands [i] = settingsHands.GetComponentsInChildren<Toggle> () [i].isOn;
			}

			//SAVE SPEED
			data.speed = speedSlider.value;

			binaryFormatter.Serialize (file, data);
			file.Close ();

	}

	public void LoadData () {
		print ("RHYTHM GAME SETTINGS:");
		if (File.Exists (Application.persistentDataPath + "/iGotRhythmSettings.dat")) {
			

			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/iGotRhythmSettings.dat", FileMode.Open);
			IGotRhythmSettings data = (IGotRhythmSettings)binaryFormatter.Deserialize (file);
			file.Close ();

			//LOAD LEVEL
			for (int i=0; i<settingsLevels.GetComponentsInChildren<Toggle> ().Length; i++) {
				settingsLevels.GetComponentsInChildren<Toggle> () [i].isOn = data.onLevel[i];
			}

			//LOAD HANDS
			for (int i=0; i<settingsHands.GetComponentsInChildren<Toggle> ().Length; i++) {
				settingsHands.GetComponentsInChildren<Toggle> () [i].isOn = data.hands[i];
			}

			//LOAD SPEED
			speedSlider.value = data.speed;
			print (" -> User settings applied.");
		} else {
			//IF FILE DOES NOT YET EXISTS, OPEN THE WELCOME SCREEN 
			print (" -> No settings have been set by the user.");
			print (" -> Using default settings.");

			//DEFAULT SETTINGS ARE SELECTED

			//LOAD LEVEL

			settingsLevels.GetComponentsInChildren<Toggle> () [0].isOn = true;
			settingsLevels.GetComponentsInChildren<Toggle> () [1].isOn = false;
			settingsLevels.GetComponentsInChildren<Toggle> () [2].isOn = false;

			//LOAD HANDS
			settingsHands.GetComponentsInChildren<Toggle> () [1].isOn = true;

			//LOAD SPEED
			speedSlider.value = 50;

		}			
	}

	public void SaveRecords (int accuracy, int totalScore) {

		tempRhythmAccuracyRecords.Add (accuracy);
		tempRhythmScoreRecords.Add (totalScore);

		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/iGotRhythmRecords.dat");
		IGotRhythmSettings data = new IGotRhythmSettings ();

		if (tempRhythmScoreRecords.Count > 20) {
			print ("There are more than 20 records. Oldest record on the list deleted.");
			tempRhythmScoreRecords.RemoveAt (0);
		}

		data.accuracyRecords = tempRhythmAccuracyRecords;
		data.scoreRecords = tempRhythmScoreRecords;

		print ("RHYTHM GAME RECORDS SAVED:");
		print (" -> Accuracy was: " + accuracy);
		print (" -> Score was: " + totalScore);
		binaryFormatter.Serialize (file, data);
		file.Close ();
	}

	public void LoadRecords () {
		print ("RHYTHM GAME RECORDS:");
		if (File.Exists (Application.persistentDataPath + "/iGotRhythmRecords.dat")) {

			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/iGotRhythmRecords.dat", FileMode.Open);
			IGotRhythmSettings data = (IGotRhythmSettings)binaryFormatter.Deserialize (file);
			file.Close ();

			tempRhythmScoreRecords = data.scoreRecords;
			tempRhythmAccuracyRecords = data.accuracyRecords;

			int numOfGamesPlayed = tempRhythmAccuracyRecords.Count;

			print (" -> " + numOfGamesPlayed + " game(s) have been played so far.");

		} else {
			print (" -> There are no records set.");
		}
	}
}

//CLASS THAT WILL WRITE TO THE FILE THE USER SETTINGS
[Serializable]
class IGotRhythmSettings {

	public bool[] onLevel = new bool[3];
	public bool[] hands = new bool[2];
	public float speed;
	public List<int> accuracyRecords = new List<int> ();
	public List<int> scoreRecords = new List<int> ();

}
