using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int attackDamage;

    public void Initialize()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        //this.gameObject.SetActive(false);
        Debug.Log("Enemy Died");
    }
}
