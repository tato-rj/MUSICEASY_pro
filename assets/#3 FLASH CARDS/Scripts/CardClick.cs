using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardClick : MonoBehaviour {

	public void Flip () {
		StartCoroutine (FlipCard(this.gameObject));
	}

	public IEnumerator FlipCard (GameObject element) {

		element.GetComponent<Animation>().Play ("cardFlipA");
		element.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (0.16f);

		foreach (Transform child in element.transform) {
			if (child.gameObject.activeSelf) {
				child.gameObject.SetActive (false);
			} else {
				child.gameObject.SetActive (true);
			}
		}

		element.GetComponent<Animation>().Play ("cardFlipB");

	}
}
