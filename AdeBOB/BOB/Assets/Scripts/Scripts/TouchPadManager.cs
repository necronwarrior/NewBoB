using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    Detects touch inputs that should be send to the selected object but
    they aren't because the collider for said plane just encapsulates the
    object itself.


*/
public class TouchPadManager : MonoBehaviour, ITouchReceiver {

    //Collider for this GameObject
	BoxCollider padCollider;

    //Currently selected elements
    private GameObject selectedObject;
    ITouchReceiver touchReceiver;


	// Use this for initialization
	void Start () {
        padCollider = GetComponent<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		

	public void setSelectedPlane(GameObject gameObject){
	
		selectedObject = gameObject;
		touchReceiver = selectedObject.GetComponent<ITouchReceiver> ();

	}

    public void removeSelectedPlane() {

        selectedObject = null;
        touchReceiver = null;

    }

	public void OnTouchUp(Vector3 point)
	{
        if(touchReceiver != null)
		    touchReceiver.OnTouchUp (point);
	}

	public void OnTouchDown(Vector3 point)
	{
            Debug.Log("Detected OnTouchDown on the ground and not on any object");
	}

	public void OnTouchMove(Vector3 point)
	{
        if (touchReceiver != null)
            touchReceiver.OnTouchMove (point);

	}

	public void OnTouchStay(Vector3 point)
	{
        if (touchReceiver != null)
            touchReceiver.OnTouchStay (point);

	}

	public void OnTouchExit(Vector3 point)
	{
        if (touchReceiver != null)
            touchReceiver.OnTouchExit (point);

	}


}
