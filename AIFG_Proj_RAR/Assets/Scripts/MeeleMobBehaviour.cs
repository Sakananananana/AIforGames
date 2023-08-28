using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleMobBehaviour : MonoBehaviour
{
    DamageSystem dmgSys;
    Rigidbody rb;

    public Transform target;
    private float maxSpeed = 5.0f;
    private float speed = 2.5f;
    private float steeringForce = 5.0f;
    private float accelerationTimeToMax = 3.0f;
    private float accelerationRate;
    //public int isChase;
    private Vector3 targetPos;
    private Vector3 targetDir;
    private Vector3 currentDir;
    private Vector3 steerDir;

    void Start()
    {
       dmgSys = GetComponent<DamageSystem>();
       rb = GetComponent<Rigidbody>();

        if (dmgSys != null)
        { 
            dmgSys.Initialize();
        }

        accelerationRate = maxSpeed / accelerationTimeToMax;
    }

    void Update()
    {
        ChaseBehave();
    }

    public void TakeDamage(int damage)
    { 
        if (dmgSys != null) 
        {
            dmgSys.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        { 
            TakeDamage(10);
            Debug.Log(dmgSys.currentHealth);
        }
    }

    void ChaseBehave()
    {
        //desired 
        targetPos = target.position;
        targetDir = (target.position - transform.position).normalized;
        targetDir.y = 0;

        //current
        currentDir = Vector3.RotateTowards(transform.forward, targetDir, 5 * Time.deltaTime, 2.0f);
        transform.rotation = Quaternion.LookRotation(currentDir);
        Debug.Log(currentDir);

        if (speed < maxSpeed)
        {
            speed = speed + accelerationRate * Time.deltaTime;
        }
        else
        {
            speed = maxSpeed;
        }

        steerDir = targetDir - currentDir;
        rb.velocity = (speed * currentDir) + (steerDir * steeringForce);
    }
}
