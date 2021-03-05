using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    [Header("Parameters")]
    public float lifeDuration = 10.0f;
    float timer = 0.0f;

    GameObject player;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= lifeDuration)
        {
            if (transform.GetChild(0).childCount > 0 && transform.GetChild(0).GetChild(0).tag == "Player")
            {
                transform.GetChild(0).GetChild(0).transform.parent = null;
            }
            Destroy(gameObject);
        }
    }
}
