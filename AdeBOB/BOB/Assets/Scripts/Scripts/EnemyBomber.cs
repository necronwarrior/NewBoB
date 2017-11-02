using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : Plane {

    bool deliverPayLoad;
    public GameObject targetObject;
    public float bombDamage = 34;
	public float swerve, swervecount, rotation;
	public Vector3 tempswerve, LocalRight;

    new void Start()
    {
		rotation = -0.4f;
		swervecount = -0.5f;

		swerve = 0;

        base.Start();

        targetLocation = transform.position;

        FindPriorityTarget();
    }

    void FindPriorityTarget()
    {
			//find random number to decided what type of object is attacked
			int randomType = Random.Range (0, 2);

			if (randomType == 0) {
				//if airbases find a rnadom airbase
				GameObject[] airBases = GameObject.FindGameObjectsWithTag ("AirBase");
				int randomBase = Random.Range (0, airBases.Length);

				targetObject = airBases [randomBase];

			} else {
				//if radars find a rnadom radar
				GameObject[] radars = GameObject.FindGameObjectsWithTag ("Radar");
				int randomRadar = Random.Range (0, radars.Length);

				targetObject = radars [randomRadar];
			}
		if (targetObject == null) {
			FindPriorityTarget ();
		}
	
        planeState = PlaneState.CHASING;
    }

    public override FlightDirection IdleState()
    {
        
        if(deliverPayLoad)
        {
            deliverPayLoad = false;
            health.currentHealth = health.fullHealth;
        }
        else
        {
            FindPriorityTarget();
        }

        return FlightDirection.Right;
    }

    public override FlightDirection ChasingState()
    {

		if (targetObject == null) {
			planeState = PlaneState.IDLE;
		}

        FlightDirection flightDirec;

        float angle = AngleOffTargert(targetObject.transform.position);

        float targetDistance = Vector3.Distance(transform.position, targetObject.transform.position);

        //if the target is close enough 
        if (targetDistance < shotDistance)
        {
            DropBomb();

            flightDirec = FlightDirection.Straight;

            //possible change 
            planeState = PlaneState.MOVING;
        }

        //rotate the plane
        flightDirec = FlyRotateDirection(angle);

        //get the direction on the XY plane 
        Vector3 xzDirection = transform.forward;

        xzDirection.y = 0;

        xzDirection = Vector3.Normalize(xzDirection);

        //Add the forward force
        rigBod.AddForce(xzDirection * accelration * Time.deltaTime);

		LocalRight = Vector3.Cross (xzDirection, Vector3.up);

		rigBod.AddForce(LocalRight*swerve);


		tempswerve = (LocalRight*swerve);

		transform.Rotate (new Vector3 (0.0f,0.0f,rotation));
		swerve+= swervecount;
		if (swerve>25)
		{
			swervecount = -0.5f;
			rotation = -0.4f;
		}
		if (swerve < -25) 
		{
			swervecount = 0.5f;
			rotation = 0.4f;
		}
        return flightDirec;
    }

    private void DropBomb()
    {

        targetObject.GetComponent<HealthBar>().TakeDamage(bombDamage);
        deliverPayLoad = true;
    }

}
