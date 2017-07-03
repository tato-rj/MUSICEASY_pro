using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	public static InputController instance;

	public GameObject[] flatButtons;
	public GameObject[] sharpButtons;
	public GameObject[] noAccidentButtons;

	void Awake () {

		instance = this;


	}

	public void ChangeKeyboard (int index) {

		if (index == 0) {
			for (int i=0; i<2; i++) {
				flatButtons [i].SetActive (false);
				sharpButtons [i].SetActive (false);
				noAccidentButtons [i].SetActive (true);
			}	
		} else if (index == 1) {
			for (int i=0; i<2; i++) {
				flatButtons [i].SetActive (false);
				sharpButtons [i].SetActive (true);
				noAccidentButtons [i].SetActive (false);
			}	
		} else if (index == 2) {
			for (int i=0; i<2; i++) {
				flatButtons [i].SetActive (true);
				sharpButtons [i].SetActive (false);
				noAccidentButtons [i].SetActive (false);
			}	
		}

	}
}
