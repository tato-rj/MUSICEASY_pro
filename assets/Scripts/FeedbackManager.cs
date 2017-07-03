using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FeedbackManager : MonoBehaviour {

	public static FeedbackManager instance;

	public int userRating;

	void Awake () {
		
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this.gameObject);	
		}

	}

	void Start () {
		//File.Delete (Application.persistentDataPath + "/feedbackRecord.dat");
		userRating = LoadRating ();

		print (userRating);
		if (userRating >= 0) {
			RateApp.instance.UpdateStars (userRating);	
		}
	}

	public int LoadRating () {
		//Check if data file for feedback request exists...
		if (File.Exists (Application.persistentDataPath + "/feedbackRecord.dat")) {
			//If it does, open it and returns its content (true or false)
			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/feedbackRecord.dat", FileMode.Open);
			FeedbackRecord data = (FeedbackRecord)binaryFormatter.Deserialize (file);
			file.Close ();

			return data.userRating;

		} else {
			return -1;
		}
	}

	public void SaveRating (int rating) {
		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/feedbackRecord.dat");
		FeedbackRecord data = new FeedbackRecord ();

		data.userRating = rating;

		binaryFormatter.Serialize (file, data);
		file.Close ();
	}
}


[Serializable]
class FeedbackRecord {

	public int userRating;

}