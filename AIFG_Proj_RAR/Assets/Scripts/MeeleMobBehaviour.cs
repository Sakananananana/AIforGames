using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleMobBehaviour : MonoBehaviour
{
    AttackRangeDetector detect;
    DamageSystem dmgSys;
    Animator anim;
    Rigidbody rb;

    public Transform target;
    public GameObject detector;

    private float maxSpeed = 5.0f;
    private float speed = 2.5f;
    private float steeringForce = 5.0f;
    private float accelerationTimeToMax = 3.0f;
    private float accelerationRate;
    float gravity = 9.8f;

    private Vector3 targetPos;
    private Vector3 targetDir;
    private Vector3 currentDir;
    private Vector3 steerDir;
    private Vector3 downForce = new Vector3 (0, -1, 0);

    public bool isChase;
    public bool inArea = false;
    public bool isAttack = false;


    void Start()
    {
       dmgSys = GetComponent<DamageSystem>();
       anim = GetComponent<Animator>();
       rb = GetComponent<Rigidbody>();

        if (dmgSys != null)
        { 
            dmgSys.Initialize();
        }

        accelerationRate = maxSpeed / accelerationTimeToMax;

        //ChaseBehave();
    }

    void Update()
    {
        detect = detector.GetComponent<AttackRangeDetector>();
        

        if (detect.inRange)
        {
            isAttack = true;
            anim.SetBool("isAttack", true);
        }
        else if (detect.inRange == false)
        {
            anim.SetBool("isAttack", false);
            anim.SetBool("isAttackEnd", true);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("isAttackEnd"))
            {

            }
            else
            {
                anim.SetBool("isAttackEnd", false);
                isAttack = false;
                //Debug.Log("chase again");
            }
        }

        if (!isAttack)
        {
            ChaseBehave();
        }
        else 
        {
        
        }
    }

    public void TakeDamage(int damage)
    { 
        if (dmgSys != null) 
        {
            dmgSys.TakeDamage(damage);
        }
    }

    private void ChaseBehave()
    {
        //desired 
        targetPos = target.position;
        targetDir = (target.position - transform.position).normalized;
        targetDir.y = 0;

        //current
        currentDir = Vector3.RotateTowards(transform.forward, targetDir, 5 * Time.deltaTime, 2.0f);
        transform.rotation = Quaternion.LookRotation(currentDir);

        if (speed < maxSpeed)
        {
            speed = speed + accelerationRate * Time.deltaTime;
        }
        else
        {
            speed = maxSpeed;
        }

        steerDir = targetDir - currentDir;
        rb.velocity = (speed * currentDir) + (steerDir * steeringForce) + (downForce * gravity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            TakeDamage(10);
            Debug.Log(dmgSys.currentHealth);
        }
    }
}
