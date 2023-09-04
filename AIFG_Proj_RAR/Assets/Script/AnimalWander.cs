using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalWander : MonoBehaviour
{
     public Rigidbody rb;
    public float maxSpeed = 6;
    public GameObject WanderAi;
    Vector3 targetPos;
    Vector3 targetDir;
    public int force;

    Vector3 wamderDir;
    float timer = 3f;

    public float angularVel = 1;
    public float accelRate;
    public float accelTimeToMax = 3f;
    public float currentSpeed;
    Vector3 steer;
    Vector3 steerDir;
    Vector3 desirVelocity;
    Vector3 curDir;   
    Vector3 lookDir;

    public float steerForce = 1f;
    private float gravity = -9.8f;

    Vector3 circlePos;
    Vector3 circleCircumPos;
    public float circleDist = 2f;
    public float circleRad = 1f;
    // Start is called before the first frame update
     void Start()
    {
        rb = GetComponent<Rigidbody>();
        accelRate = maxSpeed / accelTimeToMax;

    }





    // Update is called once per frame
    void Update()
    {

        rb.AddForce(transform.up * gravity * Time.deltaTime);
        circlePos = WanderAi.transform.position + ((curDir * currentSpeed).normalized) * circleDist;
        


        var vector2 = Random.insideUnitCircle.normalized * circleRad;
        circleCircumPos = new Vector3(vector2.x,0,vector2.y);
        targetPos =circlePos + circleCircumPos;
        
        targetDir = (targetPos - WanderAi.transform.position).normalized;
        targetDir.y = 0;

        curDir = Vector3.RotateTowards(WanderAi.transform.forward,targetDir, 3*Time.deltaTime, 0.0f);
        WanderAi.transform.rotation = Quaternion.LookRotation(curDir);

        if(currentSpeed < maxSpeed)
        {

            currentSpeed = currentSpeed + accelRate * Time.deltaTime;
        }
        else
        {
            currentSpeed = maxSpeed;

        }

        steerDir = targetDir - curDir;
        rb.velocity = (currentSpeed * curDir) + (steerDir *steerForce);


        Debug.DrawRay(WanderAi.transform.position, targetDir, Color.black);
        Debug.DrawRay(WanderAi.transform.position, WanderAi.transform.forward, Color.green);
        Debug.DrawRay(WanderAi.transform.position, steerDir, Color.red);

    }

}
