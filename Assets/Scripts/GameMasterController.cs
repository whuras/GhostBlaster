using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMasterController : MonoBehaviour
{
    [Header("UI Things")]
    public GameObject gameOver;
    public GameObject pauseMenu;
    public Text currentHealthText, totalHealthText, scoreText, roundText, enemiesText;
    bool paused = false;

    [SerializeField]
    int enemiesPerRound = 10;
    int enemiesKilled, round, score;
    public int activeFloor = 4;
    FloorController floorController;

    [Header("Enemy Prefabs")]
    public GameObject batPrefab;
    public GameObject ghostPrefab;
    public GameObject witchPrefab;
    public GameObject blackHolePrefab;
    public GameObject tornadoPrefab;

    [Header("Spawn Timers")]
    public float spawnRateMultiplier = 0.9f;
    public float spawnRate = 1.0f;
    public float batTimer = 3.0f;
    public float ghostTimer = 4.0f;
    public float witchTimer = 30.0f;
    public float blackHoleTimer = 15.0f;
    public float tornadoTimer = 15.0f;
    int batCount = 1, ghostCount = 1, witchCount = 1, blackHoleCount = 1, tornadoCount = 1;
    float masterTimer = 0f; // delay spawn start

    [Header("Cameras")]
    [SerializeField]
    public Camera FPSCamera;
    public Camera topDownCamera;
    public Camera cinematicCamera;
    WaitForSeconds cinemaTime = new WaitForSeconds(2.0f);

    [Header("OffScreen Indicator Prefab")]
    public GameObject indicator;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        floorController = FindObjectOfType<FloorController>();

        StartCoroutine(Cinematic());
    }

    // Update is called once per frame
    void Update()
    {
        masterTimer += Time.deltaTime;

        if (masterTimer >= batCount * batTimer * spawnRate)
        {
            batCount += 1;
            SpawnEnemy(batPrefab, Quaternion.identity, 3.5f);
        }
        if (masterTimer >= ghostCount * ghostTimer * spawnRate)
        {
            ghostCount += 1;
            SpawnEnemy(ghostPrefab, Quaternion.identity, 1.45f);
        }
        if (masterTimer >= witchCount * witchTimer)
        {
            witchCount += 1;
            SpawnEnemy(witchPrefab, Quaternion.identity, 5f);
        }
        if (masterTimer >= blackHoleCount * blackHoleTimer * spawnRate)
        {
            blackHoleCount += 1;
            SpawnEnemy(blackHolePrefab, Quaternion.Euler(90, 0, 0), 0.0001f);
        }
        if (masterTimer >= tornadoCount * tornadoTimer * spawnRate)
        {
            tornadoCount += 1;
            SpawnEnemy(tornadoPrefab, Quaternion.identity, 0.0f);
        }

        // UI
        if (Input.GetKeyUp(KeyCode.Escape)) PauseToggle();
        
        currentHealthText.text = (player.GetComponent<PlayerResourceController>().health * 10).ToString();
        totalHealthText.text = "/ " + (player.GetComponent<PlayerResourceController>().maxHealth * 10).ToString();
        scoreText.text = score.ToString() + " ";
        roundText.text = "ROUND " + round.ToString() + "  ";
        enemiesText.text = enemiesKilled.ToString() + "/" + enemiesPerRound + " ENEMIES  ";

        // Camera
        if (Input.GetKeyUp(KeyCode.C))
        {
            FPSCamera.enabled = !FPSCamera.enabled;
            topDownCamera.enabled = !topDownCamera.enabled;
            indicator.SetActive(!indicator.activeSelf);
        }
    }

    void SpawnEnemy(GameObject prefab, Quaternion rotationSpawn, float yPos)
    {
        GameObject randomFloor, spawnPoints, randomSpawn, enemy;
        int randNum = 0;

        do
        {
            randNum = Random.Range(0, 9);
            randomFloor = floorController.GetFloors()[randNum];
        } while (randomFloor.name.Contains(activeFloor.ToString()));

        spawnPoints = randomFloor.transform.Find("SpawnPoints").gameObject;
        randomSpawn = spawnPoints.transform.GetChild(Random.Range(0, spawnPoints.transform.childCount)).gameObject;
        enemy = Instantiate(prefab, new Vector3(randomSpawn.transform.position.x, yPos, randomSpawn.transform.position.z), rotationSpawn, randomFloor.transform);
    }
    
    public void IncrementScore(int amount)
    {
        score += amount;
    }
    
    public void IncrementEnemiesKilled(int amount)
    {
        enemiesKilled += amount;
        if(enemiesKilled >= enemiesPerRound)
        {
            enemiesKilled = 0;
            round += 1;
            spawnRate *= spawnRateMultiplier;
        }
    }

    public int GetRound()
    {
        return round;
    }

    public void PauseToggle()
    {
        if (paused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
            paused = false;
        }
        else if (!paused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
            paused = true;
        }
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameOver.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator Cinematic()
    {
        FPSCamera.enabled = false;
        topDownCamera.enabled = false;
        cinematicCamera.enabled = true;

        yield return cinemaTime;

        FPSCamera.enabled = true;
        cinematicCamera.enabled = false;
        player.GetComponent<PlayerMovement>().allowMovement = true;
    }

    public void SetActiveFloor(string Direction)
    {
        switch (Direction)
        {
            case "N":
                if (activeFloor < 3) activeFloor += 6;
                else activeFloor -= 3;
                break;
            case "S":
                if (activeFloor > 5) activeFloor -= 6;
                else activeFloor += 3;
                break;
            case "W":
                if (activeFloor == 0 || activeFloor == 3 || activeFloor == 6) activeFloor += 2;
                else activeFloor -= 1;
                break;
            case "E":
                if (activeFloor == 2 || activeFloor == 5 || activeFloor == 8) activeFloor -= 2;
                else activeFloor += 1;
                break;
            default:
                Debug.Log("Error oin SetActiveFloor");
                break;
        }

    }
}
