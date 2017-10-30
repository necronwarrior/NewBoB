using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRecieveTest : MonoBehaviour, ITouchReceiver {

	public TouchPadManager touchPadManager;
	public Transform TempParent;
	Object HoldingPattern;
	GameObject CurrentTouch, splineRoot;

	void Start (){
		HoldingPattern = Resources.Load ("Aeroplanes/HoldingPattern");
		splineRoot = GetComponent<SplineController> ().SplineRoot;
	}

	public void OnTouchDown(Vector3 point){


		touchPadManager.setSelectedPlane (this.transform.gameObject);

		transform.GetComponent<BoxCollider> ().bounds.size.Set(100.0f, 1.0f, 100.0f);		//Delete all children
		foreach (Transform child in splineRoot.transform) {
			GameObject.Destroy(child.gameObject);
		}
		//Instantiate holding pattern
		GameObject G = (GameObject)Instantiate(HoldingPattern, transform);
		GetComponent<SplineController> ().SplineRoot= G;
		GetComponent<SplineController> ().RestartSpline (4.0f);
		//Begin creating splinepath
		CurrentTouch = (GameObject)Instantiate((Object)new GameObject("Start"), splineRoot.transform);



	}

	public void OnTouchUp(Vector3 point){

		transform.GetComponent<BoxCollider> ().bounds.size.Set(1.0f, 1.0f, 1.0f);		//Delete all children


		foreach (Transform child in splineRoot.transform) {
			GameObject.Destroy(child.gameObject);
		}

		Instantiate((Object)new GameObject("End"), splineRoot.transform);

		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}

		GetComponent<SplineController> ().SplineRoot = splineRoot;
		GetComponent<SplineController> ().RestartSpline ((float)transform.childCount / 2.0f);
	}

	public void OnTouchMove(Vector3 point){
		float XDiff, YDiff;
		XDiff = point.x - CurrentTouch.transform.position.x;
		YDiff = point.y - CurrentTouch.transform.position.y;
		if ((XDiff*XDiff) + (YDiff*YDiff) >10.0f){
			CurrentTouch = (GameObject)Instantiate((Object)new GameObject("Point"), splineRoot.transform);
		}
	}

	public void OnTouchStay(Vector3 point){

	}

	public void OnTouchExit(Vector3 point){
		foreach (Transform child in TempParent) {
			GameObject.Destroy(child.gameObject);
		}
	}
}
