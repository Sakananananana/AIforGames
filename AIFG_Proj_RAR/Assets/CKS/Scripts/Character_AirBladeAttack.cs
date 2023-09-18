using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AirBladeAttack : MonoBehaviour
{
    [Header("Preset")]
    [Tooltip("Preset")]
    [SerializeField] private Transform[] _firepoints;
    [SerializeField] private GameObject _airBladePrefab;
    [SerializeField] private float _skillCD;


    [Header("Debug")]
    [Tooltip("Debug")]
    public List<GameObject> _airBladeGOs;
    public List<GameObject> _rangedEnemiesInRange = new();
    public List<GameObject> _meleeEnemiesInRange = new();
    [SerializeField] private bool _canAirBlade;
    [SerializeField] private float _cdCount;


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
        if(_cdCount < _skillCD)
        {
            _cdCount += Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (_cdCount >= _skillCD)
            {
                if (_canAirBlade)
                {
                    if (_airBladeGOs.Count < _firepoints.Length)
                    {
                        for (int i = 0; i < _firepoints.Length; i++)
                        {
                            GameObject tempGO = Instantiate(_airBladePrefab, _firepoints[i].position, _firepoints[i].rotation);
                            tempGO.GetComponent<AirBlade>()._playerAttack = this;
                            _airBladeGOs.Add(tempGO);
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

                _cdCount = 0;
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
