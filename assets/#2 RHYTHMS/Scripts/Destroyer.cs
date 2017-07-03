using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	public GameObject lastLevel;

	void OnTriggerEnter2D (Collider2D other) {

		if (other.name == "End") {
			FadeScene.instance.GoToRhythmsGameResults ();
		}

	}

}
