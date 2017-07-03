using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleSound : MonoBehaviour {

	public void PlaySound () {
		GetComponent<AudioSource> ().Play();
	}

}
