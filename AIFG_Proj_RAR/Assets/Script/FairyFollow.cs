using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyFollow : MonoBehaviour
{
   public Rigidbody rb;
    private float maxSpeed = 4;
    public GameObject player;
    public GameObject SeekAi;
    Vector3 targetPos;
    Vector3 targetDir;
    public int force;

    public float angularVel = 1;
    public float accelRate;
    public float accelTimeToMax = 3f;
    public float currentSpeed;
    Vector3 steerDir;
    Vector3 curDir;

    public float steerForce = 1f;
    private float gravity = -9.8f;
    public PlayerMovements playerMVM;
    int distance = 5;
    bool fairyCorout;
    // Start is called before the first frame update
   void Start()
    {
        rb = GetComponent<Rigidbody>();
        //playerMVM = gameObject.GetComponent<PlayerMovements>();
        accelRate = maxSpeed / accelTimeToMax;
        fairyCorout = true;


    }

    // Update is called once per frame
    void Update()
    {
        if(fairyCorout == true)
        {
        StartCoroutine(fairyActions());
        }
        rb.AddForce(transform.up * gravity * Time.deltaTime);
        targetPos = player.transform.position;
        targetDir = (targetPos - SeekAi.transform.position).normalized;
        targetDir.y = 0;

        curDir = Vector3.RotateTowards(SeekAi.transform.forward,targetDir, 3*Time.deltaTime, 0.0f);
        SeekAi.transform.rotation = Quaternion.LookRotation(curDir);



    if(Vector3.Distance(SeekAi.transform.position, player.transform.position)<=distance)
    {
        currentSpeed = 0;
    }
    else
    {
        {
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

    }
}
        

        Debug.DrawRay(SeekAi.transform.position, targetDir, Color.black);
        Debug.DrawRay(SeekAi.transform.position, SeekAi.transform.forward, Color.green);
        Debug.DrawRay(SeekAi.transform.position, steerDir, Color.red);

    }


    IEnumerator fairyActions()
    {
        
        
            int actionNo = (Random.Range(0,12))%3;

            if (actionNo == 0)
            {

             playerMVM.HP += 10;
             Debug.Log("Health");
               
            } 

            if(actionNo == 1)
            {

              playerMVM.speed +=4f;
               Debug.Log("Speed");
               
            }

            if (actionNo == 2)
            {

             playerMVM.damage *=2;
              Debug.Log("Damage");
               

            }
            fairyCorout = false;
        
        yield return new WaitForSeconds(10f);

        if(actionNo == 1)
            {

              playerMVM.speed -=4f;
               Debug.Log("Speed");
               
            }

            if (actionNo == 2)
            {

             playerMVM.damage /=2;
              Debug.Log("Damage");
               

            }

        fairyCorout = true;

    }
}
