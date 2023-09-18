using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonF : MonoBehaviour, IFMelee
{
    public Transform _followingPosTf;
    [SerializeField] private Transform _playerTf;
    
    [Tooltip("Config")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _attackCD;
    [SerializeField] private float _atkRange;
    [SerializeField] private float _chaseRange;

    [Space(30f)]
    [Header("Debug")]
    [Tooltip("Debug")]
    public GameObject _skeletonL_GO;
    [SerializeField] private Rigidbody _skeletonF_Rb;
    [SerializeField] private Transform _targetTf;
    [SerializeField] private Vector3 _targetDir;
    [SerializeField] private float _targetDist;
    [SerializeField] private float _playerDist;
    [SerializeField] private Vector3 _rotateAng;
    [SerializeField] private bool _isAttacking;
    [SerializeField] private int _currentAction;
    // Start is called before the first frame update
    void Start()
    {
        _playerTf = GameObject.FindGameObjectWithTag("Player").transform;

        _skeletonF_Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        SwitchAction();
        _targetDist = Objects_AI.CalculateDistance(_targetTf.transform.position, transform.position);
        _playerDist = Objects_AI.CalculateDistance(_playerTf.transform.position, transform.position);

        if (_playerDist > _chaseRange && _skeletonL_GO != null) // If player not in range, then follow the formation
        {
            _targetTf = _followingPosTf;
        }
        else if (_playerDist <= _chaseRange && _skeletonL_GO != null) // If player in range, then chase and attack player
        {
            _targetTf = _playerTf;
        }
        else if (_skeletonL_GO == null)
        {
            _targetTf = _playerTf;
        }

    }

    private void FixedUpdate()
    {
        SwitchAction2();
    }

    private void CheckState()
    {
        if(_targetTf == _followingPosTf)
        {
            _currentAction = 0;
        }
        else if (_targetTf == _playerTf && _targetDist > _atkRange)
        {
            _currentAction = 0;
        }
        else if (_targetTf == _playerTf && _targetDist <= _atkRange)
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
                _targetDir = Objects_AI.CalculateDir(_targetTf.position, transform.position);
                _rotateAng = Objects_AI.CalRotateAmount(_targetDir, transform.forward);
                _skeletonF_Rb.angularVelocity = Objects_AI.AimTarget(_skeletonF_Rb, _rotateAng, _rotateSpeed);
                _skeletonF_Rb.velocity = new Vector3(_targetDir.x * _moveSpeed, 0, _targetDir.z * _moveSpeed);
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
}
