using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour, IFRanged
{
    [Header("Preset")]
    [Tooltip("Preset")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _pjtPrefab;
    [SerializeField] private GameObject _skeletonF_Prefab;
    [SerializeField] private GameObject _skeletonL_Prefab;
    
    [Space(30f)]
    [Header("Config")]
    [Tooltip("Config")]
    //[SerializeField] private List<GameObject> _skeletons;
    [SerializeField] private int _summonCount;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _attackCD;
    [SerializeField] private float _atkRange;
    
    [Space(30f)]
    [Header("Debug")]
    [Tooltip("Debug")]
    [SerializeField] private GameObject _skeletonL_GO;
    [SerializeField] private Rigidbody _witchRb;
    [SerializeField] private Transform _targetTf;
    [SerializeField] private Transform _playerTf;
    [SerializeField] private Transform _altarTf;
    [SerializeField] private Vector3 _targetDir;
    [SerializeField] private float _altarDist;
    [SerializeField] private float _playerDist;
    [SerializeField] private float _targetDist;
    [SerializeField] private Vector3 _rotateAng;
    [SerializeField] private SkeletonL _skeletonL_Class;
    [SerializeField] private bool _canSpawn;
    [SerializeField] private bool _isFiring;
    [SerializeField] private int _atkCount;
    [SerializeField] private int _currentAction;
    
    // Start is called before the first frame update
    void Start()
    {
        _canSpawn = false;
        _atkCount = 0;
        _currentAction = 0;
        _witchRb = GetComponent<Rigidbody>();
        _playerTf = GameObject.FindGameObjectWithTag("Player").transform;
        _altarTf = GameObject.FindGameObjectWithTag("Altar").transform;

    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        SwitchAction();
        _playerDist = Objects_AI.CalculateDistance(_playerTf.position, transform.position);
        _altarDist = Objects_AI.CalculateDistance(_playerTf.position, transform.position);

        if(_playerDist <= _altarDist)
        {
            _targetDist = _playerDist;
            _targetTf = _playerTf;
        }
        else
        {
            _targetDist = _altarDist; 
            _targetTf = _altarTf;
        }
    }

    private void FixedUpdate()
    {
        SwitchAction2();
    }
    private void CheckState()
    {
        // Summon skeletons after 3 attacks
        if (_atkCount == 3)
        {
            _canSpawn = true;
        }

        if(_targetDist > _atkRange && !_canSpawn)
        {
            _currentAction = 0;
        }
        else if(_targetDist <= _atkRange && !_canSpawn)
        {
            _currentAction = 1;
        }
        else if (_canSpawn) // Summon 3 skeletons
        {
            _currentAction = 2;
        }
    }

    private void SwitchAction()
    {
        switch(_currentAction)
        {
            case 1: // Stop moving and attack
                if(!_isFiring)
                {
                    StartCoroutine(FireAttack(_attackCD));
                }
                break;

            case 2: // Summon skeletons
                
                for (int i = 0; i < _summonCount; i++)
                {
                    if (i == 0)
                    {
                        _skeletonL_GO = Instantiate(_skeletonL_Prefab, _spawnPoint.position, transform.rotation);
                        _skeletonL_Class = _skeletonL_GO.GetComponent<SkeletonL>();
                    }
                    else
                    {
                        GameObject tempS_FGO = Instantiate(_skeletonF_Prefab, _spawnPoint.position, transform.rotation);
                        tempS_FGO.SetActive(false);
                        var fClass = tempS_FGO.GetComponent<SkeletonF>();
                        fClass._skeletonL_GO = _skeletonL_GO;

                        if (_skeletonL_Class._formationPoss[i] != null)
                        {
                            tempS_FGO.transform.position = _skeletonL_Class._formationPoss[i].position;
                            fClass._followingPosTf = _skeletonL_Class._formationPoss[i];
                        }
                    }
                }

                _canSpawn = false;
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
                _witchRb.angularVelocity = Objects_AI.AimTarget(_witchRb, _rotateAng, _rotateSpeed);
                _witchRb.velocity = new Vector3(_targetDir.x * _moveSpeed, 0, _targetDir.z * _moveSpeed);
                break;
        }
    }

    private IEnumerator FireAttack(float cd)
    {
        _isFiring = true;
        GameObject tempPjtGO = Instantiate(_pjtPrefab, _firePoint.position, _firePoint.rotation);
        tempPjtGO.GetComponent<witchProjectile>()._pjtTarget = _targetTf;
        if(_atkCount < 3)
        {
            _atkCount++;
        }
        else
        {
            _atkCount = 0;
        }
        yield return new WaitForSeconds(cd);
        _isFiring = false;
    }
}
