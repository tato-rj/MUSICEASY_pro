using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighlightSelected : MonoBehaviour {
	
	private Color32 inactive = new Color32(82, 82, 82, 80);
	private Color32 active = new Color32(82, 82, 82, 255);

	public void Highlight () {

		if (GetComponent<UnityEngine.UI.Toggle> ().isOn) {
			GetComponentInChildren<Text> ().color = active;
		} else {
			GetComponentInChildren<Text> ().color = inactive;
		}
			
	}
		
}
