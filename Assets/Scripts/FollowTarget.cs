using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = followTarget.position + offset;
    }
}
