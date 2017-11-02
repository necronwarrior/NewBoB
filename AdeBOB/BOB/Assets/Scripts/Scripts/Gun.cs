using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    bool britishPlane;
    public bool ready2Fire = true;
    public float shotRate;
    public bool bulletDamage;
    private float shotTimer = 0;
    public GameObject bullet;
	public AudioClip[] Gunfire;

    // Use this for initialization
    void Start ()
    {

        britishPlane = transform.parent.GetComponent<Plane>().britishPlane;
        shotRate = transform.parent.GetComponent<Plane>().shotSpeed;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!ready2Fire)
        {
            shotTimer += Time.deltaTime;

            if (shotTimer > shotRate)
            {
                ready2Fire = true;

                shotTimer -= shotRate;
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if(ready2Fire)
        {
            Debug.Log(("hit somthing"));
            if (col.GetComponent<Plane>())
            {
                Debug.Log(("detect plane"));
                if(col.GetComponent<Plane>().britishPlane != britishPlane)
                {
                    FireBullet();
                }
            }
        }
    }

    void FireBullet()
    {
        GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
        newBullet.GetComponent<BulletScript>().britishBullet = britishPlane;

        Debug.Log("Bullet Fired");
		GetComponent<AudioSource>().PlayOneShot(Gunfire[Random.Range(0,4)]);
    }


}
