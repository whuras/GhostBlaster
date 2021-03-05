using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitation : MonoBehaviour
{
    [Header("Parameters")]
    public float gravityMultiplier = 1f;
    public float gravityWellRadius;

    private void FixedUpdate()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, gravityWellRadius))
        {
            if (col.transform.tag == "Player")
            {
                Vector3 velocity = new Vector3((transform.position.x - GameObject.Find("Player").transform.position.x), 0, (transform.position.z - GameObject.Find("Player").transform.position.z));
                col.GetComponent<CharacterController>().Move(velocity * gravityMultiplier * Time.deltaTime);
            }
        }
    }
}
