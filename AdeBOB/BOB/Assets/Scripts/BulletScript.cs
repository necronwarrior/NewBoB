using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    Rigidbody rigBod;

    public bool britishBullet;

    public float bulletVelocity;

    public float bulletTime = 4;
    float timer = 0;


	// Use this for initialization
	void Start () {
        rigBod = GetComponent<Rigidbody>();

        rigBod.AddForce(transform.forward * bulletVelocity);

	}
	
	// Update is called once per frame
	void Update () {
		

        //increase time 
        if(timer < bulletTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            //destroy object 
            Destroy(gameObject);
        }

	}
}
