using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NotesGameController : MonoBehaviour {

	public static NotesGameController instance;

	public GameObject settingsScreen;
	public GameObject staff;
	public GameObject note;
	public GameObject[] accidentals;
	public GameObject ledgerLine;
	public float lowerPos;
	public float halfStep;
	public bool[] hasAccident = new bool[3];
	public string randomNoteName;
	private GameObject accident;
	private string accidentSymbol;
	private string[] noteNames = { "A1", "B1", "C2", "D2", "E2", "F2", "G2", 
									"A2", "B2", "C3", "D3", "E3", "F3", "G3", 
										"A3", "B3", "C4", "D4", "E4", "F4", "G4", 
											"A4", "B4", "C5", "D5", "E5", "F5", "G5", 
												"A5", "B5", "C6", "D6", "E6"};
	

	//Clefs
	public GameObject[] clefs;
	public string clefName;
	private float[] trebleClefPos = {-1.35f, 1.07f};
	private float[] bassClefPos = {-1.233f, 1.04f};
	private float[] tenorClefPos = {-1.36f, 1.046f};
	private float[] altoClefPos = {-1.36f, 1.234f};
	private float[][] clefsPos = new float[4][];

	//Level
	public int[][] level = new int[3][];
	public int min;
	public int max;
	public int levelNum;

	public List<int> tempNoteScoreRecords;
	public List<int> tempNoteAccuracyRecords;

	public GameObject settingsLevels;
	public GameObject settingsAccidentals;
	public GameObject settingsClefs;
	public GameObject settingsDuration;

	public GameObject card;
	public GameObject cardBack;
	public GameObject timeContainer;
	public GameObject startButton;

	public float timeRemaining;
	public Text countdownText;
	public bool gameOn = false;
	private bool coroutineIsOn = false;
	public GameObject clock;
	private int lastNum;

	void Awake () {

		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
		}
			
		tempNoteScoreRecords = new List<int> ();
		tempNoteAccuracyRecords = new List<int> ();

		//Easy
		level [0] = new int[]{8,13};
		//Medium
		level [1] = new int[]{5,16};
		//Hard
		level [2] = new int[]{0,21};

		clefsPos [0] = trebleClefPos;
		clefsPos [1] = bassClefPos;
		clefsPos [2] = altoClefPos;
		clefsPos [3] = tenorClefPos;

		lowerPos = 0.134f;
		halfStep = 0.0915f;

		LoadData ();
		LoadRecords ();

	}

	void Start () {

		Screen.orientation = ScreenOrientation.Portrait;

	}

	void FixedUpdate () {

		if (gameOn) {

			if (timeRemaining >= 0 && timeRemaining <= 120) {
				countdownText.text = (int)timeRemaining + "";
				timeRemaining -= Time.deltaTime;

				if (timeRemaining <= 6 && !coroutineIsOn) {

					StartCoroutine (CountDownSound());
				}

			} else if (timeRemaining < 1) {

				StartCoroutine (TimeUp());
			}
		}

	}

	public IEnumerator CountDownSound () {

		coroutineIsOn = true;

		for (int i=0; i<=5; i++) {
			countdownText.GetComponent<AudioSource> ().Play ();
			countdownText.GetComponent<Animation> ().Play ();
			yield return new WaitForSeconds (1f);			
		}
	
	}

	public void StartGame () {

		if (timeRemaining == 999) {
			countdownText.text = "-";
		} else {
			countdownText.text = timeRemaining.ToString();
		}
		SetLevel ();
		StartCoroutine (FlipCard());

	}

	public void ResetGame () {

		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	
	}

	public IEnumerator FlipCard () {

		startButton.SetActive (false);
		card.GetComponent<Animation>().Play ("cardFlipA");
		card.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (0.16f);
	
		cardBack.SetActive (false);

		card.GetComponent<Animation>().Play ("cardFlipB");

		yield return new WaitForSeconds (0.2f);

		timeContainer.SetActive (true);
		CreateNewNote ();
		gameOn = true;

	}

	public IEnumerator TimeUp () {

		if (!clock.GetComponent<Animation>().isPlaying) {
			clock.GetComponent<Animation>().Play ();
			clock.GetComponent<AudioSource> ().Play ();
		}

		countdownText.text = "-";

		yield return new WaitForSeconds (1.8f);

		StartCoroutine(FadeScene.instance.LoadLevel ("notes_results"));
		//LevelManager.instance.GoToNotesGameResults ();
	}

	public void CreateNewNote () {

		//Destroy current note
		foreach (Transform child in staff.transform) {
			Destroy (child.gameObject);
		}

		InstantiateClef ();
		RandomNote ();
		InputClick.instance.ToggleMusicName ();
	}

	void SetLevel () {

		for (int i=0; i<settingsLevels.GetComponentsInChildren<Toggle> ().Length; i++) {
			if (settingsLevels.GetComponentsInChildren<Toggle> ()[i].isOn) {
				min = level[i][0];
				max = level[i][1];
			}
		}
	}

	private int GetNewRandomNum () {
		//Random number to get a position for the note
		int num;
		//min = 0 and max = 21
		num = UnityEngine.Random.Range (min, max);

		if (num == lastNum) {
			num = UnityEngine.Random.Range (min, max);
		}

		return num;
	}

	void RandomNote () {

		int randomNum = GetNewRandomNum ();

		//New position of the note
		float newPos;

		string noteName;
		string octave;

		//Multiplies the half step distance by a random number of intervals
		newPos = halfStep * randomNum;

		//Instantiates new note at the random position and puts the note as child of the staff parent
		GameObject newNote = (GameObject)Instantiate (note, new Vector3 (0, lowerPos + newPos, 0), Quaternion.identity);
		newNote.transform.parent = staff.transform;

		//Tags the note with its name
		noteName = GetName (randomNum, clefName);

		//Gets the octave the notes is in
		octave = noteName.Substring(noteName.Length - 1);

		//Sets the tag (Removes the octave number)
		newNote.tag = noteName.Substring(0, noteName.Length - 1);

		//If needed
		AddAccident (newNote);

		//If needed
		AddLedgerLine (randomNum);

		//Adjusts the name of the note in case of E sharp, C flat, etc
		if (accidentSymbol != "No accident") {
			newNote.tag += accidentSymbol;
			//Fix issue with Cb, B#, Fb and E#
			if (newNote.tag == "Cb") {
				//Adjusts the octave
				octave = (int.Parse (octave)-1).ToString();
				newNote.tag = "B";
			} else if (newNote.tag == "Fb") {
				newNote.tag = "E";
			} else if (newNote.tag == "B#") {
				//Adjusts the octave
				octave = (int.Parse (octave)+1).ToString();
				newNote.tag = "C";
			} else if (newNote.tag == "E#") {
				newNote.tag = "F";
			}
		}

		if (/*!Mic.instance.listening*/true) {
			NotesSounds.instance.PlayNoteSound (newNote.tag+octave);
		}
		//DISPLAYS THE NAME OF THE NOTE (FOR DEBUGGING PURPOSES)
		randomNoteName = newNote.tag;
		if (randomNoteName == "Db") {
			randomNoteName = "C#";	
		}
		if (randomNoteName == "Eb") {
			randomNoteName = "D#";
		}
		if (randomNoteName == "Gb") {
			randomNoteName = "F#";
		}
		if (randomNoteName == "Ab") {
			randomNoteName = "G#";
		}
		if (randomNoteName == "Bb") {
			randomNoteName = "A#";
		}
		randomNoteName += octave;
		print ("Target is: "+randomNoteName);
		lastNum = randomNum;
	}

	public string GetName(int arrayIndex, string clefName) {

		if (clefName == "Bass clef") {
			//Default clef
		} else if (clefName == "Treble clef") {
			arrayIndex += 12;
		} else if (clefName == "C clef") {
			arrayIndex += 6;
		} else if (clefName == "Alto clef") {
			arrayIndex += 4;
		}

		//Searches in the array of names for the name of the note and returns it as a string
		return noteNames[arrayIndex];

	}

	void InstantiateClef () {

		int index;
		List<int> clefList = new List<int>();

		for (int i=0; i<settingsClefs.GetComponentsInChildren<Toggle> ().Length; i++) {

			if (settingsClefs.GetComponentsInChildren<Toggle> ()[i].isOn) {
				clefList.Add(i);
			}

		}

		index = UnityEngine.Random.Range (0, clefList.Count);

		GameObject newClef = (GameObject)Instantiate (clefs[clefList[index]], new Vector3 (clefsPos[clefList[index]][0], clefsPos[clefList[index]][1], 0), Quaternion.identity);
		newClef.transform.parent = staff.transform;
		clefName = newClef.tag;

	}

	void AddAccident (GameObject note) {
		
		List<int> accidentList = new List<int> ();
		int index;

		accidentList.Add (0);

		for (int i=0; i<settingsAccidentals.GetComponentsInChildren<Toggle> ().Length; i++) {

			if (settingsAccidentals.GetComponentsInChildren<Toggle> ()[i].isOn) {
				accidentList.Add(i+1);
			}

		}

		index = UnityEngine.Random.Range (0, accidentList.Count);
		InputController.instance.ChangeKeyboard(accidentList[index]);

		GameObject newAccident = (GameObject)Instantiate (accidentals[accidentList[index]], new Vector3(-0.262f, note.transform.position.y, 0), Quaternion.identity);
		newAccident.transform.parent = note.transform; 
		accidentSymbol = newAccident.tag;

	}

	void AddLedgerLine (int randomNum) {
		float yPos;

		//Lines above staff
		if (randomNum > 15) {
			yPos = lowerPos + halfStep * 16;
			InstantiateLedgerLine (yPos);

			if (randomNum > 17) {
				yPos = lowerPos + halfStep * 18;
				InstantiateLedgerLine (yPos);

				if (randomNum == 20) {
					yPos = lowerPos + halfStep * 20;
					InstantiateLedgerLine (yPos);
				}	
			}	
		}

		//Lines below staff
		if (randomNum < 5) {
			yPos = lowerPos + halfStep * 4;
			InstantiateLedgerLine (yPos);

			if (randomNum < 3) {
				yPos = lowerPos + halfStep * 2;
				InstantiateLedgerLine (yPos);

				if (randomNum == 0) {
					yPos = lowerPos;
					InstantiateLedgerLine (yPos);
				}	
			}	
		}
	}

	void InstantiateLedgerLine (float yPos) {
		GameObject newLedgerLine = (GameObject)Instantiate (ledgerLine, new Vector3(0, yPos, 0), Quaternion.identity);	
		newLedgerLine.transform.parent = staff.transform;
	}

	/*
	 * 
	 * 
	 * 
	 * 
	 *	SAVE/LOAD DATA 
	 * 
	 * 
	 * 
	 * 
	 */

	public void SaveData () {

		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/noteMasterSettings.dat");
		NoteMasterSettings data = new NoteMasterSettings ();

		data.onLevel[0] = settingsLevels.GetComponentsInChildren<Toggle> () [0].isOn;
		data.onLevel[1] = settingsLevels.GetComponentsInChildren<Toggle> () [1].isOn;
		data.onLevel[2] = settingsLevels.GetComponentsInChildren<Toggle> () [2].isOn;

		data.isClef[0] = settingsClefs.GetComponentsInChildren<Toggle> () [0].isOn;
		data.isClef[1] = settingsClefs.GetComponentsInChildren<Toggle> () [1].isOn;
		data.isClef[2] = settingsClefs.GetComponentsInChildren<Toggle> () [2].isOn;
		data.isClef[3] = settingsClefs.GetComponentsInChildren<Toggle> () [3].isOn;

		data.hasAccident[0] = settingsAccidentals.GetComponentsInChildren<Toggle> () [0].isOn;
		data.hasAccident[1] = settingsAccidentals.GetComponentsInChildren<Toggle> () [1].isOn;

		for (int i=0; i<settingsDuration.GetComponentsInChildren<Toggle> ().Length; i++) {
			if (settingsDuration.GetComponentsInChildren<Toggle> () [i].isOn) {
				data.duration = float.Parse(settingsDuration.GetComponentsInChildren<Toggle> () [i].name);
			}
		}

		binaryFormatter.Serialize (file, data);
		file.Close ();

	}

	public void LoadData () {

		print ("NOTE GAME SETTINGS:");
		if (File.Exists (Application.persistentDataPath + "/noteMasterSettings.dat")) {

			float duration;

			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/noteMasterSettings.dat", FileMode.Open);
			NoteMasterSettings data = (NoteMasterSettings)binaryFormatter.Deserialize (file);
			file.Close ();

			//LOAD LEVEL
			for (int i=0; i<settingsLevels.GetComponentsInChildren<Toggle> ().Length; i++) {
				settingsLevels.GetComponentsInChildren<Toggle> () [i].isOn = data.onLevel[i];
				if (settingsLevels.GetComponentsInChildren<Toggle> () [i].isOn) {
					levelNum = i +=1 ;
				}
			}

			//LOAD DURATION
			for (int i=0; i<settingsDuration.GetComponentsInChildren<Toggle> ().Length; i++) {
				duration = float.Parse(settingsDuration.GetComponentsInChildren<Toggle> () [i].name);
				if (duration == data.duration) {
					settingsDuration.GetComponentsInChildren<Toggle> () [i].isOn = true;
					timeRemaining = duration;
				}
			}

			//LOAD CLEF
			settingsClefs.GetComponentsInChildren<Toggle> () [0].isOn = data.isClef [0];
			settingsClefs.GetComponentsInChildren<Toggle> () [1].isOn = data.isClef [1];
			settingsClefs.GetComponentsInChildren<Toggle> () [2].isOn = data.isClef [2];
			settingsClefs.GetComponentsInChildren<Toggle> () [3].isOn = data.isClef [3];
			//If user left all clefs unchecked, check Treble clef by default
			if (!settingsClefs.GetComponentsInChildren<Toggle> () [0].isOn &&
				!settingsClefs.GetComponentsInChildren<Toggle> () [1].isOn &&
				!settingsClefs.GetComponentsInChildren<Toggle> () [2].isOn&&
				!settingsClefs.GetComponentsInChildren<Toggle> () [3].isOn) {
				settingsClefs.GetComponentsInChildren<Toggle> () [0].isOn = true;
			}

			//LOAD ACCIDENT
			settingsAccidentals.GetComponentsInChildren<Toggle> () [0].isOn = data.hasAccident [0];
			settingsAccidentals.GetComponentsInChildren<Toggle> () [1].isOn = data.hasAccident [1];
			print (" -> User settings applied.");
		} else {
			
			print (" -> No settings have been set by the user.");
			print (" -> Using default settings.");

			settingsLevels.GetComponentsInChildren<Toggle> () [0].isOn = true;
			settingsLevels.GetComponentsInChildren<Toggle> () [1].isOn = false;
			settingsLevels.GetComponentsInChildren<Toggle> () [2].isOn = false;

			settingsClefs.GetComponentsInChildren<Toggle> () [0].isOn = true;
			settingsClefs.GetComponentsInChildren<Toggle> () [1].isOn = false;
			settingsClefs.GetComponentsInChildren<Toggle> () [2].isOn = false;
			settingsClefs.GetComponentsInChildren<Toggle> () [3].isOn = false;

			settingsAccidentals.GetComponentsInChildren<Toggle> () [0].isOn = false;
			settingsAccidentals.GetComponentsInChildren<Toggle> () [1].isOn = false;

			settingsDuration.GetComponentsInChildren<Toggle> () [1].isOn = true;
			timeRemaining = float.Parse(settingsDuration.GetComponentsInChildren<Toggle> () [1].name);
		
		}
	}

	public void SaveRecords (int accuracy, int totalScore) {

		tempNoteAccuracyRecords.Add (accuracy);
		tempNoteScoreRecords.Add (totalScore);

		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/noteMasterRecords.dat");
		NoteMasterSettings data = new NoteMasterSettings ();

		if (tempNoteScoreRecords.Count > 20) {
			print ("There are more than 20 records. Oldest record on the list deleted.");
			tempNoteScoreRecords.RemoveAt (0);
		}

		data.accuracyRecords = tempNoteAccuracyRecords;
		data.scoreRecords = tempNoteScoreRecords;

		print ("NOTE GAME RECORDS SAVED:");
		print (" -> Accuracy was: " + accuracy);
		print (" -> Score was: " + totalScore);

		binaryFormatter.Serialize (file, data);
		file.Close ();
	}

	public void LoadRecords () {
		print ("NOTE GAME RECORDS:");

		if (File.Exists (Application.persistentDataPath + "/noteMasterRecords.dat")) {

			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/noteMasterRecords.dat", FileMode.Open);
			NoteMasterSettings data = (NoteMasterSettings)binaryFormatter.Deserialize (file);
			file.Close ();

			tempNoteScoreRecords = data.scoreRecords;
			tempNoteAccuracyRecords = data.accuracyRecords;

			int numOfGamesPlayed = tempNoteAccuracyRecords.Count;

			print (" -> " + numOfGamesPlayed + " game(s) have been played so far.");

		} else {
			print (" -> There are no records set.");
		}
	}
}

[Serializable]
class NoteMasterSettings {
	
	public bool[] onLevel = new bool[3];
	public bool[] isClef = new bool[4];
	public bool[] hasAccident = new bool[2];
	public float duration;
	public List<int> accuracyRecords = new List<int> ();
	public List<int> scoreRecords = new List<int> ();

}
