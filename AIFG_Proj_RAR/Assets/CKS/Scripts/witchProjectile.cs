using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class witchProjectile : MonoBehaviour
{
    public Transform _pjtTarget;

    [Header("Config")]
    [Tooltip("Config")]
    public float _pjtDamage;
    [SerializeField] private float _pjtMoveSpd = 2f;
    [SerializeField] private float _pjtRotateSpd = 10f;

    [Space(30f)]
    [Header("Debug")]
    [Tooltip("Debug")]
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _playerTempPos;
    [SerializeField] private Vector3 _pjtDirection;
    [SerializeField] private Vector3 _pjtRotateAng;
    [SerializeField] private Rigidbody _pjtRb;
    [SerializeField] private bool _isFollowing = true;
    // Start is called before the first frame update
    void Start()
    {
        _pjtRb = GetComponent<Rigidbody>();
        _pjtDirection = Objects_AI.CalculateDir(_pjtTarget.position, transform.position);
        Destroy(gameObject, 10f);
    }

    void FixedUpdate()
    {
        _pjtRotateAng = Objects_AI.CalRotateAmount(_pjtDirection, transform.forward);
        _pjtRb.angularVelocity = Objects_AI.AimTarget(_pjtRb, _pjtRotateAng, _pjtRotateSpd);
        _pjtRb.velocity = _pjtDirection * _pjtMoveSpd;
        //transform.position = Vector3.MoveTowards(transform.position, _player.position, _pjtMoveSpd * Time.deltaTime);
        //transform.LookAt(_player);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Altar") || collider.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }
}
