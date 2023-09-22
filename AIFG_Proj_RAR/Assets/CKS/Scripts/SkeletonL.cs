using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonL : MonoBehaviour, IFMelee
{
    public List<Transform> _formationPoss;
    DamageSystem dmgSys;

    [Header("Config")]
    [Tooltip("Config")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _attackCD;
    [SerializeField] private float _atkRange;

    [Space(30f)]
    [Header("Debug")]
    [Tooltip("Debug")]
    [SerializeField] private Rigidbody _skeletonL_Rb;
    [SerializeField] private GameObject _target;
    [SerializeField] private Vector3 _targetDir;
    [SerializeField] private float _targetDist;
    [SerializeField] private Vector3 _rotateAng;
    [SerializeField] private bool _isAttacking;
    [SerializeField] private int _currentAction;

    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _skeletonL_Rb = GetComponent<Rigidbody>();
        dmgSys = GetComponent<DamageSystem>();
        if (dmgSys != null)
        {
            dmgSys.Initialize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _targetDist = Objects_AI.CalculateDistance(_target.transform.position, transform.position);
        CheckState();
        SwitchAction();
    }

    private void FixedUpdate()
    {
        SwitchAction2();
    }
    private void CheckState()
    {
        if (_targetDist > _atkRange)
        {
            _currentAction = 0;
        }
        else if (_targetDist <= _atkRange)
        {
            _currentAction = 1;
        }
    }
    private void SwitchAction()
    {
        switch (_currentAction)
        {
            case 1: // Stop moving and attack
                if (!_isAttacking)
                {
                    StartCoroutine(MeleeAttack(_attackCD));
                }
                break;
        }
    }

    private void SwitchAction2()
    {
        switch (_currentAction)
        {
            case 0:
                _targetDir = Objects_AI.CalculateDir(_target.transform.position, transform.position);
                _rotateAng = Objects_AI.CalRotateAmount(_targetDir, transform.forward);
                _skeletonL_Rb.angularVelocity = Objects_AI.AimTarget(_skeletonL_Rb, _rotateAng, _rotateSpeed);
                _skeletonL_Rb.velocity = new Vector3(_targetDir.x * _moveSpeed, 0, _targetDir.z * _moveSpeed);
                break;
        }
    }
    private IEnumerator MeleeAttack(float cd)
    {
        //melee Attack
        _isAttacking = true;
        yield return new WaitForSeconds(cd);
        _isAttacking = false;
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
        if (other.CompareTag("Sword") || other.CompareTag("AirBlade"))
        {
            TakeDamage(10);
            Debug.Log(dmgSys.currentHealth);
        }
    }
}
