using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarDamage : MonoBehaviour
{
    DamageSystem dmgSys;
    public static bool isDead;

    private void Start()
    {
        dmgSys = GetComponent<DamageSystem>();
       
        if (dmgSys != null)
        {
            dmgSys.Initialize();
        }
    }

    private void Update()
    {
        if (dmgSys.currentHealth < 0)
        {
            isDead = true;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyProjectile"))
        {
            Debug.Log("damaged by EnemyPjt");
            dmgSys.TakeDamage(5);
        }

        if (other.CompareTag("Bat"))
        {
            dmgSys.TakeDamage(10);
        }
    }
}
