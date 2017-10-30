using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangar : MonoBehaviour {

    //The state of the hangar, pretty self-explanatory
    public enum HangarState {
        PLANE_COMING_IN,
        PLANE_INSIDE,
        PLANE_COMING_OUT,
        IDLE
    }

    //Audio variables
    public AudioClip repairingSound;
    public AudioSource repairingSource;

    // The current state of the hangar
    [SerializeField]
    private HangarState currentState = HangarState.IDLE;

    // The necessary distance for the plane to start landing
    public float distanceToStartLanding = 1.0f;

    // The distance at which we consider the plane to be close enough
    public float planeIsInsideThreshold = 0.01f;

    // The target position for the landing 
    private Vector3 targetPosition;

    // The gameobject of the plane that will land next
    [SerializeField]
    private GameObject planeComingToHangar = null;

    // Data about the plane before landing
    private Vector3 originalPlaneScale = Vector3.one;

    // = REPAIR STUFF = 
    // Total time to repair in seconds
    public float timeToRepair = 3.0f;

    // Current time left for repairs
    private float timeLeftRepairing = 0.0f;

    // Landing Speed for the plane when it comes in and when it goes out
    public float landingSpeed = 10.0f;


    // Timing stuff so it takes the same time to take off and to land
    private float timeToLand = 0.0f;
    private float currentTimeTakingOff = 0.0f;

    void Awake() {
        planeComingToHangar = null;
    }

	// Use this for initialization
	void Start () {
        targetPosition = this.transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        // If we don't have a plane or something has happened to it (was destroyed)
        // We set the state as idle and we have nothing else to do. 
        if (planeComingToHangar == null) {
            currentState = HangarState.IDLE;
            return;
        }



        Vector3 deltaPosition = targetPosition - planeComingToHangar.transform.position;

        float currentDistanceToPlane = deltaPosition.magnitude;

        // We get how close we are to the "landing zone" from 0 to 1
        float normalizedDistance = (currentDistanceToPlane) / distanceToStartLanding;


        // Just in case, you never know with floating point stuff, C# and Unity
        // all trying to make something explode at the same time :)
        normalizedDistance = Mathf.Clamp(normalizedDistance, 0.0f, 1.0f);


        switch (currentState)
        {
            case HangarState.PLANE_COMING_IN:

                if (currentDistanceToPlane <= planeIsInsideThreshold)
                { //Yeah, I know this cannot be lower than Zero
                    //If the plane is close enough then we set the state as inside and we stop rendering it

                    this.currentState = HangarState.PLANE_INSIDE;
                    this.timeLeftRepairing = timeToRepair;
                    this.planeComingToHangar.GetComponentInChildren<Renderer>().enabled = false;
                    planeComingToHangar.GetComponent<PlaneTouchReciever>().DestroyTrail();

                    repairingSource.PlayOneShot(repairingSound, 1.0f);

                }

                // We move it and point to to the right direction
                planeComingToHangar.transform.position = Vector3.Lerp(planeComingToHangar.transform.position, targetPosition, Time.deltaTime * landingSpeed);
                planeComingToHangar.transform.LookAt(this.transform);

                //  We scale the plane to make it seem like it's going down
                planeComingToHangar.transform.localScale = Vector3.Lerp(this.originalPlaneScale, Vector3.zero, 1.0f - normalizedDistance);

                timeToLand += Time.deltaTime;

                break;

            case HangarState.PLANE_INSIDE:
                timeLeftRepairing -= Time.deltaTime;
                if (timeLeftRepairing <= 0.0f) { 
                    //We have "repaired" the plane
                    this.planeComingToHangar.GetComponent<AllyPlane>().Repair();
                    
                    //We make it visible
                    this.planeComingToHangar.GetComponentInChildren<Renderer>().enabled = true;

                    //And the plane can come out again
                    this.currentState = HangarState.PLANE_COMING_OUT;
                    currentTimeTakingOff = 0.0f;
					
                    //We now make it move in the original circle 
                    planeComingToHangar.GetComponent<PlaneTouchReciever>().ActivateHoldingPattern();
                    planeComingToHangar.GetComponent<SplineInterpolator>().enabled = true;
                }

                break;

            case HangarState.PLANE_COMING_OUT:


                currentTimeTakingOff += Time.deltaTime;

                planeComingToHangar.transform.localScale = Vector3.Lerp(Vector3.zero, originalPlaneScale, currentTimeTakingOff/timeToLand );

                if (currentTimeTakingOff > timeToLand)
                { //The plane just left completely from the hangar

                    //And we come back to our default state
                    ResetPlaneComingToHangar();
                    currentState = HangarState.IDLE;

                    //@@ TODO: See if we reach this state correctly and why can't we select the plane after taking off (also the null reference exception in 143)
                }

                break;


            case HangarState.IDLE:
            default:
                if (currentDistanceToPlane < distanceToStartLanding)
                {
                    currentState = HangarState.PLANE_COMING_IN;
                    timeToLand += 0.0f;
                    this.originalPlaneScale = planeComingToHangar.transform.localScale;
                    planeComingToHangar.GetComponent<SplineInterpolator>().enabled = false;

                }
                break;
        }
        
		
	}


    public void SetPlaneComingToHangar(GameObject planeObject) {
        this.planeComingToHangar = planeObject;
        targetPosition.z = planeComingToHangar.transform.position.z;
    }

    public void ResetPlaneComingToHangar() {
        this.planeComingToHangar = null;
    }


}
