using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogFight : MonoBehaviour {

    private UnityEngine.Object spherePrefab;


    public Vector3 dogfightCenter;
    private bool dogFighting = false;
	private SphereCollider rangeCollider;


    //Dancing Stuff
    public float fightingSpeed = 1.0f;
    public float currentTime = 0.0f;


    //OtherPlane
    GameObject otherPlaneGameObject;

    //PlaneComponents
    GeneralPlane enemyPlaneComponent;
    GeneralPlane myPlaneComponent;

    //Audio
    public AudioClip gunfire_1;
    public AudioClip gunfire_2;
    public AudioSource gunfireSource;

    public AudioClip impact_1;
    public AudioClip impact_2;
    public AudioSource impactSource;


    public AudioClip explosionClip;
    public AudioSource explosionSource;


    void Awake() {

        spherePrefab = Resources.Load("Prefabs/TestSphere");

        rangeCollider = GetComponentInChildren<SphereCollider>();
        
		Debug.Log (rangeCollider);

    }

	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {

        if (dogFighting) {
            if (otherPlaneGameObject == null) {
                EndDogfight();
            }
            Dance();
            PlaySounds();

        }

	}

    void PlaySounds() {

        if (!gunfireSource.isPlaying) {
            if (((int)Random.Range(0.0f, 100.0f)) % 2 == 0) {
                gunfireSource.PlayOneShot(gunfire_1, 1.0f);
            }
            else {
                gunfireSource.PlayOneShot(gunfire_2, 1.0f);
            }
        }

        if (!impactSource.isPlaying)
        {
            if (((int)Random.Range(0.0f, 100.0f)) % 2 == 0)
            {
                impactSource.PlayOneShot(impact_1, 1.0f);
            }
            else {
                impactSource.PlayOneShot(impact_2, 1.0f);
            }
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if (dogFighting) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyPlane"))
        {
			GetComponent<SplineInterpolator> ().enabled = false;
            StartDogfight(other);

        }

    }

    void OnTriggerExit(Collider other)
    {

        /*
        if (!dogFighting) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyPlane"))
        {

            EndDogfight();
        }
        */

    }


    void StartDogfight(Collider other) {

		if (GetComponent<PlaneTouchReciever> ().TrailTouch!=null)
		GameObject.Destroy (GetComponent<PlaneTouchReciever> ().TrailTouch);

        Debug.Log("Dogfighting");

        dogFighting = true;

        dogfightCenter = transform.position + ((other.gameObject.transform.position - transform.position) / 2.0f);

        //dogfightCenter = transform.TransformPoint(dogfightCenter);

       // GameObject newSphere = (GameObject)Instantiate(spherePrefab, transform);
       // newSphere.transform.position = dogfightCenter;

        this.rangeCollider.radius = this.rangeCollider.radius * 2.0f;

        otherPlaneGameObject = other.gameObject;

        GeneralPlane enemyPlaneComponent = otherPlaneGameObject.GetComponent<EnemyPlane>();
        GeneralPlane myPlaneComponent = GetComponent<AllyPlane>();


        enemyPlaneComponent.StartDealingDamage(myPlaneComponent);
        myPlaneComponent.StartDealingDamage(enemyPlaneComponent);


    }

    void EndDogfight() {

        explosionSource.PlayOneShot(explosionClip, 1.0f);

        dogFighting = false;

        this.rangeCollider.radius = this.rangeCollider.radius / 2.0f;

        otherPlaneGameObject = null;

		if (enemyPlaneComponent != null) {
			enemyPlaneComponent.StopDealingDamage ();
		}
		else {

			GetComponent<PlaneTouchReciever> ().ActivateHoldingPattern ();
		}

        if (myPlaneComponent != null)
            myPlaneComponent.StopDealingDamage();


    }

    void Dance() {

        currentTime += Time.deltaTime;

        Vector3 noiseAxis = new Vector3(
            Mathf.PerlinNoise(currentTime / 3.0f , 0.0f),
            Mathf.PerlinNoise((currentTime+50.0f) / 3.0f, 0.0f),
            Mathf.PerlinNoise((currentTime+100.0f) / 3.0f, 0.0f));
        
        noiseAxis.Scale(new Vector3(2.0f, 2.0f, 2.0f));
        noiseAxis -= new Vector3(1.0f, 1.0f, 1.0f);
        noiseAxis.Normalize();


        Vector3 noiseAxis2 = new Vector3(
            Mathf.PerlinNoise((currentTime + 100.0f) / 3.0f, 0.0f),
            Mathf.PerlinNoise((currentTime + 10.0f) / 3.0f, 0.0f),
            Mathf.PerlinNoise((currentTime + 200.0f) / 3.0f, 0.0f));

        noiseAxis2.Scale(new Vector3(2.0f, 2.0f, 2.0f));
        noiseAxis2 -= new Vector3(1.0f, 1.0f, 1.0f);
        noiseAxis2.Normalize();

        {
            Vector3 previousPosition = transform.position;
            transform.RotateAround(dogfightCenter, noiseAxis, fightingSpeed * Time.deltaTime);
            Vector3 deltaPosition = transform.position - previousPosition;
            transform.LookAt(transform.position + deltaPosition);
        }

        if (otherPlaneGameObject != null)
        {

            Vector3 otherPreviousPosition = otherPlaneGameObject.transform.position;
            otherPlaneGameObject.transform.RotateAround(dogfightCenter, noiseAxis2, fightingSpeed * Time.deltaTime);
            Vector3 otherDeltaPosition = otherPlaneGameObject.transform.position - otherPreviousPosition;
            otherPlaneGameObject.transform.LookAt(otherPlaneGameObject.transform.position + otherDeltaPosition);

        }
        

    }

}
