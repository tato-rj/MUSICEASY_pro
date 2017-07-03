using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateCards : MonoBehaviour {

	private GameObject[] cards;
	private AudioSource buttonClick;

	void Awake () {

		//buttonClick = GetComponent<AudioSource> ();
		cards = GameObject.FindGameObjectsWithTag (gameObject.name);

	}

	public void UpdatesCards () {

		bool state = GetComponent<Toggle> ().isOn;
		foreach (GameObject card in cards) {
			card.SetActive (state);
			//buttonClick.Play ();
		}

	}

	public void Shuffle (GameObject contentToAnimate) {
		FlashCardsController.instance.shuffle = true;
		FlashCardsController.instance.SortCards ();
		contentToAnimate.GetComponent<Animation> ().Play ();
		FlashCardsController.instance.shuffle = false;
	}
		
}