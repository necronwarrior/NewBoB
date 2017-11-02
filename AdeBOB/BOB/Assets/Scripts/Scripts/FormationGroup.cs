using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FormationType { FOURFINGER, SCHAWRM, VICFORM};

public class FormationGroup : MonoBehaviour {

    //The formation of the group of planes
    public FormationType formationType;

    //The state of the formation
    public PlaneState formationState;

    //The planes and there positions
    public List<Vector3> planeOffSets;
    public List<Plane> planes;

    //The points the planes are moving through to get to 
    public List<GameObject> targetPositions;
    Vector3 targetPosition;
    bool targetingEnemy;

    //The spacing constant between the planes
    public float offset;

    //Determines formation side
    public bool leftSideFormation;

    //The Rigidbody of the plane
    Rigidbody rigBod;

    //Movement Variable
    public float maxVelocity;
    public float accelration;
    public float rotationSpeed;
    public FlightDirection flightDirection;

    //Move infromation from the drawn line
    SplineInterpolator splineInterp;
    public float distanceFeathering =1;

    //test varriables
    Vector3 directionToobject;

	// Use this for initialization
	void Start ()
    {

        rigBod = GetComponent<Rigidbody>();

        splineInterp = GetComponent<SplineInterpolator>();

        CalculateOffsets();

        SetPlanesGroup();

        formationState = PlaneState.IDLE;

    }
	
	// Update is called once per frame
	void Update () {


        CalculateOffsets();
        CopyLeadPlaneRoation();

        //Loop through planes to set the target posotion
        //if they are in the laching mode
        for(int i =1; i <  planes.Count; i++)
        {
            if(planes[i].planeState == PlaneState.FORMING)
            {
                
                planes[i].formationTarget = planeOffSets[i];
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) 
        {
            CreateBalls();
        }
        if (formationState == PlaneState.IDLE)
        {
            transform.RotateAround(FindFarRightOfFormation(), Vector3.up, -rotationSpeed);

            // Find the forward vector
            Vector3 xzDirection = transform.forward;

            //Take away the Y vector so that the plane does not move verticaly
            xzDirection.y = 0;

            //Normalise the vector to make up for taking away the Y
            xzDirection = Vector3.Normalize(xzDirection);

            //add the force
            rigBod.AddForce(xzDirection * accelration * Time.deltaTime);

            //clamp the force
            rigBod.velocity = Vector3.ClampMagnitude(rigBod.velocity, maxVelocity);
        }
        else
        {
            if (targetPositions.Count != 0)
            {
                MoveFormation();
            }
            else
            {
                formationState = PlaneState.IDLE;
            }
        }

    }

    void MoveFormation()
    {

        //Find 2D Distance to target on xz plane

        //Get 2D vectors
        Vector2 targetXZ = new Vector2(targetPositions[0].transform.position.x, targetPositions[0].transform.position.z);
        Vector2 formationXZ = new Vector2(transform.position.x, transform.position.z);

        //Get the distance
        float distanceToTarget = Vector2.Distance(targetXZ,formationXZ);

        //Check if close enough to find next point
        if(distanceToTarget <= distanceFeathering)
        {
            GetNextPoint();
        }

        float angle;

        //Find the direction of the plane
        Vector3 targetDir = targetPositions[0].transform.position - transform.position;

        directionToobject = targetDir;
        //Find the angle 
        angle = Vector3.Angle(transform.forward, targetDir);

        //Find if the angle is negtive or postive
        int sign = Vector3.Cross(transform.forward, targetDir).y < 0 ? -1 : 1;

        //Make the angle postive or negtive
        angle *= sign;

        if (angle < 0)
        {
            if(angle > -rotationSpeed * Time.deltaTime)
            {
                transform.RotateAround(FindFarRightOfFormation(), Vector3.up, angle);
            }
            else
            {
                transform.RotateAround(FindFarRightOfFormation(), Vector3.up, -rotationSpeed * Time.deltaTime);
            }
           
            flightDirection = FlightDirection.Left;

        }
        else if (angle > 0)
        {
            if (angle < -rotationSpeed * Time.deltaTime)
            {
                transform.RotateAround(FindFarRightOfFormation(), Vector3.up, angle);
            }
            else
            {
                transform.RotateAround(FindFarRightOfFormation(), Vector3.up, rotationSpeed * Time.deltaTime);
            }
            flightDirection = FlightDirection.Right;

        }
        else
        {
            flightDirection = FlightDirection.Straight;
        }


        // Find the forward vector
        Vector3 xzDirection = transform.forward; 

        //Take away the Y vector so that the plane does not move verticaly
        xzDirection.y = 0;

        //Normalise the vector to make up for taking away the Y
        xzDirection = Vector3.Normalize(xzDirection);

        //add the force
        rigBod.AddForce(xzDirection * accelration * Time.deltaTime);

        //clamp the force
        rigBod.velocity = Vector3.ClampMagnitude(rigBod.velocity, maxVelocity);

       
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1000);
        Gizmos.DrawLine(transform.position, transform.position + directionToobject * 1000);
    }
    private void GetNextPoint()
    {
        Destroy(targetPositions[0].gameObject);
        targetPositions.RemoveAt(0);
    }

    //Set A list of targets for the plane formation to follow 
    public void SetNewTargetList(List<GameObject> newList)
    {
        targetPositions = new List<GameObject>(newList);
        formationState = PlaneState.MOVING;
    }

    Vector3 FindFarRightOfFormation()
    {

        //return value
        Vector3 farRight = Vector3.zero;

        //Markers for when the right values
        //are found in the itarator
        int itaratorPosX = 0;

        //set the max and min values to first plane
        float localMaxX = planes[0].transform.localPosition.z;
        float maxZ = planes[0].transform.localPosition.x;
        float minZ = planes[0].transform.localPosition.x;

        //Loop through all the planes
        for (int i = 0; i < planes.Count; i++ )
        {
            //if plane is in formation 
            if (planes[i].planeState == PlaneState.FORMED)
            {
                //Gets max Z
                if (planes[i].transform.localPosition.x > localMaxX)
                {
                    itaratorPosX = i;
                    localMaxX = planes[i].transform.localPosition.x;
                }

                //Gets max X
                if (planes[i].transform.localPosition.z > maxZ)
                {
                    maxZ = planes[i].transform.localPosition.x;
         
                }

                //Gets min X
                if (planes[i].transform.localPosition.z < minZ)
                {
                    minZ = planes[i].transform.localPosition.z;
                }
            }
        }

        //get the middle of the x Coordiantes
        farRight.z = (minZ + maxZ) / 2;

        //set z to the constant height 
        farRight.x = planes[itaratorPosX].transform.localPosition.x;

        //tranform the point from local to world 
      
        return transform.TransformPoint(farRight); 
    }


    Vector3 FindFarLeftOfFormation()
    {

        //return value
        Vector3 farleft = Vector3.zero;

        //set the max and min values to first plane
        float localMinX = planes[0].transform.localPosition.z;
        float maxZ = planes[0].transform.localPosition.x;
        float minZ = planes[0].transform.localPosition.x;

        //Loop through all the planes
        for (int i = 0; i < planes.Count; i++)
        {
            //if plane is in formation 
            if (planes[i].planeState == PlaneState.FORMED)
            {
                //Gets max Z
                if (planes[i].transform.localPosition.x < localMinX)
                {   
                    localMinX = planes[i].transform.localPosition.x;
                }

                //Gets max X
                if (planes[i].transform.localPosition.z > maxZ)
                {
                    maxZ = planes[i].transform.localPosition.x;

                }

                //Gets min X
                if (planes[i].transform.localPosition.z < minZ)
                {
                    minZ = planes[i].transform.localPosition.z;
                }
            }
        }

        //get the middle of the x Coordiantes
        farleft.z = (minZ + maxZ) / 2;

        //set z to the constant height 
        farleft.x = localMinX;

        //tranform the point from local to world 

        return transform.TransformPoint(farleft);
    }


    public void MoveToTarget(Vector3 newTarget)
    {
        targetPosition = newTarget;

    }

	public void ActivateForming()
	{
		foreach (Plane p in planes)
		{
			p.planeState = PlaneState.FORMING;
		}
		planes [0].planeState = PlaneState.FORMED;
	}

    void SwitchFormationSide()
    {
        leftSideFormation = !leftSideFormation;

        foreach (Plane p in planes)
        {
            p.planeState = PlaneState.FORMING;
        }
    }

    void SetPlanesGroup()
    {
        foreach ( Plane p in planes)
        {
            p.formationGroup = this;
        }
    }

    void CopyLeadPlaneRoation()
    {
        foreach (Plane p in planes)
        {
            if (p.planeState == PlaneState.FORMED)
            {
                p.transform.rotation = planes[0].transform.rotation;
            }
        }
    }

    void CalculateOffsets()
    {

        switch (formationType)
        {
            case FormationType.FOURFINGER:
                //Clear the list
                planeOffSets.Clear();

                //First plane is at center
                planeOffSets.Add(planes[0].transform.position);

                if (leftSideFormation)
                {

                    Vector3 pos2;

                    //Find the displacement from the forward and right vectors of the center plane
                    pos2 = planes[0].transform.position - (planes[0].transform.right * offset);

                    //Take away the Y coordinate so it is not effected by the plane turning 
                    pos2.y = planes[0].transform.position.y;

                    //Add the position to the list 
                    planeOffSets.Add(pos2);

                    Vector3 pos3;

                    //Find the displacement from the forward and right vectors of the center plane
                    pos3 = planes[0].transform.position + (planes[0].transform.right * offset) + (-planes[0].transform.forward * offset);

                    //Take away the Y coordinate so it is not effected by the plane turning 
                    pos3.y = planes[0].transform.position.y;

                    //Add the position to the list 
                    planeOffSets.Add(pos3);

                    Vector3 pos4;

                    //Find the displacement from the forward and right vectors of the 3rd plane
                    pos4 = pos3 + (planes[0].transform.right * offset);

                    //Take away the Y coordinate so it is not effected by the plane turning 
                    pos4.y = planes[0].transform.position.y;

                    //Add the position to the list 
                    planeOffSets.Add(pos4);
                }
                else
                {
                    Vector3 pos2;

                    //Find the displacement from the forward and right vectors of the center plane
                    pos2 = planes[0].transform.position - (planes[0].transform.right * offset);

                    //Take away the Y coordinate so it is not effected by the plane turning 
                    pos2.y = planes[0].transform.position.y;

                    //Add the position to the list 
                    planeOffSets.Add(pos2);

                    Vector3 pos3;

                    //Find the displacement from the forward and right vectors of the center plane
                    pos3 = planes[0].transform.position + (planes[0].transform.right * offset) + (planes[0].transform.forward * offset);

                    //Take away the Y coordinate so it is not effected by the plane turning 
                    pos3.y = planes[0].transform.position.y;

                    //Add the position to the list 
                    planeOffSets.Add(pos3);

                    Vector3 pos4;

                    //Find the displacement from the forward and right vectors of the 3rd plane
                    pos4 = pos3 + (planes[0].transform.right * offset);

                    //Take away the Y coordinate so it is not effected by the plane turning 
                    pos4.y = planes[0].transform.position.y;

                    //Add the position to the list 
                    planeOffSets.Add(pos4);
                }

                break;

            case FormationType.VICFORM:
                {
                    //Clear the list
                    planeOffSets.Clear();

                    //First plane is at center
                    planeOffSets.Add(Vector3.zero);

                    //Find the displacement from the forward and right vectors of the center plane
                    Vector3 pos2 = planes[0].transform.position - (planes[0].transform.right * offset) + (-planes[0].transform.forward * offset);

                    //Take away the Y coordinate so it is not effected by the plane turning 
                    pos2.y = planes[0].transform.position.y;

                    //Add the position to the list 
                    planeOffSets.Add(pos2);


                    //Find the displacement from the forward and right vectors of the center plane
                    Vector3 pos3 = planes[0].transform.position + (planes[0].transform.right * offset) + (-planes[0].transform.forward * offset);

                    //Take away the Y coordinate so it is not effected by the plane turning 
                    pos3.y = planes[0].transform.position.y;

                    //Add the position to the list 
                    planeOffSets.Add(pos3);
                }


                break; 
        }

    }


    void CreateBalls()
    {
        //debug tool 

        for (int i = 0; i < planeOffSets.Count; i++)
		{
			GameObject ball = Instantiate (GameObject.CreatePrimitive (PrimitiveType.Sphere), FindFarRightOfFormation (), Quaternion.identity, planes[0].transform);
			ball.GetComponent<Renderer> ().material.color = new Color (1, 0, 0);
			ball.transform.localScale *= 300;

			//GameObject balls = Instantiate (GameObject.CreatePrimitive (PrimitiveType.Sphere), FindFarLeftOfFormation (), Quaternion.identity);
			//balls.GetComponent<Renderer> ().material.color = new Color (0, 1, 0);
		}
    }

    Vector3 ConverOffsetToWorld(Matrix4x4 offset)
    {

        Matrix4x4 positionMat;

        positionMat = offset * planes[0].transform.localToWorldMatrix.inverse;

        return positionMat.GetColumn(3);
    }

    public void EngadgePlane(Plane newPlane)
    {
        newPlane.transform.parent = this.transform;

    }
}
