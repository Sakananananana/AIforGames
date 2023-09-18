using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalWander : MonoBehaviour
{
    public Rigidbody rb;
    public float maxSpeed = 0;
    public GameObject WanderAi;
    Vector3 targetPos;
    Vector3 targetDir;
    public int force;
    public GameObject healingMeat;

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
    public float circleDist = 0;
    public float circleRad = 1f;

    public bool isDead;
    public bool isWalking;
 
     public float timeSinceLastDirectionChange;
     private float changeDirectionInterval = 5.0f;
    // Start is called before the first frame update
     void Start()
    {
        rb = GetComponent<Rigidbody>();
        accelRate = maxSpeed / accelTimeToMax;
        isDead = false;
        isWalking = false;
    }



    // Update is called once per frame
    void Update()
    {
        
        

        //wander part
        timeSinceLastDirectionChange += Time.deltaTime;
          if(timeSinceLastDirectionChange >= changeDirectionInterval) 
        {
       
        
        

        //set direction
        var vector2 = Random.insideUnitCircle.normalized * circleRad;
        //change direction if near altar
        if(vector2.y < 15 || vector2.y >-5 || vector2.x < 10 || vector2.x > -10 )
        {

            vector2 = Random.insideUnitCircle.normalized * circleRad;
        }


        circleCircumPos = new Vector3(vector2.x,0,vector2.y);
        targetPos =circlePos + circleCircumPos;

         
        targetDir = (targetPos - WanderAi.transform.position).normalized;
        targetDir.y = 0;
        timeSinceLastDirectionChange = 0.0f;
       }
 
        //Debug.Log(targetDir);
        rb.AddForce(transform.up * gravity * Time.deltaTime);
        circlePos = WanderAi.transform.position + ((curDir * currentSpeed).normalized) * circleDist;
        Debug.Log(isWalking);
        if(isWalking == false)
        {
        StartCoroutine(WanderAround());
isWalking = false;

        }
        curDir = Vector3.RotateTowards(WanderAi.transform.forward,targetDir, 5*Time.deltaTime, 0.0f);
        //Debug.Log(curDir);
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
        // end wandering

        
        if(isDead == true)
        {

            amDead();
        }

        Debug.DrawRay(WanderAi.transform.position, targetDir, Color.black);
        Debug.DrawRay(WanderAi.transform.position, WanderAi.transform.forward, Color.green);
        Debug.DrawRay(WanderAi.transform.position, steerDir, Color.red);

    }
    
    IEnumerator WanderAround()
    {
        //wandering movement
        
        isWalking = true;
        yield return new WaitForSeconds(3f);    
        
        
        maxSpeed = 6;
        isWalking = false;
                
       
       

    }
    
   
 void amDead()
 {
    Destroy(gameObject);
    Instantiate(healingMeat, WanderAi.transform.position, Quaternion.identity);

 }


      private void OnTriggerEnter(Collider other)
      {
        if (other.CompareTag("Sword"))
        {
        isDead = true;
        }
      }

}
