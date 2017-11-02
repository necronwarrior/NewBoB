using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour {

	MeshCollider Cone;
	public GameObject Plane;

	// Use this for initialization
	void Start () {
		Cone = GetComponent<MeshCollider> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Enemy")
		{
			collision.gameObject.GetComponent<SpringJoint> ().connectedBody = Plane.GetComponent<Rigidbody> ();
			Plane.GetComponent<SplineInterpolator> ().enabled = false;
			Plane.GetComponent<SplineController> ().enabled = false;
		}
	}
}
