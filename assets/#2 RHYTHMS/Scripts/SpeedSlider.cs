using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour {

	public GameObject turtle;
	public GameObject rabbit;

	void Start () {
	
		AnimateAnimals ();
	
	}

	public void AnimateAnimals () {

		float alpha = (GetComponent<Slider> ().value / 100 - 0.4f) * 2.4f;

		turtle.GetComponent<Image> ().color = new Color (1,1,1, (1 - alpha));
		rabbit.GetComponent<Image> ().color = new Color (1,1,1, (0.05f + alpha));

	}

}
