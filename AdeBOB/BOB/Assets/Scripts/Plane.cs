using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlaneState { IDLE, MOVING, CHASING, FORMING, FORMED};

public class Plane : MonoBehaviour
{

    public bool britishPlane;
    public Vector3 targetLocation;
    public Plane targetPlane;
    public float speed;
    public float accelration;
    public float rotationSpeed;
    public PlaneState planeState;

    Rigidbody rigBod;
    
    public float shotDistance;
    public bool ready2Fire = true;
    public float shotSpeed;
    private float shotTimer = 0;
    public GameObject bullet;

    public HealthBar health;

    AudioSource audioSource;



    //formation information 
    public bool formationFlying;
    public FormationGroup formationGroup;
    public Vector3 formationTarget;



    // Use this for initialization
    void Start() {

        rigBod = GetComponent<Rigidbody>();
        health = GetComponent<HealthBar>();
        transform.Find("Health Bar").parent = null;
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update() {


        //for tiliting plane after finding what direction its moving
        bool flyLeft = false;
        bool flyRight = false;
        bool flyStraight = false;

        
        float targetAngle = 0;
        //angle that plane is flying from its target
        float angle;

        switch (planeState)
        {
            case PlaneState.IDLE:
                {

                    transform.Rotate(Vector3.up, rotationSpeed / 4 * Time.deltaTime);
                    rigBod.AddForce(transform.forward * accelration * Time.deltaTime);

                    break;
                }
            case PlaneState.CHASING:
                {

                    if (targetPlane == null)
                    {
                        planeState = PlaneState.IDLE;

                        break;
                    }


                    angle = AngleOffTargert(targetPlane.transform.position);

                    float targetDistance = Vector3.Distance(transform.position, targetPlane.transform.position);

                    if (targetAngle == 0 && targetDistance < shotDistance)
                    {
                        FireBullet();

                        flyStraight = true;
                    }

                    //rotate the plane
                    if (angle > targetAngle)
                    {
                        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                        flyRight = true;
                    }
                    else if (angle < targetAngle)
                    {
                        transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
                        flyLeft = true;
                    }
                    else
                    {
                        flyStraight = true;
                    }

                    Vector3 xzDirection = transform.forward;

                    xzDirection.y = 0;

                    xzDirection = Vector3.Normalize(xzDirection);

                    rigBod.AddForce(xzDirection * accelration * Time.deltaTime);


                    break;
                }
            case PlaneState.MOVING:
                {

                    angle = AngleOffTargert(targetLocation);

                    //rotate the plane
                    if (angle > targetAngle)
                    {
                        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

                        flyRight = true;

                    }
                    else if (angle < targetAngle)
                    {
                        transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);

                        flyLeft = true;
                    }
                    else
                    {
                        flyStraight = true;
                    }

                    Vector3 xzDirection = transform.forward;

                    xzDirection.y = 0;

                    xzDirection = Vector3.Normalize(xzDirection);

                    rigBod.AddForce(xzDirection * accelration * Time.deltaTime);

                    if (Vector3.Distance(targetLocation, transform.position) < 2)
                    {
                        planeState = PlaneState.IDLE;

                    }

                    break;
                }
            case PlaneState.FORMING:
                {

                    angle = AngleOffTargert(formationTarget);

                    //rotate the plane
                    if (angle > targetAngle)
                    {
                        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

                        flyRight = true;

                    }
                    else if (angle < targetAngle)
                    {
                        transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);

                        flyLeft = true;
                    }
                    else
                    {
                        flyStraight = true;
                    }

                    Vector3 xzDirection = transform.forward;

                    xzDirection.y = 0;

                    xzDirection = Vector3.Normalize(xzDirection);

                    rigBod.AddForce(xzDirection * accelration * Time.deltaTime);

                    if (Vector3.Distance(formationTarget, transform.position) < 1)
                    {
                        planeState = PlaneState.FORMED;
                        rigBod.velocity = Vector3.zero;

                        Destroy(rigBod);
                        formationGroup.EngadgePlane(this);

                    }

                    break;
                }
            case PlaneState.FORMED:
                {
                   
                    break;
                }
        }

        /*if(flyLeft)
        {
            Vector3 planeRot = transform.rotation.eulerAngles;
            float newAngle = Mathf.Lerp(planeRot.z, 20, 0.01f);
            planeRot.z = newAngle;
            planeRot.x = 0;
            transform.rotation = Quaternion.Euler(planeRot);
        }
        if(flyRight)
        {
            Vector3 planeRot = transform.rotation.eulerAngles;
            float newAngle = Mathf.Lerp(planeRot.z, -20, 0.01f);
            planeRot.z = newAngle;
            planeRot.x = 0;
            transform.rotation = Quaternion.Euler(planeRot);
        }
        if(flyStraight)
        {
            Vector3 planeRot = transform.rotation.eulerAngles;
            float newAngle = Mathf.Lerp(planeRot.z, 0, 0.01f);
            planeRot.z = newAngle;
            planeRot.x = 0;
            transform.rotation = Quaternion.Euler(planeRot);
        }*/

        BulletUpdate();


        //check if pane is trying to catch up with its group
        //if so allow it to go faster to visualise the formation
        //else clamp the velocity to the normal max velocity

        if (planeState != PlaneState.FORMED)
        {
            bool speedUp1 = false;
            bool speedUp2 = false;

            if (planeState == PlaneState.FORMING)
            {
                speedUp1 = true;
            }
            if (Vector3.Distance(transform.position, formationTarget) > 0.9)
            {
                speedUp2 = true;
            }

            if (speedUp1 && speedUp2)
            {
                Debug.Log("gota go fast");
                rigBod.velocity = Vector3.ClampMagnitude(rigBod.velocity, speed * 1.5f);
            }
            else if (planeState == PlaneState.FORMED)
            {
                rigBod.velocity = Vector3.ClampMagnitude(rigBod.velocity, 0);
            }
            else
            {
                rigBod.velocity = Vector3.ClampMagnitude(rigBod.velocity, speed);
            }
        }
        

       

    }

    float AngleOffTargert(Vector3 target)
    {
        float angle;
        //find the direction of the plane
        Vector3 targetDir = target - transform.position;

        //find the angle 
        angle = Vector3.Angle(transform.forward, targetDir);

        //find if the angle is negtive or postive
        int sign = Vector3.Cross(transform.forward, targetDir).y < 0 ? -1 : 1;

        //make the angle postive or negtive
        angle *= sign;

        return angle;
    }

    private void BulletUpdate()
    {
        //shot
        if (!ready2Fire)
        {
            shotTimer += Time.deltaTime;

            if (shotTimer > shotSpeed)
            {
                ready2Fire = true;

                shotTimer -= shotSpeed;
            }
        }
    }

    private void FireBullet()
    {
        if(ready2Fire)
        {
            GameObject newBullet = Instantiate(bullet, transform.position,transform.rotation);
            newBullet.GetComponent<BulletScript>().britishBullet = britishPlane;

            audioSource.Play();

            ready2Fire = false;
        }
    }

    public void SetTargetPostion(Vector3 newTarget)
    {
        targetLocation = newTarget;
        planeState = PlaneState.MOVING;
    }

    public void SetTargetPlane(Plane newTarget)
    {
        targetPlane = newTarget;
        planeState = PlaneState.CHASING;


    }

    void OnTriggerEnter(Collider col)
    {

        if(col.GetComponent<BulletScript>())
        {
            if(britishPlane != col.GetComponent<BulletScript>().britishBullet)
            health.TakeDamage(4);
            Destroy(col.gameObject);
        }
    }
}
