using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTouchReciever : MonoBehaviour, ITouchReceiver {

	public GameObject Map;

	public List<GameObject> NodeList;

	public float distanceBetweenObjects = 10.0f;

	public GameObject TrailTouch;

	public float TrailTime;

	private Vector3 lastPointPosition;

	//Prefab
	private UnityEngine.Object TrailPrefab, spherePrefab ;

    // Hangar detecting stuff
    public LayerMask hangarLayerMask;



	void Start() {

		TrailPrefab = Resources.Load("Prefabs/Touchtrail");
		spherePrefab = Resources.Load("Prefabs/Node");

		//ActivateHoldingPattern ();
	}
		
	void Update(){



	}
       
	public void OnTouchUp(Vector3 point)
	{
		//transform.GetChild (0).GetComponent<Rigidbody> ().isKinematic = true;




        // Detecting if we have finished the line on top of a hangar
        Ray ray = new Ray(point, new Vector3(0.0f, 0.0f, 1.0f));
        RaycastHit touchHit;

		GetComponent<FormationGroup> ().SetNewTargetList (NodeList);
		NodeList.Clear();

    }

	public void OnTouchDown(Vector3 point)
	{


		Map.SendMessage("SetSelectedObject", transform.gameObject, SendMessageOptions.DontRequireReceiver);

		//ActivateHoldingPattern ();
		DestroyTrail ();


		TrailTouch = (GameObject)Instantiate (TrailPrefab);
		TrailTouch.transform.parent = transform.parent.transform.parent;
		TrailTouch.GetComponent<TrailRenderer> ().time = Mathf.Infinity;
		TrailTouch.GetComponent<TrailRenderer> ().widthMultiplier = 1f;
		TrailTouch.GetComponent<TrailRenderer> ().alignment = LineAlignment.View;
		TrailTouch.GetComponent<TrailRenderer> ().textureMode = LineTextureMode.RepeatPerSegment;
		TrailTouch.transform.position =  new Vector3(point.x, 5.0f, point.z);

		TrailTime = 0.0f;

		generatePoint (point);
		//Map.SendMessage ("OnTouchDown", point, SendMessageOptions.DontRequireReceiver);
	}

	public void OnTouchMove(Vector3 point)
	{
		point = new Vector3(point.x, 10.0f, point.z);
		//Debug.Log ((point - lastPointPosition).magnitude);
		if ((point - lastPointPosition).magnitude >= distanceBetweenObjects)
		{
			//Debug.Log((point - lastPointPosition).magnitude);
			generatePoint(point);
		}

		if (TrailTouch!=null)
			TrailTouch.transform.position = new Vector3(point.x, 5.0f, point.z);
	}

	public void OnTouchStay(Vector3 point)
	{
		
	}

	public void OnTouchExit(Vector3 point)
	{
	}
		

	private void generatePoint(Vector3 position) {

		GameObject Node = (GameObject)Instantiate(spherePrefab);
		Node.transform.position = position;

		NodeList.Add (Node);
		if (NodeList.Count > 1)
			Node.GetComponent<FrontNBack> ().Back = (Node.transform.position - NodeList [NodeList.Count - 2].transform.position).magnitude;
		
		lastPointPosition = Node.transform.position;
	}

	public void ActivateHoldingPattern()
	{
		/*int i= 0;
		foreach (Transform child in transform) {
			child.GetComponent<Plane> ().planeState = PlaneState.IDLE;
			i++;
		}

		for(int c = i-1; c>0; c--)
		{
			transform.GetChild (c).parent = null;
		}*/

		//Legacy



	}

	public void DestroyTrail(){
		if (TrailTouch != null)
			GameObject.Destroy (TrailTouch);
	}
}
