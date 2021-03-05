using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Parameters")]
    public int damage = 1;
    public float movementSpeed = 0f;
    public float roundSpeedIncrease = 2f;
    public float followRange = 0f;
    public int maxIdlePosition = 25; // size of floor +/-

    [Header("Toggles")]
    public bool targetAcquired = false;
    public bool flashed = false;

    float tempMoveSpeed = 0f;

    Vector3 idleTargetPosition;
    Transform player;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }
    void Start()
    {
        SetNewIdlePosition();
        tempMoveSpeed = movementSpeed;
    }

    void Update()
    {
        movementSpeed = tempMoveSpeed + (roundSpeedIncrease * (float)FindObjectOfType<GameMasterController>().GetRound());

        float targetDistance = Vector3.Distance(transform.position, player.position);

        if (targetDistance < followRange)
        {
            targetAcquired = true;
        }

        if (flashed)
        {
            transform.rotation = Quaternion.LookRotation(-(player.position - transform.position), Vector3.up);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z), -1 * movementSpeed * Time.deltaTime);
        }
        else if (targetAcquired)
        {
            transform.rotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z), movementSpeed * Time.deltaTime);
        }
        else
        {
            if(Vector3.Distance(transform.localPosition, idleTargetPosition) < 0.001f)
            {
                SetNewIdlePosition();
            }
            else
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, idleTargetPosition, movementSpeed * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(idleTargetPosition - transform.localPosition, Vector3.up);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Floor")
        {
            transform.SetParent(other.transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            player.GetComponent<PlayerResourceController>().TakeDamage(damage);
        }
    }

    public void SetNewIdlePosition()
    {
        idleTargetPosition = new Vector3(Random.Range(-maxIdlePosition,maxIdlePosition), transform.localPosition.y, Random.Range(-maxIdlePosition, maxIdlePosition));
    }

}
