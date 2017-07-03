using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppAdvisory.SharingSystem;

public class ScreenShot : MonoBehaviour {

	void Start () {
		VSSHARE.DOTakeScreenShot ();
	}

	public void TakeShot () {
		VSSHARE.DOTakeScreenShot ();
	}

	public void ShowShot () {
		VSSHARE.DOOpenScreenshotButton ();
	}

	public void HideShot () {
		VSSHARE.DOHideScreenshotIcon ();
	}

	public void Share () {
		VSSHARE.DOOnclickedOnIconScreenshot ();
	}

	public IEnumerator Sequence () {
		VSSHARE.DOTakeScreenShot();
		yield return new WaitForSeconds (0.4f);
		VSSHARE.DOOpenScreenshotButton ();
		yield return new WaitForSeconds (1f);
		VSSHARE.DOOnclickedOnIconScreenshot ();
	}
}
