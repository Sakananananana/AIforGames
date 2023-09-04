using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AirBladeAttack : MonoBehaviour
{
    [SerializeField] private Transform[] _firepoints;
    [SerializeField] private GameObject _airBladePrefab;
    [SerializeField] private GameObject[] _airBladeGOs;
    public List<GameObject> _rangedEnemiesInRange = new();
    public List<GameObject> _meleeEnemiesInRange = new();
    [SerializeField] private bool _canAirBlade;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_rangedEnemiesInRange.Count == 0 && _meleeEnemiesInRange.Count == 0)
        {
            _canAirBlade = false;
        }
        else
        {
            _canAirBlade = true;
        }

        //if can AirBlade then show UI

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (_canAirBlade)
            {
                if (_airBladeGOs == null)
                {
                    for (int i = 0; i < _firepoints.Length; i++)
                    {
                        _airBladeGOs[i] = Instantiate(_airBladePrefab, _firepoints[i].position, _firepoints[i].rotation);
                    }
                }
                else
                {
                    for (int i = 0; i < _firepoints.Length; i++)
                    {
                        _airBladeGOs[i].transform.position = _firepoints[i].position;
                        _airBladeGOs[i].SetActive(true);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IFRanged>() != null)
        {
            _rangedEnemiesInRange.Add(other.gameObject);
        }
        else if(other.GetComponent<IFMelee>() != null)
        {
            _meleeEnemiesInRange.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IFRanged>() != null)
        {
            if (_rangedEnemiesInRange != null)
            {
                _rangedEnemiesInRange.Remove(other.gameObject);
            }
        }
        else if (other.GetComponent<IFMelee>() != null)
        {
            if (_meleeEnemiesInRange != null)
            {
                _meleeEnemiesInRange.Remove(other.gameObject);
            }
        }
    }
}
