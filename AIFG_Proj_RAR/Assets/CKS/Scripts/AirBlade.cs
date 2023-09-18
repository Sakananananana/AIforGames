using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBlade : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("Config")]
    public float _pjtDamage;
    [SerializeField] private float _pjtMoveSpd = 2f;
    [SerializeField] private float _pjtRotateSpd = 10f;
    [SerializeField] private GameObject _target;
    
    [Header("Debug")]
    [Tooltip("Debug")]
    public Character_AirBladeAttack _playerAttack;
    [SerializeField] private Rigidbody _pjtRb;
    [SerializeField] private Vector3 _pjtDirection;
    [SerializeField] private Vector3 _pjtRotateAng;
    [SerializeField] private float _meleeEnemyNum;
    [SerializeField] private float _rangedEnemyNum;
    [SerializeField] private List<int> _numList = new();

    private void OnEnable()
    {
        _pjtRb = GetComponent<Rigidbody>();

        //Chase a random target from the List of enemies within range, prior ranged enemy
        ////_playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<Character_AirBladeAttack>();
        _meleeEnemyNum = _playerAttack._meleeEnemiesInRange.Count;
        _rangedEnemyNum = _playerAttack._rangedEnemiesInRange.Count;

        if (_rangedEnemyNum < 1)
        {
            _target = _playerAttack._meleeEnemiesInRange[RandomNum()];
        }
        else
        {
            _target = _playerAttack._rangedEnemiesInRange[RandomNum()];
        }

        _pjtRb = GetComponent<Rigidbody>();
        
        if (_target != null)
        {
            _pjtDirection = Objects_AI.CalculateDir(_target.transform.position, transform.position);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _pjtRb = GetComponent<Rigidbody>();

        if (_rangedEnemyNum < 1)
        {
            _target = _playerAttack._meleeEnemiesInRange[RandomNum()];
        }
        else
        {
            _target = _playerAttack._rangedEnemiesInRange[RandomNum()];
        }

        if (_target != null)
        {
            _pjtDirection = Objects_AI.CalculateDir(_target.transform.position, transform.position);
        }
        else
        {
            gameObject.SetActive(false);
        }
        
        //Chase a random target from the List of enemies within range, prior ranged enemy

    }

    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            _pjtDirection = Objects_AI.CalculateDir(_target.transform.position, transform.position);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        _pjtRotateAng = Objects_AI.CalRotateAmount(_pjtDirection, transform.forward);
        _pjtRb.angularVelocity = Objects_AI.AimTarget(_pjtRb, _pjtRotateAng, _pjtRotateSpd);
        _pjtRb.velocity = _pjtDirection * _pjtMoveSpd;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _target)
        {
            gameObject.SetActive(false);
            //Deal damage to target
        }
    }

    private int RandomNum()
    {
        int randomNum = 0;
        int randomIndex = 0;
        _numList = new();

        if (_rangedEnemyNum < 1)
        {
            for (int i = 0; i < _meleeEnemyNum; i++)
            {
                _numList.Add(i);
                randomIndex = Random.Range(0, _numList.Count);
                randomNum = _numList[randomIndex];
            }
            return randomNum;
        }
        else
        {
            for (int i = 0; i < _rangedEnemyNum; i++)
            {
                _numList.Add(i);
                randomIndex = Random.Range(0, _numList.Count);
                randomNum = _numList[randomIndex];
            }

            return randomNum;
        }
    }
}
