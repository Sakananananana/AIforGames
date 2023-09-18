using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleMobBehaviour : MonoBehaviour
{
    #region Global Variable Declaration
    //Script Calling
    PlayerMovements player;
    AttackRangeDetector detect;
    DamageSystem dmgSys;
    Animator anim;
    Rigidbody rb;

    //Game Object & Transform References
    public Transform target;
    public GameObject detector;

    //Float Declaration
    private float charPos;
    private float speed = 2.5f;
    private float gravity = -9.8f;
    private float maxSpeed = 5.0f;
    private float steeringForce = 7.5f;
    private float accelerationTimeToMax = 3.0f;
    private float distBetweenObst;
    private float distBetweenNorm;
    private float accelerationRate;

    //Vector3 Declaration
    private Vector3 avoidPos;
    private Vector3 hitPtPos;
    private Vector3 steerDir;
    private Vector3 targetPos;
    private Vector3 targetDir;
    private Vector3 currentDir;
    private Vector3 groundCheckPos = new Vector3(0, 0.5f, 0);

    //Boolean & Layer Mask Declaration
    public bool inArea = false;
    public bool isAttack = false;
    public bool hitTarget = false;
    public bool isGrounded = false;
    public LayerMask ground;
    public LayerMask wall;
    #endregion

    void Start()
    {
        detect = detector.GetComponent<AttackRangeDetector>();
        player = GetComponent<PlayerMovements>();
        dmgSys = GetComponent<DamageSystem>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (dmgSys != null)
        { 
            dmgSys.Initialize();
        }

        accelerationRate = maxSpeed / accelerationTimeToMax;
    }

    void Update()
    {
        if (detect.inRange)
        {
            isAttack = true;
            if (isAttack)
            {
                anim.SetBool("isAttack", true);
            }
        }
        else if (detect.inRange == false && isAttack)
        {
            anim.SetBool("isAttack", false);
            anim.SetBool("isAttackEnd", true);
            StartCoroutine(isAttackManager());
        }

        if (!isAttack)
        {
            anim.SetBool("isAttack", false);
            GroundCheck();
            SeekBehave();
        }
    }
   
    private void SeekBehave()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 5.0f, wall))
        {
            if (hit.transform.CompareTag("Obstacle"))
            {
                hitPtPos = hit.point;
                avoidPos = hit.point + hit.normal * 5f;
                Debug.DrawLine(transform.position, avoidPos);
            }
            hitTarget = true;
        }
        else
        {
            targetPos = target.position;
        }

        if (hitTarget)
        {
            targetPos = avoidPos;
            distBetweenObst = (transform.position - hitPtPos).magnitude;
            distBetweenNorm = (avoidPos - transform.position).magnitude;
            charPos = transform.position.magnitude;
        }
        if (distBetweenObst >= 4f)
        {
            hitTarget = false;
            targetPos = target.position;
            hitPtPos = Vector3.zero;
            avoidPos = Vector3.zero;
        }

        //desired 
        targetDir = (targetPos - transform.position).normalized;
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
        rb.AddForce(steerDir * steeringForce);
        if (isGrounded)
        {
            rb.velocity = (speed * currentDir);
        }
        else
        {
            rb.velocity = (speed * currentDir);
            rb.velocity = Vector3.up * gravity;
        }
    }

    private void GroundCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position - groundCheckPos, 0.15f, ground);

        if (colliders.Length > 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    IEnumerator isAttackManager()
    {
        yield return new WaitForSeconds(4.5f);
        anim.SetBool("isAttackEnd", false);
        isAttack = false;
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
}
