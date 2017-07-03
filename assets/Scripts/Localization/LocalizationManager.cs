using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour {

	public static LocalizationManager instance;
	public bool isReady = false;
	private string missingText = "Localized text not found";

	private Dictionary<string, string> localizedText;

	void Awake () {

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	public void LoadLocalizedText (string fileName) {

		//Creates a dictionary to store language
		localizedText = new Dictionary<string, string> ();
		//Creates a file path with users language
		string filePath = Path.Combine (Application.streamingAssetsPath, fileName);

		//If that translation exists...
		if (File.Exists (filePath)) {
			print (" -> User's language is: " + fileName);
		//If translation does NOT exits...
		} else {
			//Sets the file path to be the default in English
			filePath = Path.Combine (Application.streamingAssetsPath, "English.json");
			print (" -> Language not found. Default English was loaded.");

		}
		//Opens the file
		string dataAsJson = File.ReadAllText (filePath);
		LocalizationData loadedData = JsonUtility.FromJson<LocalizationData> (dataAsJson);
		//Adds the translation to the dictionary
		for (int i=0; i<loadedData.items.Length; i++) {
			localizedText.Add (loadedData.items [i].key, loadedData.items [i].value);
		}
		//Load game
		isReady = true;

	}

	public string GetLocalizedValue (string key) {
	
		string result = missingText;

		if (localizedText.ContainsKey(key)) {
			result = localizedText [key];
		}

		return result;

	}

	public bool GetIsReady() {
		return isReady;
	}


}
