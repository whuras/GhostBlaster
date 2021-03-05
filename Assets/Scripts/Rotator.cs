using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    Vector3 rotationAngle = new Vector3(0.0f, 25.0f, 0.0f);
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationAngle * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
