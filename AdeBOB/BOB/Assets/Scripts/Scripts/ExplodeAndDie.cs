using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAndDie : MonoBehaviour {

    private ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {

        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
        Destroy(gameObject, 2.0f);

	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
