using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreens : MonoBehaviour {

	public void GoToGame () {
		TutorialScreen.instance.SetTutorialStatus ();
		SceneManager.LoadScene ("pre-load_screen");
	}

}
