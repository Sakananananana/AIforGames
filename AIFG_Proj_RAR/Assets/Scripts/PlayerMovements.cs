using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.AI;
using Debug = UnityEngine.Debug;

public class PlayerMovements : MonoBehaviour
{
    Animator anim;
    public Rigidbody rb;
    DamageSystem dmgSys;
    PauseBehaviour pause;
    MeeleMobBehaviour mob;

    private BoxCollider boxCollider;
    public Transform cam;
    public GameObject sword;
    public GameObject pauseMenu;

    public int damage;
    public int HP;
    public bool isDead;
    public float speed = 6f;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float gravity = 9.8f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        mob = GetComponent<MeeleMobBehaviour>();

        boxCollider = sword.GetComponent<BoxCollider>();
        isDead = false;
        HP = 1000;
        damage = 15;
    }

    // Update is called once per frame
    void Update()
    {
        pause = pauseMenu.GetComponent<PauseBehaviour>();

        if (pause.paused == false && !isDead)
        {
            boxCollider = sword.GetComponent<BoxCollider>();

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            movementMechanics(horizontal, vertical);

            if (Input.GetMouseButtonDown(0))
            {
                attackMechanics();
            }

            HpMechanics();
        }
    }

    void movementMechanics(float horizontal, float vertical)
    {
        Vector3 direction = new Vector3(horizontal, 0, vertical);


        if (direction.magnitude >= 0.1f)
        {
            anim.SetBool("walking", true);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.velocity = moveDir * speed;
        }
        else
        {
            anim.SetBool("walking", false);
            direction.x = 0f;
            direction.z = 0f;
            direction.y = -1;
            rb.velocity = direction * gravity;
        }
    }

    void attackMechanics()
    {
        anim.SetTrigger("attack");
        boxCollider.enabled = true;
        Invoke("attackOff", 2f);
    }

    void attackOff()
    {
        boxCollider.enabled = false;
    }

    public void HpMechanics()
    {
        if (HP <= 0)
        {
            isDead = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bat"))
        {
            Debug.Log("damaged");
            HP -= 10;
        }
    }

}
