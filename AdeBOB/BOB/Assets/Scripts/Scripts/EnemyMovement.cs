using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {


	public AudioClip Narration;
	bool Audioonce;
	public Transform Startpos, Finishpos, Parent;
	public float GoTime, timecount;
	// Use this for initialization
	void Start () {
		transform.position = Startpos.position;
		timecount = 0.0f;
		Audioonce = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad > GoTime && GetComponent<EnemyPlane>().health >= 100.0f ) {
			transform.position = Vector3.Lerp (Startpos.position, Finishpos.position, timecount);
			timecount += (Time.deltaTime/20.0f);
			//transform.LookAt (Finishpos.position);
			if (Narration != null
				&& Audioonce == true) {
				GetComponent<AudioSource> ().PlayOneShot (Narration);
				Audioonce = false;
			}
		}
	}
}
