using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveLoadLanguage : MonoBehaviour {

	public static SaveLoadLanguage instance;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	public void SaveLanguage (string language) {

		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/musiclyLanguage.dat");
		UserLanguage data = new UserLanguage ();

		data.language = language;

		binaryFormatter.Serialize (file, data);
		file.Close ();

		SceneManager.LoadScene ("pre-load_screen");
	}

	public string LoadLanguage () {
		if (File.Exists (Application.persistentDataPath + "/musiclyLanguage.dat")) {

			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/musiclyLanguage.dat", FileMode.Open);
			UserLanguage data = (UserLanguage)binaryFormatter.Deserialize (file);
			file.Close ();

			return data.language;

		} else {
			return "No";
		}

	}

	public void ResetLanguage () {
		File.Delete (Application.persistentDataPath + "/musiclyLanguage.dat");
		SceneManager.LoadScene ("pre-load_screen");
	}
}


[Serializable]
class UserLanguage {

	public string language;

}
