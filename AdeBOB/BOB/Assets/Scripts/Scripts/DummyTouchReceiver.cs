using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTouchReceiver : MonoBehaviour, ITouchReceiver
{

	public GameObject SelectedObject;

    //Prefab
    private UnityEngine.Object spherePrefab;

    void Start() {

        spherePrefab = Resources.Load("Prefabs/TestSphere");

    }

	public void SetSelectedObject(GameObject SEl)
	{
		SelectedObject = SEl;
	}

    public void OnTouchUp(Vector3 point)
    {
		if(SelectedObject!=null && SelectedObject!=transform.gameObject)
		SelectedObject.SendMessage ("OnTouchUp", point, SendMessageOptions.DontRequireReceiver);
		SelectedObject = null;
    }

    public void OnTouchDown(Vector3 point)
    {
		//SelectedObject = transform.gameObject;
		Debug.Log ("hit");
    }

    public void OnTouchMove(Vector3 point)
    {
		if(SelectedObject!=null&& SelectedObject!=transform.gameObject)
		SelectedObject.SendMessage ("OnTouchMove", point, SendMessageOptions.DontRequireReceiver);
    }

    public void OnTouchStay(Vector3 point)
    {
		if(SelectedObject!=null)
		SelectedObject.SendMessage ("OnTouchStay", point, SendMessageOptions.DontRequireReceiver);
    }

    public void OnTouchExit(Vector3 point)
    {
		if(SelectedObject!=null)
		SelectedObject.SendMessage ("OnTouchExit", point, SendMessageOptions.DontRequireReceiver);
    }



}
