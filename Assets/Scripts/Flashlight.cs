using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    Vector3 lightPos;

    private void Start()
    {
        lightPos = transform.localPosition;
        transform.localPosition = new Vector3(0, -100, 0);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.localPosition = lightPos;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            transform.localPosition = new Vector3(0, -100, 0);
            transform.GetChild(0).gameObject.SetActive(false);            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Bat"))
        {
            other.GetComponent<EnemyMovement>().flashed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Bat"))
        {
            other.GetComponent<EnemyMovement>().flashed = false;
        }
    }

}
