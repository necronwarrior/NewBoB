using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour {


    public Plane selectedPlane;

    public bool controllingFormation = false;

    public FormationGroup selectedGroup;

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
      
        //if left click to attempt to select
        //cast ray to see what the mouse is over
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                //if the player has clicked on a plane
                if (hit.collider.GetComponent<Plane>())
                {
                                   
                    selectedPlane = hit.collider.GetComponent<Plane>();

                    //check if that plane is part of a grouped formation 
                    if (selectedPlane.planeState == PlaneState.FORMED || selectedPlane.planeState == PlaneState.FORMING)
                    {

                        Debug.Log("formation selected");

                        selectedGroup = selectedPlane.formationGroup;

                        controllingFormation = true;
                    }
                    else
                    {
                        controllingFormation = false;
                    }

                   
                }
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                //if hit a unit attack that unit
                if (hit.collider.GetComponent<Plane>())
                {
                    selectedPlane.SetTargetPlane(hit.collider.GetComponent<Plane>());
                }
                else if (hit.collider.name == "Floor")
                {
                    if (controllingFormation)
                    {
                        selectedGroup.MoveToTarget(hit.point);
                    }
                    else
                    {
                        selectedPlane.SetTargetPostion(hit.point);
                    }
                }
              
            }
        }
    }
}
