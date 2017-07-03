using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour {

	private IEnumerator Start () {

		string language = Application.systemLanguage.ToString() + ".json";
		string userLang = SaveLoadLanguage.instance.LoadLanguage ();
		print ("LANGUAGE SETTINGS:");
		//Check if user saved a language preference. If not...
		if (userLang == "No") {
			print (" -> No languages have been saved by the user.");
			LocalizationManager.instance.LoadLocalizedText (language);

			//If user did save a language, use language set by the user
		} else {
			print (" -> Using user's prefered language!");
			LocalizationManager.instance.LoadLocalizedText (userLang + ".json");

		}

		while (!LocalizationManager.instance.GetIsReady ()) {
			yield return null;
		}

		SceneManager.LoadScene ("_main");
	}

}
