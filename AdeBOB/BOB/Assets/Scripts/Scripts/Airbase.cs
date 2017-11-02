using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airbase : MonoBehaviour {


	public HealthBar Health;

	bool Destroyed;

	// Use this for initialization
	void Start () {
		Health = GetComponent<HealthBar> ();
		Destroyed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Destroyed == false) {
				
		}
	}
}
