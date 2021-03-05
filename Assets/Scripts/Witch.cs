using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    [Header("Fireball")]
    public GameObject fireballPrefab;
    public float fireballTimer = 1.0f;
    float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        Transform player = GameObject.Find("Player").transform;
        transform.rotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);

        timer += Time.deltaTime;
        if(timer >= fireballTimer)
        {
            timer = 0.0f;
            GameObject fireball = Instantiate(fireballPrefab, transform.position, transform.rotation);
        }
    }
}
