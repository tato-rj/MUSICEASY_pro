using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//CONTROLS THE FADE IN AND OUT BETWEEN SCENES
public class FadeScene : MonoBehaviour {

	public static FadeScene instance;

	public bool animateIntro = true;
	public Texture2D fadeOutTexture;
	public float fadeSpeed;
	private int drawDepth = -1000;
	private float alpha = 1f;
	private int fadeDir = -1;

	void Awake () {

		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this.gameObject);	
		}

	}
		
	void OnGUI () {
		
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);

	}

	public float BeginFade (int direction) {

		fadeDir = direction;
		return (fadeSpeed);

	}

	void OnEnable () {
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable () {
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}
		
	void OnLevelFinishedLoading (Scene scene, LoadSceneMode mode) {
		BeginFade (-1);
	}

	public IEnumerator LoadLevel(string levelName) {

		float fadeTime = BeginFade (1);

		yield return new WaitForSeconds (fadeTime/4f);

		SceneManager.LoadScene (levelName);
	}

	public void GoToNotesGame () {

		StartCoroutine(LoadLevel ("notes"));

	}

	public void GoToNotesGameResults () {

		StartCoroutine(LoadLevel ("notes_results"));

	}

	public void GoToRhythmsGame () {

		StartCoroutine(LoadLevel ("rhythms"));

	}

	public void GoToRhythmsGameResults () {

		StartCoroutine(LoadLevel ("rhythms_results"));

	}

	public void GoToFlashCards () {

		StartCoroutine(LoadLevel ("flashCards"));

	}

	public void GoToMainScreen () {

		StartCoroutine(LoadLevel ("_main"));

	}

	public void GoToLanguageScreen () {
		LocalizationManager.instance.isReady = false;
		StartCoroutine(LoadLevel ("userChooseLanguage"));

	}

}
