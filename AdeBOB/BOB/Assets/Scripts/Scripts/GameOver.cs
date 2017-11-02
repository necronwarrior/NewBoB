using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

	bool GameOverSwitch;
	// Use this for initialization
	void Start () {
		GameOverSwitch = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.FindGameObjectsWithTag ("AirBase")==null
			&&GameObject.FindGameObjectsWithTag ("Radar")==null)
		{
			GameOverSwitch=true;
		}
		if(GameOverSwitch==true)
		{
			RenderSettings.ambientLight = Color.black;

			GetComponent<Light> ().intensity -= 1;
		}
	}
}
