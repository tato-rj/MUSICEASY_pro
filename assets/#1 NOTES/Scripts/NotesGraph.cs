using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;
using System.Linq;
using AppAdvisory.SharingSystem;

public class NotesGraph : MonoBehaviour {

	public GameObject emptyGraph;
	public WMG_Axis_Graph graph;
	public WMG_Series results;
	public Text averageAccuracyText;
	public Text minAccuracyText;
	public Text maxAccuracyText;
	public Text numOfGames;
	public Image ring;

	private List<int> resultsData;
	private List<Vector2> resultsList;
	//private List<int> sortAccuracyList;
	private float averageAccuracy;
	private float count;
	private bool screenshotExists;

	// Use this for initialization
	void Start () {

		NotesGameController.instance.LoadRecords ();
		screenshotExists = false;
		resultsData = new List<int> ();
		resultsData = NotesGameController.instance.tempNoteAccuracyRecords;
		averageAccuracy = (float)NotesGameController.instance.tempNoteAccuracyRecords.Average ();


		GameObject graphGo = GameObject.Instantiate (emptyGraph);
		graphGo.transform.SetParent (this.transform, false);
		graph = graphGo.GetComponent<WMG_Axis_Graph> ();
		graph.xAxis.hideLabels = true;
		graph.xAxis.hideTicks = true;
		graph.xAxis.hideGrid = true;
		graph.xAxis.HideAxisArrowTopRight = true;
		graph.yAxis.HideAxisArrowTopRight = true;
		graph.tooltipEnabled = false;
		//graph.toolTipLabel.GetComponent<Text>().fontSize = 26;
		graph.transform.Find ("Background").transform.Find("Anchored").gameObject.SetActive (false);

		resultsList = new List<Vector2> ();
		for (int i=0; i<resultsData.Count; i++) {
			Vector2 graphPoint = new Vector2 (i, resultsData[i]);
			resultsList.Add (graphPoint);
		}

		graph.yAxis.AxisMaxValue = GetMax(resultsData);

		results = graph.addSeries ();
		results.pointValues.SetList (resultsList);
		results.UseXDistBetweenToSpace = true;

		results.pointWidthHeight = 24f;
		results.lineScale = 1f;
		results.pointColor = new Color32 (244,67,54,255);
		results.lineColor = new Color32 (200,55,44, 255);

	
		numOfGames.text = NotesGameController.instance.tempNoteAccuracyRecords.Count.ToString();
		maxAccuracyText.text = GetMax (NotesGameController.instance.tempNoteAccuracyRecords).ToString()+"%";
		minAccuracyText.text = GetMin (NotesGameController.instance.tempNoteAccuracyRecords).ToString()+"%";


	}
		
	private void TakeScreenshot () {
		VSSHARE.DOTakeScreenShot ();
		screenshotExists = true;
	}

	void Update () {

		if (gameObject.activeSelf) {
			if (ring.fillAmount < averageAccuracy / 100f) {
				ring.fillAmount += Time.deltaTime / 2f;
				averageAccuracyText.text = Mathf.FloorToInt(ring.fillAmount * 100).ToString () + "%";				
			} else if (!screenshotExists) {
				TakeScreenshot ();
			}
		} else {
			ring.fillAmount = 0;
			averageAccuracyText.text = "0%";		
		}
	}

	int GetMax (List<int> list) {
		List<int> newList = new List<int>();
		newList = list;
		newList.Sort();
		int max = newList [newList.Count - 1];
		return max;
	}
	int GetMin (List<int> list) {
		List<int> newList = new List<int>();
		newList = list;
		newList.Sort();
		int min = list [0];
		return min;
	}
}
