using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Parameters")]
    public int damage = 1;
    public float range = 50.0f;

    [Header("Laser")]
    public GameObject laserPrefab;
    public GameObject endPoint;
    int hitCounter = 0;
    bool shootNow = false;
    GameObject spawnedLaser;
    WaitForSeconds laserTime = new WaitForSeconds(0.1f);

    GameObject player;
    GameMasterController gameMaster;

    private void Start()
    {
        player = GameObject.Find("Player");
        gameMaster = FindObjectOfType<GameMasterController>();

        // Laser Setup
        spawnedLaser = Instantiate(laserPrefab, endPoint.transform) as GameObject;
        spawnedLaser.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shootNow = true;
        }
    }

    private void FixedUpdate()
    {
        if (shootNow)
        {
            shootNow = false;
            HandleShooting();
        }
    }

    void HandleShooting()
    {
        StartCoroutine(Laser());
        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Mathf.Infinity);

        int enemyCount = 0;
        int points = 0;

        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].transform.tag == "Enemy")
            {
                enemyCount += 1;
                points += hits[i].transform.GetComponent<EnemyResourceController>().GetPointValue();
                hits[i].transform.GetComponent<EnemyResourceController>().TakeDamage(damage);
            }
        }

        if (enemyCount == 0)
        {
            hitCounter = 0; // reset hit counter
            gameMaster.IncrementScore(-1);
            player.GetComponent<PlayerResourceController>().IncreaseFear(1);
            FindObjectOfType<Dog>().StartFade();
        }
        else if(enemyCount > 1)
        {
            gameMaster.IncrementScore(points + 5);
        }
        else
        {
            gameMaster.IncrementScore(points);
        }

        gameMaster.IncrementEnemiesKilled(enemyCount);
        hitCounter += enemyCount;

        if(hitCounter >= 5)
        {
            hitCounter = 0;
            player.GetComponent<PlayerResourceController>().Heal(1);
        }
    }
    
    private IEnumerator Laser()
    {
        spawnedLaser.SetActive(true);
        spawnedLaser.transform.position = endPoint.transform.position;
        yield return laserTime;
        spawnedLaser.SetActive(false);
    }
}
