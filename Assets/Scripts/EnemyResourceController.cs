using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResourceController : MonoBehaviour
{
    [Header("Parameters")]
    public int health = 1;
    public int pointValue = 1;
    public float lifetime = 30.0f;

    float timer = 0.0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            if (gameObject.GetComponent<EnemyMovement>())
            {
                if (!gameObject.GetComponent<EnemyMovement>().targetAcquired) // if enemy is chasing player, do not destroy
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) Destroy(gameObject);
    }

    public int GetHealth()
    {
        return health;
    }
    
    public int GetPointValue()
    {
        return pointValue;
    }
}
