using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class ToggleInputMenu : MonoBehaviour, IPointerClickHandler {

	public Text[] inputs;
	public GameObject musicNotes;
	public GameObject pianoKeys;
	public GameObject blueLine;
	private Text menu;
	private Color gray;
	private Color blue;

	void Awake () {

		menu = GetComponent<Text> ();
		blue = inputs [0].color;
		gray = inputs [1].color;

	}

	public void OnPointerClick (PointerEventData eventData) {
		
		if (menu.color == gray) {
			blueLine.GetComponent<AudioSource> ().Play ();
			if (menu.tag == "Music notes") {
				pianoKeys.SetActive (false);
				musicNotes.SetActive (true);
				blueLine.GetComponent<Animation> ().Play ("blueLineToLeft");
			} else if (menu.tag == "Piano keys") {
				pianoKeys.SetActive (true);
				musicNotes.SetActive (false);
				blueLine.GetComponent<Animation> ().Play ("blueLineToRight");
			}
		}
		foreach (Text input in inputs) {
			input.color = gray;
		}
		menu.color = blue;
	}
}
