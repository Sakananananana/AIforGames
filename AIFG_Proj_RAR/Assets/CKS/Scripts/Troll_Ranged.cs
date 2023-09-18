using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll_Ranged : MonoBehaviour, IFRanged
{
    [Header("Preset")]
    [Tooltip("Preset")]
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject[] _patrolPoints;
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private DamageSystem _damageSystem;

    [Header("Config")]
    [Tooltip("Config")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private float _patrolDistanceGap = 0.1f;
    [SerializeField] private float _attackRange = 10f;
    [SerializeField] private float _attackCD = 2f;
    [SerializeField] private float _fleeDuration = 2f;

    [Header("Debug")]
    [SerializeField] private Rigidbody _rangedTrollRb;
    [SerializeField] private Vector3 _currentPatrolPt;
    [SerializeField] private Vector3 _patrolDir;
    [SerializeField] private Vector3 _rotateAng;
    [SerializeField] private Vector3 _playerDir;
    [SerializeField] private float _playerDistance;
    [SerializeField] private float _patrolDistance;
    [SerializeField] private List<int> _numList = new();
    [SerializeField] private Coroutine _atkCoroutine;
    [SerializeField] private Coroutine _fleeCoroutine;
    [SerializeField] private bool _isFiring;
    [SerializeField] private bool _isAttacking;
    [SerializeField] private bool _isFleeing;
    [SerializeField] private bool _isAttacked;

    public static bool _isPatroling;

    //public static bool _isFleeing;

    // Start is called before the first frame update
    void Start()
    {
        _damageSystem = GetComponent<DamageSystem>();
        _damageSystem.Initialize();
        _patrolPoints = new GameObject[4];
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoints");
        _currentPatrolPt = _patrolPoints[0].transform.position;
        _rangedTrollRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _playerDistance = Objects_AI.CalculateDistance(_player.position, transform.position);
        _patrolDistance = Objects_AI.CalculateDistance(_currentPatrolPt, transform.position);

        if(_playerDistance <= _attackRange)
        {
            if (!_isFiring && !_isFleeing)
            {
                _atkCoroutine = StartCoroutine(ShootFireball(_attackCD));
            }
        }
        else
        {
            _isFiring = false;
            _isAttacking = false;
        }
        if(_isAttacked)
        {
            _fleeCoroutine = StartCoroutine(Flee(_fleeDuration));
        }
    }
    private void FixedUpdate()
    {
        if (!_isFleeing && !_isAttacking)
        {
            StopFleeCoroutine();
            StopShootFireballCoroutine();

            if (_patrolDistance > _patrolDistanceGap)
            {
                _patrolDir = Objects_AI.CalculateDir(_currentPatrolPt, transform.position);
                _rotateAng = Objects_AI.CalRotateAmount(_patrolDir, transform.forward);
                _rangedTrollRb.angularVelocity = Objects_AI.AimTarget(_rangedTrollRb, _rotateAng, _rotateSpeed);
                _rangedTrollRb.velocity = new Vector3(_patrolDir.x * _moveSpeed, 0, _patrolDir.z * _moveSpeed);

            }
            else
            {
                _currentPatrolPt = _patrolPoints[RandomNum()].transform.position;
            }
        }
        else if (!_isFleeing && _isAttacking)
        {
            StopFleeCoroutine();

            _patrolDir = Objects_AI.CalculateDir(_currentPatrolPt, transform.position);
            _rotateAng = Objects_AI.CalRotateAmount(_patrolDir, transform.forward);
            _rangedTrollRb.angularVelocity = Objects_AI.AimTarget(_rangedTrollRb, _rotateAng, _rotateSpeed);
            _rangedTrollRb.velocity = Vector3.zero;
        }
        else if(_isFleeing)
        {
            StopShootFireballCoroutine();

            _playerDir = Objects_AI.CalculateDir(_player.position, transform.position);
            _rotateAng = Objects_AI.CalRotateAmount(_playerDir, -transform.forward);
            _rangedTrollRb.angularVelocity = Objects_AI.AimTarget(_rangedTrollRb, _rotateAng, _rotateSpeed);
            _rangedTrollRb.velocity = new Vector3(-_playerDir.x * _moveSpeed, 0, -_playerDir.z * _moveSpeed);
        }

    }

    private int RandomNum()
    {
        int randomNum = 0;
        int randomIndex = 0;

        if(_currentPatrolPt == _patrolPoints[0].transform.position)
        {
            _numList = new() { 1, 2, 3 };
            randomIndex = Random.Range(0, _numList.Count);
            randomNum = _numList[randomIndex];

            return randomNum;
        }
        else if (_currentPatrolPt == _patrolPoints[1].transform.position)
        {
            _numList = new() { 0, 2, 3 };
            randomIndex = Random.Range(0, _numList.Count);
            randomNum = _numList[randomIndex];

            return randomNum;
        }
        else if (_currentPatrolPt == _patrolPoints[2].transform.position)
        {
            _numList = new() { 0, 1, 3 };
            randomIndex = Random.Range(0, _numList.Count);
            randomNum = _numList[randomIndex];

            return randomNum;
        }
        else if (_currentPatrolPt == _patrolPoints[3].transform.position)
        {
            _numList = new() { 0, 1, 2 };
            randomIndex = Random.Range(0, _numList.Count);
            randomNum = _numList[randomIndex];

            return randomNum;
        }

        return randomNum;
    }

    private IEnumerator ShootFireball(float cd)
    {
        _isFiring = true;
        _isAttacking = true;
        Instantiate(_fireballPrefab, _firePoint.position, _firePoint.rotation);
        Debug.Log("Attack!");
        yield return new WaitForSeconds(cd);
        _isFiring = false;
    }
    private IEnumerator Flee(float cd)
    {
        _isAttacked = false;
        _isFleeing = true;
        Debug.Log("Flee!");
        yield return new WaitForSeconds(cd);
        _isFleeing = false;
    }
    private void StopShootFireballCoroutine()
    {
        if (_atkCoroutine != null)
        {
            StopCoroutine(_atkCoroutine);
            _atkCoroutine = null;
        }
    }
    private void StopFleeCoroutine()
    {
        if (_fleeCoroutine != null)
        {
            StopCoroutine(_fleeCoroutine);
            _fleeCoroutine = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Sword") || other.CompareTag("AirBlade"))
        {
            _damageSystem.TakeDamage(10);
            _isAttacked = true;
        }
    }
}
