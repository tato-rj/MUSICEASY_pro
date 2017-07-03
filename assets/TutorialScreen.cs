using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TutorialScreen : MonoBehaviour {

	public static TutorialScreen instance;

	void Awake () {

		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this.gameObject);	
		}

	}

	public bool LoadTutorialStatus () {
		//Check if data file for feedback request exists...
		if (File.Exists (Application.persistentDataPath + "/tutorialStatus.dat")) {
			//If it does, open it and returns its content (true or false)
			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/tutorialStatus.dat", FileMode.Open);
			TutorialStatus data = (TutorialStatus)binaryFormatter.Deserialize (file);
			file.Close ();

			return data.showTutorial;

		} else {
			return true;
		}
	}

	public void SetTutorialStatus () {
		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/tutorialStatus.dat");
		TutorialStatus data = new TutorialStatus ();

		data.showTutorial = false;

		binaryFormatter.Serialize (file, data);
		file.Close ();
	}

}

[Serializable]
class TutorialStatus {

	public bool showTutorial;

}