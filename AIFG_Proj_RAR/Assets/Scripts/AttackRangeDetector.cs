using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeDetector : MonoBehaviour
{
    public bool inRange = false;
    bool detected = false;
    

    public void Update()
    {
        if (detected == true)
        {
            inRange = true;
        }
        else if (detected == false)
        { 
            inRange = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detected = false;
        }
    }

}
