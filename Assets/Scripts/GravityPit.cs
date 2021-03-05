using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPit : MonoBehaviour
{
    public float lifeDuration = 30.0f;
    float timer = 0.0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= lifeDuration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            FindObjectOfType<GameMasterController>().GameOver();
        }
    }
}
