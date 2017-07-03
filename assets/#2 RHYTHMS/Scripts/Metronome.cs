using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Metronome : MonoBehaviour {

	public static Metronome instance;

	public GameObject metronome;
	public Text count;
	public Slider speedSlider;
	public float speed;

	void Awake () {

		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
		}
		speed = speedSlider.GetComponent<Slider>().value;

	}

	public void UpdateSpeed () {

		speed = speedSlider.GetComponent<Slider>().value;

	}
		

	public float SpeedRate () {

		return 60f / speed;

	}

}
