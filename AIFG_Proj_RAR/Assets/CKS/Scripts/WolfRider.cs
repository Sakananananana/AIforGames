using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfRider : MonoBehaviour, IFMelee
{
    [Header("Preset")]
    [Tooltip("Preset")]
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _altar;
    DamageSystem dmgSys;


    [Header("Config")]
    [Tooltip("Config")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _ragedMoveSpd = 10f;
    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private float _attackRange = 10f;
    [SerializeField] private float _attackCD = 2f;

    [Header("Debug")]
    [SerializeField] private Rigidbody _wolfRiderRb;
    [SerializeField] private Vector3 _altarDir;
    [SerializeField] private Vector3 _rotateAng;
    [SerializeField] private float _altarDistance;
    [SerializeField] private bool _canAttack;
    [SerializeField] private bool _isAttacking;
    [SerializeField] private bool _isFirstAttacked;
    [SerializeField] private Coroutine _atkCoroutine;

    public static bool _isPatroling;
    // Start is called before the first frame update
    void Start()
    {
        _wolfRiderRb = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _altar = GameObject.FindGameObjectWithTag("Altar").transform;
        dmgSys = GetComponent<DamageSystem>();
        if (dmgSys != null)
        {
            dmgSys.Initialize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _altarDistance = Objects_AI.CalculateDistance(_altar.position, transform.position);

        if(_isAttacking)
        {
            _atkCoroutine = StartCoroutine(StartAttack(_attackCD));
        }
    }
    void FixedUpdate()
    {
        if (_altarDistance > _attackRange)
        {
            _isAttacking = false;
            _altarDir = Objects_AI.CalculateDir(_altar.position, transform.position);
            _rotateAng = Objects_AI.CalRotateAmount(_altarDir, transform.forward);
            _wolfRiderRb.angularVelocity = Objects_AI.AimTarget(_wolfRiderRb, _rotateAng, _rotateSpeed);
            _wolfRiderRb.velocity = new Vector3(_altarDir.x * _moveSpeed, 0, _altarDir.z * _moveSpeed);
            StopAttackCoroutine();
        }
        else
        {
            if(!_canAttack)
            {
                _isAttacking = true;
                _canAttack = true;
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Sword") || collider.CompareTag("AirBlade"))
        {
            if (!_isFirstAttacked)
            {
                _moveSpeed = _ragedMoveSpd;
                _isFirstAttacked = true;
            }

            TakeDamage(10);
            Debug.Log(dmgSys.currentHealth);
        }
    }
    private IEnumerator StartAttack(float cd)
    {
        _isAttacking = false;
        //Enable attack collider and animation
        //Straight away reduce Altar hp by time without collider

        Debug.Log("WolfRider Attack!");
        yield return new WaitForSeconds(cd);
        _isAttacking = true;
    }
    private void StopAttackCoroutine()
    {
        if (_atkCoroutine != null)
        {
            StopCoroutine(_atkCoroutine);
            _atkCoroutine = null;
        }
    }

    public void TakeDamage(int damage)
    {
        if (dmgSys != null)
        {
            dmgSys.TakeDamage(damage);
        }
    }
}
