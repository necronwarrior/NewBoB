using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickLookAt : MonoBehaviour {

	public GameObject LookAtObj;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (LookAtObj.transform);
	}
}
