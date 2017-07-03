using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.iOS;

public class FlashCardsController : MonoBehaviour {

	public static FlashCardsController instance;

	public Text totalNumOfCards;
	public Text currentCardNo;
	public Text section;
	public RectTransform content;
	public GameObject noCardsInView;
	private List<GameObject> cards = new List<GameObject> ();
	public RectTransform center;
	public bool shuffle;

	private float[] distance;
	private bool dragging = false;
	private int cardDistance = 550;
	private int minCardNum;
	private int numberOfCards;

	private int cardInView;

	void Awake () {

		instance = this;

		/*
		//If device is an iPad
		if ((Device.generation.ToString()).IndexOf("iPad") > -1) {
			currentCardNo.gameObject.SetActive (false);
			totalNumOfCards.gameObject.SetActive (false);
			section.gameObject.SetActive (false);
		}
		*/

		shuffle = false;
		SortCards ();

	}

	void Start () {

		numberOfCards = cards.Count;
		print ("FLASH CARDS:");
		print (" -> There are " + numberOfCards + " cards available.");
		distance = new float[numberOfCards];

	}

	void Update () {
	
		for (int i=0; i<numberOfCards; i++) {
			distance [i] = Mathf.Abs (center.transform.position.x - cards[i].transform.position.x);
		}

		float minDistance = Mathf.Min (distance);

		for (int a=0; a<numberOfCards; a++) {
			if (minDistance == distance[a]) {
				cardInView = a + 1;
				currentCardNo.text = LocalizationManager.instance.GetLocalizedValue ("flashcards_card")+" "+cardInView+" "+LocalizationManager.instance.GetLocalizedValue ("flashcards_of");
				section.text = LocalizationManager.instance.GetLocalizedValue ("flashcards_"+cards [a].tag);
				minCardNum = a;
			}
		}

		if (!dragging) {

			LerpToCard (minCardNum * -cardDistance);
							
		}

	}

	void ResetPosition () {
		content.anchoredPosition = new Vector2 (0,content.anchoredPosition.y);


	}

	void LerpToCard (int position) {

		float newX = Mathf.Lerp (content.anchoredPosition.x, position, Time.deltaTime * 5f);
		Vector2 newPosition = new Vector2 (newX, content.anchoredPosition.y);

		content.anchoredPosition = newPosition;

	}

	public void StartDrag () {

		dragging = true;

	}

	public void EndDrag () {

		StartCoroutine(DelayLerp());

	}

	public IEnumerator DelayLerp () {
		yield return new WaitForSeconds (0.4f);
		dragging = false;
	}

	public void SortCards () {

		//CLEAR LIST
		cards.Clear ();

		//LOOKS AT EACH CARD
		foreach (Transform child in content.transform) {
			//ADDS IT TO THE LIST IF IT IS ACTIVE
			if (child.gameObject.activeSelf) {
				cards.Add (child.gameObject);
			}

		}
		//COUNTS NUMBER OF CARDS IN THE LIST
		numberOfCards = cards.Count;

		if (shuffle) {
			for (int i=0; i<cards.Count; i++) {
				GameObject temp = cards [i];
				int randomNumber = Random.Range (i, cards.Count);
				cards [i] = cards [randomNumber];
				cards [randomNumber] = temp;
			}
		}

		//WRITES NUMBER OF CARDS ON THE SCREEN
		if (cards.Count != 0) {
			totalNumOfCards.text = cards.Count.ToString ();
			noCardsInView.SetActive (false);
		} else {
			section.text = "";
			totalNumOfCards.text = "";
			currentCardNo.text = "";
			noCardsInView.SetActive (true);
		}

		//ADJUSTS THE POSITIONS OF THE NEW LIST
		for (int i=0; i<cards.Count; i++) {
			
			Vector2 newPos = new Vector2 (cardDistance * i, 0);
			cards [i].GetComponent<RectTransform> ().anchoredPosition = newPos;
	
		}
		distance = new float[numberOfCards];

	}


	public void ReturnToMainScreen () {

		FadeScene.instance.GoToMainScreen ();

	}
}
