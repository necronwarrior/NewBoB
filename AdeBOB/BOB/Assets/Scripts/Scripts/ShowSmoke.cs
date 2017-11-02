using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSmoke : MonoBehaviour {

    public float smokeHealthThreshold = 100.0f;

    private AllyPlane allyPlaneScript;
    private ParticleSystem particleSystem;
    private bool smokin;

    void Awake() {
        allyPlaneScript = transform.parent.GetComponent<AllyPlane>();
        particleSystem = GetComponent<ParticleSystem>();
        smokin = false;
    }

	// Use this for initialization
	void Start () {
        particleSystem.Stop();

    }
	
	// Update is called once per frame
	void Update () {

        bool shouldBeSmokin = (allyPlaneScript.health < smokeHealthThreshold);

        if (smokin != shouldBeSmokin) {

            smokin = shouldBeSmokin;

            if (smokin) {
                particleSystem.Play();
            }
            else {
                particleSystem.Stop();
            }

        }
        
		
	}
}
