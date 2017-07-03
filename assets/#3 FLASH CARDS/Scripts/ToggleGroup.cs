using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGroup : MonoBehaviour {

	public void ToggleChildren (GameObject group) {

		bool state = GetComponent<Toggle> ().isOn;

		foreach (Toggle child in group.GetComponentsInChildren<Toggle>()) {
			child.isOn = state;
		}
	
	}

	public void ToggleParent (GameObject group) {

		bool allOff = true;

		foreach (Toggle child in group.GetComponentsInChildren<Toggle>()) {
			if (child.isOn) {
				allOff = false;
			}
		}

		if (allOff) {
			GetComponent<Toggle> ().isOn = false;
		} else {
			GetComponent<Toggle> ().isOn = true;
		}

	}

}
