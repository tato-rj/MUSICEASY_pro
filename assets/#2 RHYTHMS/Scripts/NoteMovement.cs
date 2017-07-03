using UnityEngine;
using System.Collections;

public class NoteMovement : MonoBehaviour {

	public static NoteMovement instance;
	//private float xPos;

	void Start () {

		instance = this;
		//xPos = 25f;

	}

	void Update () {

		if (RhythmsGameController.instance.gameOn) {
			transform.position = Vector3.Lerp (
				transform.position, 
				new Vector3 (RhythmsGameController.instance.startX, RhythmsGameController.instance.startY, 0), 
				1f
			);
			RhythmsGameController.instance.startX -= Time.deltaTime * (Metronome.instance.speed *2/40f);
		}
	}
}
