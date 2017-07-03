using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesSounds : MonoBehaviour {

	public static NotesSounds instance;
	public List<AudioSource> noteSounds;


	void Awake () {

		instance = this;
		foreach (AudioSource sound in gameObject.GetComponents<AudioSource>()) {
			noteSounds.Add (sound);
		} 
	
	}

	public void PlayNoteSound (string name) {
		//Looks at all sounds in the list
		for (int i = 0; i < noteSounds.Count; i++) {
			//If that sound is the correct one, play it
			if (noteSounds[i].clip.name == name) {
				noteSounds[i].Play ();
			}
		}
	}
}
