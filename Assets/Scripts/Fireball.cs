using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("Parameters")]
    public int damage = 1;
    public float speed = 10f;
    public float lifeTime = 10f;

    float timer = 0f;

    Transform player;
    Vector3 targetDir;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        targetDir = (player.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {   
        transform.position += targetDir * speed * Time.deltaTime;
        timer += Time.deltaTime;
        if(timer > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player.GetComponent<PlayerResourceController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
